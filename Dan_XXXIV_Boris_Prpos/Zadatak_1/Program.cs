using System;
using System.Collections.Generic;
using System.Threading;

namespace Zadatak_1
{
    class Program
    {
        //static amount of money that will be shared between all threads
        static int totalAmount = 10000;
        //object used for lock
        static readonly object l = new object();

        static void Main(string[] args)
        {
            //taking input and validating number of users from first ATM
            Console.WriteLine("Welcome to our bank. We have two ATM machines available and 10 000 RSD.\nPlease enter how many people wants to withdraw money from first ATM:");
            int firstPeople;
            bool tryFirst = Int32.TryParse(Console.ReadLine(), out firstPeople);
            while (!tryFirst || firstPeople<0)
            {
                Console.WriteLine("Please enter valid number:");
                tryFirst = Int32.TryParse(Console.ReadLine(), out firstPeople);
            }

            //taking input and validating number of users from first ATM
            Console.WriteLine("Please enter how many people wants to withdraw money from second ATM:\n");
            int secondPeople;
            bool trySecond = Int32.TryParse(Console.ReadLine(), out secondPeople);
            while (!trySecond || secondPeople<0)
            {
                Console.WriteLine("Please enter valid number:");
                trySecond = Int32.TryParse(Console.ReadLine(), out secondPeople);
            }

            //initializing list of threads and radnom generator
            List<Thread> ATM_1 = new List<Thread>();
            List<Thread> ATM_2 = new List<Thread>();
            Random rnd = new Random();

            //creating people(threads) for first ATM
            for (int i = 0; i < firstPeople; i++)
            {
                //creating temp for naming
                int temp = i + 1;
                int amount1 = rnd.Next(100, 10001);
                //initializaing thread and passing method
                Thread t = new Thread(new ThreadStart(() => Withdraw(amount1, Thread.CurrentThread)))
                {
                    //generating threads using temp (basicaly i+1, to avoid 0)
                    Name = string.Format("Thread_First_ATM_" + temp)
                };
                //adding threads to first list
                ATM_1.Add(t);
            }           
            //creating people(threads) for first ATM
            for (int i = 0; i < secondPeople; i++)
            {
                int temp = i + 1;
                int amount2 = rnd.Next(100, 10001);
                Thread t = new Thread(new ThreadStart(() => Withdraw(amount2, Thread.CurrentThread)))
                {
                    Name = string.Format("Thread_Second_ATM_" + temp)
                };
                //adding threads to second list
                ATM_2.Add(t);
            }

            //merging two list together
            ATM_1.AddRange(ATM_2);

            //going through merged list and starting every thread
            for (int i = 0; i < ATM_1.Count; i++)
            {
                ATM_1[i].Start();
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Method that checks if there is enough amount left
        /// </summary>
        /// <param name="payOut"></param>
        /// <param name="t"></param>
        static void Withdraw(int payOut,Thread t)
        {
            //locking method
            lock (l)
            {
                //in case that there is enough money
                if (totalAmount - payOut >= 0)
                {
                    //decreasing amount
                    totalAmount = totalAmount - payOut;
                    //displaying messages
                    Console.WriteLine("Client {0} is trying to withdraw {1} RSD.", t.Name, payOut);
                    Console.WriteLine("Amount left in the bank: {0} RSD.\n", totalAmount);
                }
                //in case that there is not enough money left
                else
                {
                    //displaying messages
                    Console.WriteLine("Client {0} tried to withdraw {1} RSD from the bank, but there is not enough money.\n", t.Name, payOut);
                }
            }
        }
    }
}
