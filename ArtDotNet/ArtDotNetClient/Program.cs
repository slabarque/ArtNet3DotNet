using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using ArtDotNet;

namespace ArtDotNetClient
{
	class MainClass
	{

		public static void Main(string[] args)
		{
			var subUni = 0;
			var running = true;
			var uniCounter = 0;
			var counter = 0;
			var lockobj = new object();
            var ipAddress = ConfigurationManager.AppSettings[$"ipAddress_{Environment.MachineName}"];

            Console.WriteLine("ArtDotNet Client");
			var controller = new ArtNetController();
			controller.Address = !string.IsNullOrEmpty(ipAddress)? IPAddress.Parse(ipAddress): IPAddress.Loopback;

			controller.DmxPacketReceived += (s, p) =>
			{
				counter++;
				if (p.SubUni != subUni)
					return;
				lock (lockobj)
				{
					Console.Clear();
					Console.WriteLine("ArtNet Universe " + subUni);
					Console.WriteLine($"{++uniCounter}/{counter} packets received");

					Console.WriteLine(string.Join(Environment.NewLine, p.Data
						.Select((x, i) => new { Index = i, Value = x })
						.GroupBy(x => x.Index / 24)
						.Select(x => string.Join(", ", x.Select(v => v.Value).ToList()))
						.ToList()));
				}
			};

			controller.Start();

			while (running)
			{
				var key = Console.ReadKey(true);

				if (key.Key == ConsoleKey.UpArrow)
					subUni++;

				if (key.Key == ConsoleKey.DownArrow)
					subUni--;

				if (key.Key == ConsoleKey.Escape)
					running = false;
                uniCounter = 0;

                Console.WriteLine("ArtNet Universe " + subUni);
			}

			controller.Stop();
		}
	}
}
