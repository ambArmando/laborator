using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;

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
            var insertOperation = TableOperation.Insert(student);
            await studentsTable.ExecuteAsync(insertOperation);
        }
    }
}
