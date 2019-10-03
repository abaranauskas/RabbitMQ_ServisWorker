using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Consumer.RabbitMQ;

namespace RabbitMQ.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string QueueName = "WorkerQueue_Queue";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumer = new RabbitMQConsumer(_logger);
                consumer.CreateConnection();
                consumer.ProcessMesaages();                
            }
        }
    }
}
