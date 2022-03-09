using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;

namespace AzureConsmosDBAPIDemo
{
    class Common
    {
        public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("提供的存储帐户信息无效。 请确认 app.config 文件中的 AccountName 和 AccountKey 有效 - 然后重新启动应用程序。");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("提供的存储帐户信息无效。 请确认 app.config 文件中的 AccountName 和 AccountKey 有效 - 然后重新启动示例。");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
        public static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;

            // 从连接字符串中检索存储帐户信息。
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            // 创建用于与表服务交互的表客户端
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("为演示创建一个表");

            // 创建用于与表服务交互的表客户端
            CloudTable table = tableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("创建的表名为：{0}", tableName);
            }
            else
            {
                Console.WriteLine("表 {0} 已存在", tableName);
            }

            Console.WriteLine();
            return table;
        }
    }
}