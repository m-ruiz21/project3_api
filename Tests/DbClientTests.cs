using Project2Api.DbTools;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;


namespace Project2Api.Tests
{
    [TestFixture]
    public class DbClientTests
    {
        private IConfiguration _configuration;
        private DbClient _dbClient;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _dbClient = new DbClient(_configuration);
        }

        [Test]
        public async Task ExecuteQueryAsync_WithValidQuery_ReturnsDataTable()
        {
            // Arange
            var query = "SELECT * FROM menu_item";

            // Act
            DataTable result = await _dbClient.ExecuteQueryAsync(query);

            // Assert Not Null
            Assert.IsNotNull(result);

            // Make sure it's a datatable
            Assert.IsInstanceOf<DataTable>(result);

            // Make sure it actually returned something
            Assert.IsTrue(result.Rows.Count > 0);
        }

        [Test]
        public async Task ExecuteNonQueryAsync_WithValidQuery_ReturnsRowsAffected()
        {
            // Arrange 
            var query = "INSERT INTO menu_item_test (name, quantity, price, category) VALUES ('hot dog', 12, 15.0, 'base')";

            // Act 
            var rowsAffected = await _dbClient.ExecuteNonQueryAsync(query);

            // Assert 
            Assert.AreEqual(1, rowsAffected); // Assuming the above query inserts one row

            // clean up
            query = "DELETE FROM menu_item_test WHERE name='hot dog'";
            rowsAffected = await _dbClient.ExecuteNonQueryAsync(query);
            Assert.AreEqual(1, rowsAffected);
        }

    }
}