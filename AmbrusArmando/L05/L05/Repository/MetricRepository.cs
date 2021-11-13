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
    class MetricRepository
    {
        private CloudTableClient cloudTableClient;
        private CloudTable cloudTableMetric;
        private string connectionString;
        private List<StudentEntity> students;
        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(connectionString);
            cloudTableClient = account.CreateCloudTableClient();
            cloudTableMetric = cloudTableClient.GetTableReference("Metric");
            await cloudTableMetric.CreateIfNotExistsAsync();
        }
        private async Task InsertMetric(MetricEntity metric)
        {
            var insertOperation = TableOperation.Insert(metric);
            await cloudTableMetric.ExecuteAsync(insertOperation);
        }
        public MetricRepository(List<StudentEntity> students)
        {
            this.students = students;
            connectionString = "DefaultEndpointsProtocol=https;AccountName=ambdatc;AccountKey=4Q1XKWkWtCcgc2OjJbYMhVKn5Sj679mT0ixkNWNlFIPuicBeLgrSMKw/Lz/dsG+t8DPSNgz9+YR8QaeK6sdOig==;EndpointSuffix=core.windows.net";
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }
        public void GenerateMetric()
        {
            MetricEntity metric;
            int count;
            List<string> facultati = new List<string>();
            foreach (var student in students)
            {
                if (!facultati.Contains(student.PartitionKey))
                    facultati.Add(student.PartitionKey);
            }
            foreach (var facultate in facultati)
            {
                count = 0;
                foreach (var student in students)
                {
                    if (student.PartitionKey == facultate)
                        count++;
                }
                metric = new MetricEntity(facultate, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                metric.Count = count;
                Task.Run(async () => { await InsertMetric(metric); })
                        .GetAwaiter()
                        .GetResult();
            }
            count = 0;
            foreach (var student in students)
            {
                count++;
            }
            metric = new MetricEntity("General", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            metric.Count = count;
            Task.Run(async () => { await InsertMetric(metric); })
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
