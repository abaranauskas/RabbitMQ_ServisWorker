using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RabbitMQ.Consumer.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private ILogger<Worker> _logger;
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private IModel _channel;
        private QueueingBasicConsumer _consumer;
        private const string QueueName = "WorkerQueue_Queue";

        public RabbitMQConsumer(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.BasicQos(0, 1, false);
            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume(QueueName, false, _consumer);
        }

        internal void ProcessMesaages()
        {
            while (true)
            {               
                var ea = _consumer.Queue.Dequeue();
                var properties = ea.BasicProperties;
                var replyPropreties = _channel.CreateBasicProperties();
                replyPropreties.CorrelationId = properties.CorrelationId;

                _logger.LogInformation("----------------------------------------------------------");

                var message = (DMFObject)ea.Body.DeSerialize(typeof(DMFObject));

                var responseBytes = Encoding.UTF8.GetBytes($"Response generated in consumer - {message.FirstName}");
                _channel.BasicPublish("", properties.ReplyTo, replyPropreties, responseBytes);
                _logger.LogInformation($"Message was processed in consumer: {message.FirstName}");

                _channel.BasicAck(ea.DeliveryTag, false);

                _logger.LogInformation("----------------------------------------------------------");
            }
        }
    }
}
