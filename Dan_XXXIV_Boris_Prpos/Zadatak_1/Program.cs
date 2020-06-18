using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        static int totalAmount = 10000;
        static readonly object l = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to our bank. We have two ATM machines available.\nPlease enter how many people wants to withdraw money from first ATM:");
            int firstPeople = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter how many people wants to withdraw money from second ATM:");
            int secondPeople = Convert.ToInt32(Console.ReadLine());
            int totalThreads = firstPeople + secondPeople;

            List<Thread> ATM_1 = new List<Thread>();
            List<Thread> ATM_2 = new List<Thread>();
            Random rnd = new Random();

            for (int i = 0; i < firstPeople; i++)
            {
                int temp = i + 1;
                int amount1 = rnd.Next(100, 10000);
                Thread t = new Thread(new ThreadStart(() => Withdraw(amount1, Thread.CurrentThread)))
                {
                    Name = string.Format("Thread_First_ATM_" + temp)
                };
                ATM_1.Add(t);
            }
            for (int i = 0; i < secondPeople; i++)
            {
                int temp = i + 1;
                int amount2 = rnd.Next(100, 10000);
                Thread t = new Thread(new ThreadStart(() => Withdraw(amount2, Thread.CurrentThread)))
                {
                    Name = string.Format("Thread_Second_ATM_" + temp)
                };
                ATM_2.Add(t);
            }
            ATM_1.AddRange(ATM_2);

            for (int i = 0; i < ATM_1.Count; i++)
            {
                ATM_1[i].Start();
            }
          
            Console.ReadLine();
        }

        //static int GenerateAmount()
        //{
        //    Random rnd = new Random();
        //    int amount = rnd.Next(100, 10000);
        //    return amount;
        //}
        static void Withdraw(int payOut,Thread t)
        {
            lock (l)
            {
                if (totalAmount - payOut >= 0)
                {
                    totalAmount = totalAmount - payOut;
                    Console.WriteLine("Client {0} is trying to withdraw {1} RSD.", t.Name, payOut);
                    Console.WriteLine("Amount left in the bank: {0} RSD.", totalAmount);
                }
                else
                {
                    Console.WriteLine("Client {0} tried to withdraw {1} RSD from the bank, but there is not enough money.", t.Name, payOut);
                }
            }
        }
    }
}
