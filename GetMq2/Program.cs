using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
namespace GetMq2
{
    class Program
    {
        static void Main(string[] args)
        {

            #region  老代码 直接到队列的   不用设置exchang   直接basicconsume 中的队列名称
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
            //    }
            // }
            #endregion

            #region 订阅
            if (false)
            {
                ConnectionFactory subfactory = new ConnectionFactory();
                subfactory.HostName = "localhost";
                using (IConnection connection = subfactory.CreateConnection())
                {
                    using (IModel model = connection.CreateModel())
                    {
                        model.ExchangeDeclare(
                           exchange: "20181009exchange",
                           type: "fanout",
                           durable: true,
                           autoDelete: false
                           );
                        var queuename = model.QueueDeclare().QueueName;
                        // 把队列 绑定到exchagn  在producter 端不需要知道队列的名称， 生产者 把信息发送到exchang  然后exchange 把所有的消息发送给
                        //  和exchange 绑定的 队列
                        model.QueueBind(
                            queue: queuename,
                            exchange: "20181009exchange",
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
                        while (true)
                        {
                            model.BasicConsume(
                         queue: queuename,
                         autoAck: true,
                         consumer: cusmoer
                         );
                        }

                    }
                }
                Console.ReadKey();
            }
        
            #endregion

            #region  routing
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

                    // 这种队列 失去连接后 会自动删除
                    string qnam = model.QueueDeclare().QueueName;

                    //  可以把一个队列绑定到多个路由规则上
                    model.QueueBind(
                      queue: qnam,
                      exchange: "123935exchange",
                      routingKey: "info",
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
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        }
    }


