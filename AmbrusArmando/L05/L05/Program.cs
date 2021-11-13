using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L05.Repository;

namespace L05
{
    class Program
    {
        private static StudentsRepository studentsRepository;
        private static MetricRepository metricRepository;

        static void Main(string[] args)
        {
            studentsRepository = new StudentsRepository();
            metricRepository = new MetricRepository(studentsRepository.GetAllStudents().Result);
            metricRepository.GenerateMetric();
        }
    }
}
