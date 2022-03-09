using AzureConsmosDBAPIDemo.Model;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureConsmosDBAPIDemo
{
    class SamplesUtils
    {
        public static async Task<CustomerEntity> InsertOrMergeEntityAsync(CloudTable table, CustomerEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                // 创建 InsertOrReplace 表操作
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // 执行操作。
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                CustomerEntity insertedCustomer = result.Result as CustomerEntity;

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("请求 InsertOrMerge 操作的费用：" + result.RequestCharge);
                }

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        public static async Task<CustomerEntity> RetrieveEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                CustomerEntity customer = result.Result as CustomerEntity;
                if (customer != null)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.Email, customer.PhoneNumber);
                }

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("检索操作的请求费用：" + result.RequestCharge);
                }

                return customer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        public static async Task DeleteEntityAsync(CloudTable table, CustomerEntity deleteEntity)
        {
            try
            {
                if (deleteEntity == null)
                {
                    throw new ArgumentNullException("deleteEntity");
                }

                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                TableResult result = await table.ExecuteAsync(deleteOperation);

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("请求删除操作费用：" + result.RequestCharge);
                }

            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        //  </DeleteItem>

        /// <summary>
        /// 检查给定的连接字符串是否用于 Azure 表存储或 Azure CosmosDB 表。
        /// </summary>
        /// <returns>如果 azure cosmosdb 表为真</returns>
        public static bool IsAzureCosmosdbTable()
        {
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;
            return !String.IsNullOrEmpty(storageConnectionString) && (storageConnectionString.Contains("table.cosmosdb") || storageConnectionString.Contains("table.cosmos"));
        }
    }
}
