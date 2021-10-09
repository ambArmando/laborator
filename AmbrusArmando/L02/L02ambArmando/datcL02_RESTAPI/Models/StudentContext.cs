using Microsoft.EntityFrameworkCore;

namespace StudentsAPI.Models {

    public class StudentContext : DbContext {

        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students {get; set;}
    
    }

}
