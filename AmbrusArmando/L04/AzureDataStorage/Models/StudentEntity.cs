using Microsoft.WindowsAzure.Storage.Table;


namespace AzureDataStorage
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string row, string partition)
        {
            this.RowKey = row;
            this.PartitionKey = partition;
        }
        public StudentEntity () { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Year { get; set; }
    }
}
