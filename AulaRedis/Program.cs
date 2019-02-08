using StackExchange.Redis;
using System;

namespace AulaRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            var db = redis.GetDatabase();

            //Console.WriteLine("SET A 1");
            //db.StringSet("A", "1");

            //Console.WriteLine($"GET A: {db.StringGet("A")}");

            //Console.WriteLine("INCR A");
            //db.StringIncrement("A");

            //Console.WriteLine($"GET A: {db.StringGet("A")}");

            //Console.WriteLine("SADD tech SQL");
            //db.SetAdd("tech", "SQL");

            //Console.WriteLine("HSET CLI AA 1");
            //db.HashSet("CLI", new HashEntry[] { new HashEntry("AA", "1") });

            //Console.WriteLine("LPUSH L1 A");
            //db.ListRightPush("L1", "A");

            //Console.WriteLine("LPUSH L1 B");
            //db.ListRightPush("L1", "B");

            var sub = redis.GetSubscriber();
            sub.Subscribe("14NET", (ch, msg) =>
            {
                Console.WriteLine(msg.ToString());
            });

            Console.ReadLine();
        }
    }
}
