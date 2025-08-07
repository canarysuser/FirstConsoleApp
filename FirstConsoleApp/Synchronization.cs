using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Console; 


namespace FirstConsoleApp
{
    internal class Synchronization
    {
        private int counter = 1; 
        
        public void RunDefault()
        {
            var name = Thread.CurrentThread.Name; 
            var method = nameof(RunDefault);
            WriteLine($"Thread {name} enters {method}");
            while(counter<100)
            {
                int temp = counter;
                temp++;
                Thread.Sleep(1);
                WriteLine($"Thread {name} in {method} reports counter at {counter}");
                counter = temp;
            }
            WriteLine($"Thread {name} exits {method}");
        }
        public void RunInterlocked()
        {
            var name = Thread.CurrentThread.Name;
            var method = nameof(RunInterlocked);
            WriteLine($"Thread {name} enters {method}");
            while (counter < 100)
            {
                Interlocked.Increment(ref counter);
                Thread.Sleep(1);
                WriteLine($"Thread {name} in {method} reports counter at {counter}");
            }
            WriteLine($"Thread {name} exits {method}");
        }
        //T1 - Locked O1    -> To Lock O2
        //T2 - Locked O2    -> To Lock O1 
        private object _syncRoot = new object(); 

        public void RunMonitor()
        {
            var name = Thread.CurrentThread.Name;
            var method = nameof(RunMonitor);
            WriteLine($"Thread {name} enters {method}");
            while (counter < 40)
            {
                //Monitor.Enter(_syncRoot); //=> sets the synclock bit [1st bit of the object on the heap] to 1
                if (Monitor.TryEnter(_syncRoot, millisecondsTimeout: 1000))
                {
                    int temp = counter;
                    temp++;
                    Thread.Sleep(1000);
                    WriteLine($"Thread {name} in {method} reports counter at {counter}");
                    counter = temp;
                    Monitor.PulseAll(_syncRoot); //signals other waiting threads 
                    Monitor.Exit(_syncRoot); //=> reset/unset the synclock bit to 0 
                }
                else
                    break;
            }
            WriteLine($"Thread {name} exits {method}");
        }
        public void RunLock()
        {
            var name = Thread.CurrentThread.Name;
            var method = nameof(RunMonitor);
            WriteLine($"Thread {name} enters {method}");
            while (counter < 40)
            {
                lock (_syncRoot) //=> Monitor.Enter(..);
                {
                    int temp = counter;
                    temp++;
                    Thread.Sleep(1);
                    WriteLine($"Thread {name} in {method} reports counter at {counter}");
                    counter = temp;
                } //Monitor.Exit(...)
            }
            WriteLine($"Thread {name} exits {method}");
        }
        public void RunMutex()
        {
            var name = Thread.CurrentThread.Name;
            var method = nameof(RunMutex);
            WriteLine($"Thread {name} enters {method}");
            mtx.WaitOne();
            WriteLine($"Thread {name} Begins critical section in the current process.");
            Process p = Process.GetCurrentProcess(); 
            WriteLine($"Thread {name} reports Process Id: {p.Id}, Name: {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} completes critical section execution.");
            mtx.ReleaseMutex(); 
            WriteLine($"Thread {name} exits {method}");

        }
        public void RunSemaphore()
        {
            var name = Thread.CurrentThread.Name;
            var method = nameof(RunSemaphore);
            WriteLine($"Thread {name} enters {method}");
            sem.WaitOne();
            WriteLine($"Thread {name} Begins critical section in the current process.");
            Process p = Process.GetCurrentProcess();
            WriteLine($"Thread {name} reports Process Id: {p.Id}, Name: {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} completes critical section execution.");
            sem.Release(1);
            WriteLine($"Thread {name} exits {method}");
        }

        [System.Runtime.CompilerServices.MethodImpl( 
            System.Runtime.CompilerServices.MethodImplOptions.Synchronized
        )]
        public void RunWithAttribute()
        {
            var name = Thread.CurrentThread.Name;
            var method = nameof(RunWithAttribute);
            WriteLine($"Thread {name} enters {method}");
            WriteLine($"Thread {name} Begins critical section in the current process.");
            Process p = Process.GetCurrentProcess();
            WriteLine($"Thread {name} reports Process Id: {p.Id}, Name: {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} completes critical section execution.");
            WriteLine($"Thread {name} exits {method}");
        }
        static Mutex mtx;
        static Semaphore sem; 
        internal static void Test()
        {/*
            mtx = new Mutex(
                initiallyOwned: false,
                name: "SystemWideMutex",
                createdNew: out bool createdNew);
            WriteLine($"Mutex {mtx} was {(!createdNew ? "existing in the system." : "newly created.")}");

            sem = new Semaphore ( 
                initialCount: 3, 
                maximumCount: 3,
                name: "SystemWideSemaphore",
                createdNew: out createdNew);
            WriteLine($"Semaphore was {(!createdNew ? "existing in the system." : "newly created.")}");
*/
            Synchronization s = new Synchronization();
            Thread[] myThreads = new Thread[5];
            for (int i = 0; i < myThreads.Length; i++)
            {
                myThreads[i] = new Thread(s.RunWithAttribute);
                //myThreads[i] = new Thread(s.RunDefault);
                myThreads[i].Name = $"_{i+11}_";
                myThreads[i].Start();
            }
            //sem.Release(3);
            WriteLine("All threads started. Press a key to terminate...");
            ReadKey(); 
        }

    }
}
