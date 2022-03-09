using System;

namespace AzureConsmosDBAPIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cosmos 表示例");
            BasicSamples basicSamples = new BasicSamples();
            basicSamples.RunSamples().Wait();

            AdvancedSamples advancedSamples = new AdvancedSamples();
            advancedSamples.RunSamples().Wait();

            Console.WriteLine();
            Console.WriteLine("按任何一个键退出");
            Console.Read();
        }
    }
}