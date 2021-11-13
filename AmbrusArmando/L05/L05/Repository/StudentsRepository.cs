using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L05.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace L05.Repository
{
    class StudentsRepository
    {
        private CloudTableClient cloudTableClient;
        private CloudTable studentsTable;
        private string connectionString;

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(connectionString);
            cloudTableClient = account.CreateCloudTableClient();
            studentsTable = cloudTableClient.GetTableReference("StudentsTable");
            await studentsTable.CreateIfNotExistsAsync();
        }

        public StudentsRepository()
        {
            connectionString = "DefaultEndpointsProtocol=https;AccountName=ambdatc;AccountKey=4Q1XKWkWtCcgc2OjJbYMhVKn5Sj679mT0ixkNWNlFIPuicBeLgrSMKw/Lz/dsG+t8DPSNgz9+YR8QaeK6sdOig==;EndpointSuffix=core.windows.net";
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }

        public async Task<List<StudentEntity>> GetAllStudents()
        {
            var students = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                students.AddRange(resultSegment);
            } while (token != null);
            return students;
        }
    }
}
