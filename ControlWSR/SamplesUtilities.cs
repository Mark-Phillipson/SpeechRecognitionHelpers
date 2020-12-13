using ControlWSR.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWSR
{
    public class SamplesUtilities
    {
		public static async Task<CustomerEntity> InsertOrMergeEntityAsync(CloudTable table, CustomerEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			try
			{
				// Create the InsertOrReplace table operation
				TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

				// Execute the operation.
				TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
				CustomerEntity insertedCustomer = result.Result as CustomerEntity;

				if (result.RequestCharge.HasValue)
				{
					Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
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
					Console.WriteLine("Request Charge of Retrieve Operation: " + result.RequestCharge);
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
					Console.WriteLine("Request Charge of Delete Operation: " + result.RequestCharge);
				}

			}
			catch (StorageException e)
			{
				Console.WriteLine(e.Message);
				Console.ReadLine();
				throw;
			}
		}
	}
}
