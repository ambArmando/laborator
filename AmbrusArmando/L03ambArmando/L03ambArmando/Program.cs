using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace L03ambArmando
{
    class Program
    {
        private static DriveApiService driveApiService = new DriveApiService();
        private static IList<Google.Apis.Drive.v3.Data.File> filesList;

        private static void listFolders()
        {
            filesList = driveApiService.ListEntities();
            foreach (var file in filesList)
            {
                Console.WriteLine(file.Name);
            }

        }

        private static void uploadFile() {
            
        
        }

        static void Main(string[] args)
        {
            listFolders();

            Console.ReadKey();
        }
    }
}
