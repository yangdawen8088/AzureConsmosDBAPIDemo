using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureConsmosDBAPIDemo.Model;
using Microsoft.Azure.Cosmos.Table;

namespace AzureConsmosDBAPIDemo
{
    class BasicSamples
    {
        public async Task RunSamples()
        {
            Console.WriteLine("Azure Cosmos DB 表 - 基本示例\n");
            Console.WriteLine();

            string tableName = "demo" + Guid.NewGuid().ToString().Substring(0, 5);

            // 创建或引用现有表
            CloudTable table = await Common.CreateTableAsync(tableName);

            try
            {
                // 演示基本的 CRUD 功能
                await BasicDataOperationsAsync(table);
            }
            finally
            {
                // 删除表
                //await table.DeleteIfExistsAsync();
            }
        }

        private static async Task BasicDataOperationsAsync(CloudTable table)
        {
            // 创建客户实体的实例。 有关实体的描述，请参阅 Model\CustomerEntity.cs。
            CustomerEntity customer = new CustomerEntity("ydw", "ydw")
            {
                Email = "Walter@contoso.com",
                PhoneNumber = "425-555-0101"
            };

            // 演示如何插入实体
            Console.WriteLine("插入一个实体。");
            customer = await SamplesUtils.InsertOrMergeEntityAsync(table, customer);

            // 演示如何通过更改电话号码来更新实体
            Console.WriteLine("使用 InsertOrMerge Upsert 操作更新现有实体。");
            customer.PhoneNumber = "425-555-0105";
            await SamplesUtils.InsertOrMergeEntityAsync(table, customer);
            Console.WriteLine();

            // 演示如何使用点查询读取更新的实体
            Console.WriteLine("读取更新后的实体。");
            customer = await SamplesUtils.RetrieveEntityUsingPointQueryAsync(table, "Harp", "Walter");
            Console.WriteLine();

            // 演示如何删除实体
            Console.WriteLine("删除实体。 ");
            //await SamplesUtils.DeleteEntityAsync(table, customer);
            Console.WriteLine();
        }
    }
}
