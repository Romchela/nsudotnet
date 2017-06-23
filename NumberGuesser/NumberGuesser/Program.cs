using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuesser {

    class Program {
        private static int MAX_VALUE = 100;
        private static int HISTORY_VOLUME = 1000;
        private static string[] ABUSE_PHRASES = {"You are stupid!", "You are not right!"};

        static void Main(string[] args) 
        {
            Console.WriteLine("Hello! Type your name...");
            string userName = Console.ReadLine();
            Random random = new Random();
            int key = random.Next(MAX_VALUE + 1);
            string[] queries = new string[HISTORY_VOLUME];
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

                int prediction = int.Parse(input);
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
                    Console.WriteLine("Time elapsed: {0} minutes {1} seconds", time.Minutes, time.Seconds);
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
                    string abuse = ABUSE_PHRASES[random.Next(ABUSE_PHRASES.Length)];
                    Console.WriteLine(abuse);
                }
            }
        }
    }
}
