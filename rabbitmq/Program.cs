using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using System.Collections;

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

            #region  新代码    直接发送到队列的
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
                    while (true)
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
                    // 发布订阅模式 就是广播模式
                    //  在生产者 这端 不需要知道队列代的名称 也不需要知道路由规则，
                    //  只需要定义 exchang   和绑定basicpublish  
                    // 在客户端  定义exchagne  和绑定QueueBind   把队列绑定到exchange 上  然后绑定消费者
                    #region  客户端要绑定的事情
                    //      var cusmoer = new EventingBasicConsumer(model);
                    //cusmoer.Received += (m, ea) =>
                    //{
                    //    var body = ea.Body;
                    //    var message = Encoding.UTF8.GetString(body);
                    //    Console.WriteLine("新消息：{0}", message);
                    //};
                    //while (true)
                    //{
                    //    model.BasicConsume(
                    // queue: queuename,
                    // autoAck: true,
                    // consumer: cusmoer
                    // ); 
                    #endregion

                    //  如果在发送消息的时候 没有队列绑定到exchagn  那么该条信息会被删除。 
                    IBasicProperties pro =   model.CreateBasicProperties();
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

            #region 重新测试 发布订阅的情况
            ConnectionFactory Faccon = new ConnectionFactory(); // 工厂
            Faccon.HostName = "localhost";
            using (IConnection connection=Faccon.CreateConnection()) //创建连接
            {
                using (IModel model=connection.CreateModel()) // 创建一个通道
                {

                    // 设置exchang的属性
                    model.ExchangeDeclare(
                        exchange:"1442exchange",
                        type:ExchangeType.Fanout,// 广播
                          durable: true,
                        autoDelete: false
                        );

                    IBasicProperties pro = model.CreateBasicProperties();
                    pro.DeliveryMode = 2;

                    while (false)
                    {
                        Console.Write("请输入要消息队列信息：");
                        string str = Console.ReadLine();
                        model.BasicPublish(
                            exchange: "1442exchange",
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
                    //  如果在发送消息的时候 没有队列绑定到exchagn  那么该条信息会被删除。 
                    // Direct 直接用 队列的 不会删除
                    for (int i = 0; i < 10; i++)
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


            #region  新的实例
            Uri uri = new Uri("amqp://127.0.0.1:5672/");
            string exchange = "ex1";
            string exchangeType = "direct";
            string routingKey = "m1";
            bool persistMode = true;
            ConnectionFactory cf = new ConnectionFactory();
            cf.UserName = "zz";
            cf.Password = "123456";
            cf.VirtualHost = "dnt_mq";
            cf.RequestedHeartbeat = 0;
            cf.Endpoint = new AmqpTcpEndpoint(uri);
            using (IConnection connection=cf.CreateConnection())
            {
                using (IModel ch=connection.CreateModel())
                {

                    if (exchangeType != null)
                    {
                        ch.ExchangeDeclare(exchange, exchangeType);//,true,true,false,false, true,null);
                        ch.QueueDeclare("q1", true);//true, true, true, false, false, null);
                        ch.QueueBind("q1", "ex1", "m1", null);
                    }
                    IMapMessageBuilder b = new MapMessageBuilder(ch);
                    IDictionary target = (Dictionary<string,object>)b.Headers;
                    target["header"] = "hello world";
                    IDictionary targetBody = (Dictionary<string, object>)b.Body;
                    targetBody["body"] = "daizhj";
                    if (persistMode)
                    {
                        ((IBasicProperties)b.GetContentHeader()).DeliveryMode = 2;
                    }

                    ch.BasicPublish(exchange, routingKey,
                                               (IBasicProperties)b.GetContentHeader(),
                                               b.GetContentBody());
                }
            }




            #endregion

            #region rpc
            //var rpcclient = new RPCClient();
            #endregion
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadKey();
        }
    }
}
