﻿using System;
using System.Threading;

using Apache.NMS;
using Apache.NMS.Util;

namespace ActiveMQ.Demo
{
    internal class Program
    {
        protected static AutoResetEvent semaphore = new AutoResetEvent(false);
        protected static ITextMessage message = null;
        protected static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

        protected static void OnMessage(IMessage receivedMsg)
        {
            message = receivedMsg as ITextMessage;
            semaphore.Set();
        }

        private static void Main(string[] args)
        {
            // Example connection strings:
            //    activemq:tcp://activemqhost:61616
            //    stomp:tcp://activemqhost:61613
            //    ems:tcp://tibcohost:7222
            //    msmq://localhost

            var connecturi = new Uri("activemq:tcp://activemqhost:61616");

            Console.WriteLine("About to connect to " + connecturi);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            using (var connection = factory.CreateConnection())
            {
                using (var session = connection.CreateSession())
                {
                    // Examples for getting a destination:
                    //
                    // Hard coded destinations:
                    //    IDestination destination = session.GetQueue("FOO.BAR");
                    //    Debug.Assert(destination is IQueue);
                    //    IDestination destination = session.GetTopic("FOO.BAR");
                    //    Debug.Assert(destination is ITopic);
                    //
                    // Embedded destination type in the name:
                    //    IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");
                    //    Debug.Assert(destination is IQueue);
                    //    IDestination destination = SessionUtil.GetDestination(session, "topic://FOO.BAR");
                    //    Debug.Assert(destination is ITopic);
                    //
                    // Defaults to queue if type is not specified:
                    //    IDestination destination = SessionUtil.GetDestination(session, "FOO.BAR");
                    //    Debug.Assert(destination is IQueue);
                    //
                    // .NET 3.5 Supports Extension methods for a simplified syntax:
                    //    IDestination destination = session.GetDestination("queue://FOO.BAR");
                    //    Debug.Assert(destination is IQueue);
                    //    IDestination destination = session.GetDestination("topic://FOO.BAR");
                    //    Debug.Assert(destination is ITopic);
                    var destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");

                    Console.WriteLine("Using destination: " + destination);

                    // Create a consumer and producer
                    using (var consumer = session.CreateConsumer(destination))
                    {
                        using (var producer = session.CreateProducer(destination))
                        {
                            // Start the connection so that messages will be processed.
                            connection.Start();
                            producer.DeliveryMode = MsgDeliveryMode.Persistent;
                            producer.RequestTimeout = receiveTimeout;

                            consumer.Listener += OnMessage;

                            // Send a message
                            var request = session.CreateTextMessage("Hello World!");
                            request.NMSCorrelationID = "abc";
                            request.Properties["NMSXGroupID"] = "cheese";
                            request.Properties["myHeader"] = "Cheddar";

                            producer.Send(request);

                            // Wait for the message
                            semaphore.WaitOne((int) receiveTimeout.TotalMilliseconds, true);

                            if (message == null)
                            {
                                Console.WriteLine("No message received!");
                            }
                            else
                            {
                                Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                                Console.WriteLine("Received message with text: " + message.Text);
                            }
                        }
                    }
                }
            }
        }
    }
}