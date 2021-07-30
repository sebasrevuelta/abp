﻿using System.Collections.Generic;
using JetBrains.Annotations;
using RabbitMQ.Client;

namespace Volo.Abp.RabbitMQ
{
    public class QueueDeclareConfiguration
    {
        [NotNull] public string QueueName { get; }

        public string DeadLetterQueueName { get; set; }

        public bool Durable { get; set; }

        public bool Exclusive { get; set; }

        public bool AutoDelete { get; set; }

        public IDictionary<string, object> Arguments { get; }

        public string DelayQueueNamePrefix { get; set; } = "Delay.";

        public QueueDeclareConfiguration(
            [NotNull] string queueName,
            bool durable = true,
            bool exclusive = false,
            bool autoDelete = false,
            string deadLetterQueueName = null)
        {
            QueueName = queueName;
            DeadLetterQueueName = deadLetterQueueName;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = new Dictionary<string, object>();
        }

        public virtual QueueDeclareOk Declare(IModel channel)
        {
            return channel.QueueDeclare(
                queue: QueueName,
                durable: Durable,
                exclusive: Exclusive,
                autoDelete: AutoDelete,
                arguments: Arguments
            );
        }

        public virtual QueueDeclareOk DeclareDelay(IModel channel,string delayToExchangeName="")
        {
            // be dead letter queue name 
            var beDeadLetterQueueName = DelayQueueNamePrefix + QueueName;

            Arguments.Add("x-dead-letter-exchange", delayToExchangeName);
            Arguments.Add("x-dead-letter-routing-key", QueueName);
            
            return channel.QueueDeclare(
                queue: beDeadLetterQueueName,
                durable: Durable,
                exclusive: Exclusive,
                autoDelete: AutoDelete,
                arguments: Arguments
            );
        }
    }
}
