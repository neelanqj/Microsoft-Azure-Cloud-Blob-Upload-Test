using System;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using System.IO;
using System.Text;

namespace Azure
{
    class Program
    {
        static string sasUri = "https://testdeploymentautomation.blob.core.windows.net/container1?sv=2016-05-31&ss=bfqt&srt=sco&sp=rwdlacup&se=2017-07-18T07:04:26Z&st=2017-07-17T23:04:26Z&spr=https&sig=xDRb62UH6OSSVvETUGywDEWFcGyAGo1qLcBUHjI5NVQ%3D";

        static void Main(string[] args)
        {
            CloudBlockBlob blob = new CloudBlockBlob(new Uri(sasUri));

            // Create operation: Upload a blob with the specified name to the container.
            // If the blob does not exist, it will be created. If it does exist, it will be overwritten.
            try
            {
                MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes("test"));
                msWrite.Position = 0;
                var ado = new Object();
                using (msWrite)
                {
                    ado = blob.UploadFromStreamAsync(msWrite);
                }
                
                Console.WriteLine("Create operation succeeded for SAS {0}", sasUri);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == 403)
                {
                    Console.WriteLine("Create operation failed for SAS {0}", sasUri);
                    Console.WriteLine("Additional error information: " + e.Message);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                    throw;
                }
            }

            Console.ReadLine();
        }
    }
}