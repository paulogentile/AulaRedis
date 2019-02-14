using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Oraculo
{
    class Program
    {
        //private const string RedisConnectionString = "127.0.0.1";
        private const string RedisConnectionString = "40.122.106.36";
        private static ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(RedisConnectionString);
        private static IDatabase db = connection.GetDatabase();

        private const string ChatChannel = "perguntas";
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

            if (arrPergunta.Length > 0)
            {

                var perguntaTratada = arrPergunta[3].Replace("?", "").Trim();
                var arrConta = perguntaTratada.Split('+');
                var resposta = Convert.ToInt32(arrConta[0]) + Convert.ToInt32(arrConta[1]);
                //Random rnd = new Random();

                //List<string> respostas = new List<string>() {
                //     "00000041",
                //     "0000424E",
                //     "00004244",
                //     "00004D53" };

                //var resposta = respostas[rnd.Next(0, respostas.Count)];

                db.HashSet(pergunta, new HashEntry[] { new HashEntry(userName, resposta) });

                Console.WriteLine($"Pergunta: {message}");
                Console.WriteLine($"Resposta: {resposta}");
            }

            Console.CursorTop = initialCursorTop + 1;
            Console.CursorLeft = initialCursorLeft;
        }
    }
}
