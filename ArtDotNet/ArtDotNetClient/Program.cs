using System;
using System.Linq;
using System.Net;
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

			Console.WriteLine("ArtDotNet Client");
			var controller = new ArtNetController();
			controller.Address = IPAddress.Loopback;
			//controller.Address = IPAddress.Parse("10.0.20.159");

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
