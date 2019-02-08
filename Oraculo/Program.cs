using StackExchange.Redis;
using System;

namespace Oraculo
{
    class Program
    {
        private const string RedisConnectionString = "localhost";
        //private const string RedisConnectionString = "40.77.30.246";
        private static ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(RedisConnectionString);
        private static IDatabase db = connection.GetDatabase();

        private const string ChatChannel = "Perguntas";
        private static readonly string userName = "HanSolo";

        static void Main(string[] args)
        {
            // Create pub/sub
            var pubsub = connection.GetSubscriber();

            // Subscriber subscribes to a channel
            pubsub.Subscribe(ChatChannel, (channel, message) => MessageAction(message));

            Console.WriteLine("Conectado ao Cana Perguntas, aguardando mensagens.");

            // Messaging here
            while (true)
            {
                var resposta = Console.ReadLine();
                db.HashSet(pergunta, new HashEntry[] { new HashEntry(userName, resposta) });
                //pubsub.Publish(ChatChannel, $"{userName}: {Console.ReadLine()}");
            }
        }

        private static string pergunta = "";

        static void MessageAction(string message)
        {
            int initialCursorTop = Console.CursorTop;
            int initialCursorLeft = Console.CursorLeft;

            Console.MoveBufferArea(0, initialCursorTop, Console.WindowWidth, 1, 0, initialCursorTop + 1);
            Console.CursorTop = initialCursorTop;
            Console.CursorLeft = 0;

            // Separa a pergunta do número
            var arrPergunta = message.Split(' ');

            pergunta = arrPergunta[0].Replace(":", "");
            Console.WriteLine(message);

            Console.CursorTop = initialCursorTop + 1;
            Console.CursorLeft = initialCursorLeft;
        }
    }
}
