﻿using System;
using System.ComponentModel;

namespace NumberGuesser
{

	class Program
	{
		private static int _maxValue = 100;
		private static int _historyLength = 1000;
		private static string[] _abusePhrases = { "{0}, you are stupid!", "You are not right {0}!", "Can you clarify {0} what you mean?", "You are smart man {0}!" };

		static void Main(string[] args)
		{
			Console.WriteLine("Hello! Type your name...");
			var userName = Console.ReadLine();
			var random = new Random();
			var key = random.Next(_maxValue + 1);
			var queries = new string[_historyLength];
			int qNumber = 0;
			DateTime startTime = DateTime.Now;

			while (true)
			{
				Console.Write("Prediction: ");
				string input = Console.ReadLine();

				if (input == "q")
				{
					Console.WriteLine("Sorry! I go to home...");
					return;
				}

				int prediction;
				if (!int.TryParse(input, out prediction))
				{
					Console.WriteLine("Invalid format.");
					continue;
				}

				if (prediction == key)
				{
					Console.WriteLine("\nYou win!");
					Console.WriteLine("Number of turns: {0}", qNumber);
					Console.WriteLine("History of your turns:");

					for (int i = 0; i < qNumber; i++)
					{
						Console.WriteLine("{0}", queries[i]);
					}

					TimeSpan time = DateTime.Now - startTime;
					Console.WriteLine("Time elapsed: {0} minutes", Math.Truncate(time.TotalMinutes));
					return;
				}
				else if (prediction < key)
				{
					queries[qNumber] = "Greater";
					Console.WriteLine("Greater");
				}
				else
				{
					queries[qNumber] = "Less";
					Console.WriteLine("Less");
				}

				qNumber++;
				if (qNumber % 4 == 0)
				{
					string abuse = _abusePhrases[random.Next(_abusePhrases.Length)];
					Console.WriteLine(abuse, userName);
				}
			}
		}
	}
}
