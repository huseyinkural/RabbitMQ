using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Producer.Helper
{
    public class RabbitHelper
    {
        static Lazy<IConnection> _connection = new Lazy<IConnection>(CreateConnection);
        public static IConnection GetConnection => _connection.Value;
        static IConnection CreateConnection()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            return connection;
        }
    }
}
