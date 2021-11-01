using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AzureDataStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private StorageAccess storageAccess;

        public StudentsController(StorageAccess storage)
        {
            this.storageAccess = storage;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            return await storageAccess.getAllStudents();
        }

        [HttpPost]
        public async Task Post([FromBody] StudentEntity student)
        {
            await storageAccess.CreateStudent(student);
        }
    }
}