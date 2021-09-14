using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Producer.Exchanges.Topic;
using RabbitMQ.Producer.Helper;
using RabbitMQ.Producer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQ
{
    public partial class Form1 : Form
    {
        IExchangeFactory exchangeFactory;
        ISendMessage producer;
      
        public Form1()
        {
            InitializeComponent();
        }

        public static ISendMessage CreateSendMessage(string exchangeType)
        {

            switch (exchangeType)
            {
                //case ExchangeType.Direct:
                //    return new DirectMessage();
                //case ExchangeType.Headers:
                //    return new HeadersMessage();
                case ExchangeType.Topic:
                    return new TopicMessage();
                //case ExchangeType.Fanout:
                //    return new FanoutMessage();
                default:
                    throw new Exception("there is no properly exchange type");
            }
        }

        public static IExchangeFactory CreateExchange(string exchangeType)
        {

            switch (exchangeType)
            {
                //case ExchangeType.Direct:
                //    return new DirectExhange();
                //case ExchangeType.Headers:
                //    return new HeadersExchange();
                case ExchangeType.Topic:
                    return new TopicExchange();
                //case ExchangeType.Fanout:
                //    return new FanoutExchange();
                default:
                    throw new Exception("there is no properly exchange type");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            producer.SendMessage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            exchangeFactory = CreateExchange(ExchangeType.Topic);
            exchangeFactory.CreateExChangeAndQueue();
            producer = CreateSendMessage(ExchangeType.Topic);

            CreateConsumer("topic-queue-1");
            CreateConsumer("topic-queue-2");
            CreateConsumer("topic-queue-3");
            
        }

        private void CreateConsumer(string queue)
        {
            var connection = RabbitHelper.GetConnection;
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = StringExtensions.GetString(body.ToArray());
                //var message = body.GetString();
                Debug.WriteLine($"{queue} Received {message}", message);
                SetText(message);
            };
            channel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: consumer);
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text += text;
            }
        }
    }


}
