using System;
using System.Linq;
using System.Reflection;
using DbUp;
namespace DbupTest
{
    class Program
    {
        static int Main(string[] args)
        {
            string connectionString = string.Empty;
            if (args.Any() && !String.IsNullOrEmpty(args.First()))
            {
                connectionString =
                    args.FirstOrDefault();
            }
            else
            {
                throw new Exception("Must pass connection string as first argment");
            }

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                             .SqlDatabase(connectionString)
                             .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                             .LogToConsole()
                             .Build();
            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif                
                return -1;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}