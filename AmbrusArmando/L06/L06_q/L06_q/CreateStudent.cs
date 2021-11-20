using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace L06_q
{
    class CreateStudent
    {
        [Function("CreateStudent")]
        [TableOutput("StudentsTable")]
        public static StudentEntity Run([QueueTrigger("l06queue", Connection = "ambdatc")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("CreateStudent");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);
            return student;
        }
    }
}
