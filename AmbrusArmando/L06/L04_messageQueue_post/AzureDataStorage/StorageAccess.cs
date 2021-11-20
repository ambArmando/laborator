using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using Microsoft.Azure.ServiceBus;

namespace AzureDataStorage
{
    public class StorageAccess
    {
        private CloudTableClient cloudTableClient;
        private CloudTable studentsTable;
        private string conString;

        private async Task initTable()
        {
            var account = CloudStorageAccount.Parse(conString);
            cloudTableClient = account.CreateCloudTableClient();
            studentsTable = cloudTableClient.GetTableReference("StudentsTable");
            await studentsTable.CreateIfNotExistsAsync();
        }

        public StorageAccess(IConfiguration config)
        {
            conString = config.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            Task.Run(async () => { await initTable(); }).GetAwaiter().GetResult();
        }

        public async Task<List<StudentEntity>> getAllStudents()
        {
            var students = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> segment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                students.AddRange(segment);
            } while (token != null);
            return students;
        }

        public async Task CreateStudent(StudentEntity student)
        {
            //var insertOperation = TableOperation.Insert(student);
            //await studentsTable.ExecuteAsync(insertOperation);

            var jsonStudent = JsonConvert.SerializeObject(student);
            var plainText = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String = System.Convert.ToBase64String(plainText);

            Azure.Storage.Queues.QueueClient queueClient = new Azure.Storage.Queues.QueueClient(conString, "l06queue");

            await queueClient.SendMessageAsync(base64String);
        }

        public async Task EditStudent(StudentEntity student)
        {
            var editOperation = TableOperation.Merge(student);

            // Implemented using optimistic concurrency
            try
            {
                await studentsTable.ExecuteAsync(editOperation);
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                    throw new System.Exception("Entitatea a fost deja modificata. Te rog sa reincarci entitatea!");
            }
        }

        public async Task DeleteStudent(string id)
        {
            var parsedId = ParseStudentId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var entity = new DynamicTableEntity(partitionKey, rowKey) { ETag = "*" };

            await studentsTable.ExecuteAsync(TableOperation.Delete(entity));
        }

        private (string, string) ParseStudentId(string id)
        {
            var elements = id.Split('-');

            return (elements[0], elements[1]);
        }
    }
}
