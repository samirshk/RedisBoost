using StackExchange.Redis;
using System.Configuration;
using System.Net;

namespace RedisBoost.ConsoleBenchmark.Clients
{
    public class StackExchangeTestsClient : ITestClient
    {
        private ConnectionMultiplexer _conn;
        private IDatabase _client;
        string RedisConnectionString = ConfigurationManager.ConnectionStrings["RedisConfigurationString"].ConnectionString;


        public void Dispose()
        {
            if (_conn != null)
                _conn.Dispose();
        }

        public void Connect(RedisConnectionStringBuilder connectionString)
        {
            _conn = ConnectionMultiplexer.Connect(RedisConnectionString);
            //new RedisConnection(, allowAdmin: true);
            //_client.Open();
            _client = _conn.GetDatabase();
        }

        public void SetAsync(string key, string value)
        {
            _client.StringSetAsync(key, value);
        }

        public string GetString(string key)
        {
            return _client.StringGet(key);
        }

        public void FlushDb()
        {
            foreach (EndPoint e in _conn.GetEndPoints())
            {
                _conn.GetServer(e).FlushAllDatabasesAsync().Wait();
            }
        }

        public string ClientName
        {
            get { return "StackExchange.Redis"; }
        }

        public void IncrAsync(string key)
        {
            _client.StringIncrementAsync(key);
        }

        public int GetInt(string key)
        {
            return (int)_client.StringGet(key);
        }

        public ITestClient CreateOne()
        {
            return new StackExchangeTestsClient();
        }

        public void Set(string key, string value)
        {
            _client.StringSet(key, value);
        }
    }
}
