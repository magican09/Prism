using System;
using System.ServiceModel;
namespace ProjectServiseHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(ProjectServise.ProjectService)))
            {
                host.Open();
                Console.WriteLine("ProjectService started...");
                Console.ReadLine();
            }
          
        }
    }
}
