using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Producer
{
    public class RabbitMQClient
    {
        private const string WorkerQueueName = "WorkerQueue_Queue";
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private QueueingBasicConsumer _consumer;
        private string _replyQueueName = "Reply_Queue";
        private List<string> _corIds = new List<string>();

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _replyQueueName = _channel.QueueDeclare(_replyQueueName, true, false, false, null);
            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume(_replyQueueName, true, _consumer);
        }

        public void SendMessage(DMFObject message)
        {
            var correlationId = Guid.NewGuid().ToString();
            var properties = _channel.CreateBasicProperties();
            properties.ReplyTo = _replyQueueName;
            properties.CorrelationId = correlationId;
            _corIds.Add(correlationId);


            Task.Factory.StartNew(() =>
            {
                _channel.BasicPublish("", WorkerQueueName, properties, message.Serialize());
                Console.WriteLine($"Object Sent first name {message.FirstName}");

                var responseMessage = _consumer.Queue.Dequeue();

                if (_corIds.Any(x => x == responseMessage.BasicProperties.CorrelationId))
                    Console.WriteLine($"Response received at producer: {Encoding.UTF8.GetString(responseMessage.Body)}");
            });


        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
