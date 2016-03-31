// web job to run daily background task of checking ppr website for updates, downloading if are, saving in database
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;

namespace WebJobWorker
{
    public class Program
    {
        public static void Main()
        {
            var host = new JobHost();
            Task calltask=host.CallAsync(typeof(Functions).GetMethod("ManualTrigger"));
            calltask.Wait();
            Console.ReadKey();
        }
    }
}
