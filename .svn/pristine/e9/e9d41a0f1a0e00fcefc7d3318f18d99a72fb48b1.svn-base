using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MyRegion
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.ExchangeDeclare(
            //     exchange: "myexchange",
            //    type: "direct"
            //     );
            //    channel.QueueDeclare(
            //        queue: "zz",
            //        durable: true,
            //        exclusive: false,
            //        autoDelete: false,

            //        arguments: null);

            //    var consumer = new EventingBasicConsumer(channel);
            //    //注册接收事件，一旦创建连接就去拉取消息
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        var message = Encoding.UTF8.GetString(body);
            //        Console.WriteLine(" [x] Received {0}", message);
            //    };
            //    channel.BasicConsume(queue: "zz",

            //                         autoAck: true,//和tcp协议的ack一样，为false则服务端必须在收到客户端的回执（ack）后才能删除本条消息
            //                         consumer: consumer);

            //    Console.WriteLine(" Press [enter] to exit.");
            //    Console.ReadLine(); 
            #endregion

            #region MyRegion
            //ConnectionFactory newfactory = new ConnectionFactory();
            ////   newfactory.Password = "123456";
            ////  newfactory.VirtualHost = "myhost";
            //newfactory.HostName = "localhost";

            //using (IConnection connetion = newfactory.CreateConnection())
            //{
            //    using (IModel model = connetion.CreateModel())
            //    {
            //        var consumer = new EventingBasicConsumer(model);
            //        consumer.Received += (m, ea) =>
            //        {
            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body);
            //            Console.WriteLine("新队列：{0}", message);
            //        };
            //        while (true)
            //        {
            //            model.BasicConsume(
            //            queue: "1639zz",
            //            autoAck: true,
            //            consumer: consumer
            //            );
            //            Thread.Sleep(2000);
            //        }

            //        #region MyRegion

            //        //BasicGetResult result = model.BasicGet("1639zz", true);
            //        //if (result == null)
            //        //{
            //        //    // No message available at this time.
            //        //}
            //        //else
            //        //{
            //        //    IBasicProperties props = result.BasicProperties;
            //        //    byte[] body = result.Body;

            //        //    Console.WriteLine("新队列： "+System.Text.Encoding.UTF8.GetString(body));
            //        //} 
            //        #endregion

            //    } 

            // }
            #endregion

            #region 订阅
            ConnectionFactory subfactory = new ConnectionFactory();
            subfactory.HostName = "localhost";
            using (IConnection connection = subfactory.CreateConnection())
            {
                using (IModel model = connection.CreateModel())
                {
                    model.ExchangeDeclare(
                       exchange: "1221608exchange",
                       type: "fanout",
                       durable: true,
                       autoDelete: false
                       );
                    var queuename = model.QueueDeclare().QueueName;
                    model.QueueBind(
                        queue: queuename,
                        exchange: "1221608exchange",
                        routingKey: "",
                        arguments: null
                        );

                    var cusmoer = new EventingBasicConsumer(model);
                    cusmoer.Received += (m, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("新消息：{0}", message);
                    };
                    while (false)
                    {
                        model.BasicConsume(
                     queue: queuename,
                     autoAck: true,
                     consumer: cusmoer
                     );
                    }


                }
            }
            #endregion


            #region 路由
            ConnectionFactory routefactory = new ConnectionFactory();
            routefactory.HostName = "localhost";
            using (IConnection connection=routefactory.CreateConnection())
            {
                using (IModel model=connection.CreateModel())
                {
                    model.ExchangeDeclare(
                       exchange: "123935exchange",
                       type: ExchangeType.Direct,
                       durable: true,
                       autoDelete: false,
                       arguments: null
                       );
                    IBasicProperties pro = model.CreateBasicProperties();
                    pro.DeliveryMode = 2;
                    string qnam = model.QueueDeclare().QueueName;

                    model.QueueBind(
                      queue: qnam,
                      exchange: "123935exchange",
                      routingKey: "error",
                      arguments: null
                      );


                    var cusmoer = new EventingBasicConsumer(model);
                    cusmoer.Received += (m, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("新消息：{0}", message);
                    };

                    while (true)
                    {
                        model.BasicConsume(
                      queue: qnam,
                      autoAck: true,
                      consumer: cusmoer
                      );
                    }
                }
            }

            #endregion
        }


        public  static void Prodector()
        {

        }
    }
}
