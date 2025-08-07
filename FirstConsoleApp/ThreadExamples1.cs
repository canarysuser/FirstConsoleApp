using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using static System.Console;

namespace FirstConsoleApp
{
    internal class ThreadExamples1
    {
        internal static void Test()
        {
            ThreadStart ts1 = new ThreadStart(RunWithoutParameters);
            Thread th1 = new Thread(ts1);
            th1.Name = "First";
            th1.Start(); //Physical OS thread is created.
            ThreadStart ts2 = RunWithoutParameters;
            Thread th2 = new Thread(ts2);
            th2.Name = "Second";
            th2.Start();
            Thread th3 = new Thread(() =>
            {
                var name = Thread.CurrentThread.Name;
                WriteLine($"Thread {name} begins execution");
                //Simulate some task 
                Thread.Sleep(millisecondsTimeout: 5000);
                WriteLine($"Resumed. {name} exiting now....");
            });
            th3.Name = "Third";
            th3.Start();
            ParameterizedThreadStart ps1 = new ParameterizedThreadStart(RunWithParameters);
            Thread th4 = new Thread(ps1);
            th4.Name = "Fourth";
            th4.Priority = ThreadPriority.Highest;
            th4.Start(8888); 


            WriteLine("All threads started. Waiting for termination...");
            ReadKey();
        }
        static void RunWithParameters(object state)
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            if (int.TryParse(state.ToString(), out var value))
                WriteLine($"Received number {value} as input.");
            else
                WriteLine($"Received unknown type \"{state}\" as input");

            //Simulate some task 
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Resumed. {name} exiting now....");
        }
        //Method name can be anything - preferred practice is to begin with RunXXXXXX 
        static void RunWithoutParameters()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            //Simulate some task 
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Resumed. {name} exiting now....");
        }
    }
}
