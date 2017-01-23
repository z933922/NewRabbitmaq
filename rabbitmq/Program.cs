using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace rabbitmq
{
    class Program
    {
        static void Main(string[] args)
        {
            #region   以前老的代码
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connetion = factory.CreateConnection())
            //{
            //    var channel = connetion.CreateModel();
            //    // 默认是  direct 模式没有exchange name   routekey   和 queue 的name 是一样的
            //    channel.ExchangeDeclare(
            //     exchange: "myexchange",
            //     type: "direct"
            //     );
            //    channel.QueueDeclare(
            //        queue: "zz",
            //        durable: true,
            //        exclusive: false,
            //        autoDelete: false,
            //        arguments: null
            //        );

            //    string message = " 你大爷 你妈卖批";
            //    channel.BasicPublish(
            //        exchange: "",
            //         routingKey: "zz",
            //         basicProperties: null,
            //         body: Encoding.UTF8.GetBytes(message)

            //        );
            //    Console.WriteLine(" [x] Sent {0}", message);
            //}

            #endregion

            #region  新代码  
            ConnectionFactory newfactory = new ConnectionFactory();
            newfactory.HostName = "localhost";
            using (IConnection connetion = newfactory.CreateConnection())
            {
                using (IModel model = connetion.CreateModel())
                {
                    model.ExchangeDeclare(
                        exchange: "1639exchange",
                        type: ExchangeType.Direct,
                        durable: true,
                        autoDelete: false,
                        arguments: null
                        );

                    model.QueueDeclare(
                        queue: "1639zz",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    IBasicProperties props = model.CreateBasicProperties();
                    props.DeliveryMode = 2;

                    // 一直可以 向队列里面写入数据
                    while (false)
                    {
                        Console.Write("输入要发送的队列信息 ：");
                        string shuren = Console.ReadLine();
                        Console.WriteLine("队列信息：" + shuren);
                        System.Threading.Thread.Sleep(3000);
                        model.BasicPublish(
                             exchange: "",
                             routingKey: "1639zz",
                             basicProperties: props,
                             body: Encoding.UTF8.GetBytes(shuren)
                            );

                        Console.WriteLine("发送成功");
                        Console.WriteLine("");
                    }
                }
            }
            #endregion


            #region  发布代码 广播发送的
            ConnectionFactory publicfactory = new ConnectionFactory();
            publicfactory.HostName = "localhost";

            using (IConnection connection=publicfactory.CreateConnection())
            {
                using (IModel model=connection.CreateModel())
                {
                    model.ExchangeDeclare(
                        exchange:"1221608exchange",
                        type:"fanout",
                        durable:true,
                        autoDelete:false
                        );
                  IBasicProperties pro=   model.CreateBasicProperties();
                    pro.DeliveryMode = 2;
                    while (false)
                    {
                        Console.Write("请输入要消息队列信息：");
                        string str = Console.ReadLine();
                        model.BasicPublish(
                            exchange: "1221608exchange",
                            routingKey: "",
                            basicProperties: pro,
                            body: System.Text.Encoding.UTF8.GetBytes(str)
                                );
                        Console.WriteLine("发送成功");
                        Console.WriteLine();
                    }
                }
            }
            #endregion


            #region  路由发送
            ConnectionFactory rfactory = new ConnectionFactory();
            rfactory.HostName = "localhost";
            using (IConnection connection=rfactory.CreateConnection())
            {
                using (IModel model=connection.CreateModel())
                {

                    model.ExchangeDeclare(
                        exchange:"123935exchange",
                        type:ExchangeType.Direct,
                        durable:true,
                        autoDelete:false,
                        arguments:null
                        );
                    IBasicProperties pro = model.CreateBasicProperties();
                    pro.DeliveryMode = 2;

                    for (int i = 0; i < 0; i++)
                    {
                        Console.Write("请输入要消息队列信息：");
                        string str = Console.ReadLine();
                        string routekey = "error";
                        if (i%2==0)
                        {
                            routekey = "info";
                        }
                        model.BasicPublish(
                            exchange: "123935exchange",
                            routingKey: routekey,
                            basicProperties: pro,
                            body: System.Text.Encoding.UTF8.GetBytes(str)
                                );
                        Console.WriteLine("发送成功");
                        Console.WriteLine();
                    }

                }
            }
            #endregion



            #region rpc
            var rpcclient = new RPCClient();
            #endregion
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadKey();
        }
    }
}
