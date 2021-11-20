using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


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

        [HttpPut]
        public async Task<string> Edit([FromBody] StudentEntity student)
        {
            try
            {
                await storageAccess.EditStudent(student);

                return "S-a modificat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete([FromRoute] string id)
        {
            try
            {
                await storageAccess.DeleteStudent(id);

                return "S-a sters cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }

        }
    }
}