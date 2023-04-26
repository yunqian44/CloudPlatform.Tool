using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace CloudPlatform.Tool.StorageAccount.Providers
{

    public class TableStorageService
    {
        const string DefaultPolicyTableName = "EformPolicies";
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;
        private readonly ILogger<AzureBlobStorage> _logger;

        public TableStorageService(ILogger<AzureBlobStorage> logger,
            AzureBlobConfiguration tableConfiguration)
        {
            _logger= logger;
            _tableServiceClient = new (tableConfiguration.ConnectionString);
            _tableClient = _tableServiceClient.GetTableClient(DefaultPolicyTableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<Response> AddEntityAsync(TableInfo entity)
        {
            return await _tableClient.AddEntityAsync(entity);
        }

        public async Task<List<TableInfo>> GetAllEntitiesAsync()
        {
            var entities = new List<TableInfo>();
            try
            {
                var resultEntities = _tableClient.QueryAsync<TableInfo>().AsPages(default);

                await foreach (var item in resultEntities)
                {
                    entities.Add(new TableInfo() { });
                }
            }
            catch (RequestFailedException e)
            {
                _logger.LogWarning($"table not exist.");
            }


            return await Task.FromResult(entities);
        }

        public async Task<TableInfo> GetEntityAsync(string partitionKey, string rowKey)
        {
            var response = await _tableClient.GetEntityAsync<TableInfo>(partitionKey, rowKey);
            return response.Value;
        }

        public async Task UpdateEntityAsync(TableInfo entity)
        {
            await _tableClient.UpdateEntityAsync(entity, ETag.All);
        }

        public async Task DeleteEntityAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
