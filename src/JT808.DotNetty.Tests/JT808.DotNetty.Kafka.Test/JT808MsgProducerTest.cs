﻿using Confluent.Kafka;
using Confluent.Kafka.Admin;
using JT808.DotNetty.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Kafka.Test
{
    public class JT808MsgProducerTest: JT808BaseTest
    {

        public JT808MsgProducerConfig JT808ProducerConfig = new JT808MsgProducerConfig
        {
            TopicName = "JT808Msg",
            BootstrapServers = BootstrapServers
        };

        public JT808MsgProducerTest()
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = BootstrapServers }).Build())
            {
                try
                {
                    adminClient.DeleteTopicsAsync(new List<string>() { JT808ProducerConfig.TopicName }).Wait();
                }
                catch (AggregateException e)
                {
                    //Debug.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
                catch (CreateTopicsException e)
                {
                    Debug.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }

        [Fact]
        public void Test1()
        {
            using (IJT808MsgProducer jT808MsgProducer = new JT808MsgProducer(JT808ProducerConfig))
            {
                jT808MsgProducer.ProduceAsync("123456", new byte[] { 0x7E, 0, 0x7E }).Wait();
            }
        }

        [Fact]
        public void Test2()
        {
            using (IJT808MsgProducer jT808MsgProducer = new JT808MsgProducer(JT808ProducerConfig))
            {
                jT808MsgProducer.ProduceAsync("123457", new byte[] { 0x7E, 0, 0x7E }).Wait();
                jT808MsgProducer.ProduceAsync("123456", new byte[] { 0x7E, 0, 0x7E }).Wait();
            }
        }
    }
}
