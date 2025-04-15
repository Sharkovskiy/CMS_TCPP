using NUnit.Framework;
using System.Data.SqlClient;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class DatabaseIntegrationTests
    {
        private string _connectionString;
        private SqlConnection _connection;

        [SetUp]
        public void Setup()
        {
            _connectionString = "Server=localhost;Database=TestDB;Trusted_Connection=True;";
            _connection = new SqlConnection(_connectionString);
        }

        [TearDown]
        public void TearDown()
        {
            _connection?.Dispose();
        }

        [Test]
        public void DatabaseConnection_CanConnect_ReturnsTrue()
        {
            // Act
            var canConnect = CanConnectToDatabase();

            // Assert
            Assert.That(canConnect, Is.True);
        }

        private bool CanConnectToDatabase()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }
    }
} 