using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleApp
{
    internal class TaskProgramming
    {
        internal async static Task TestAsync()
        {
            Console.WriteLine("Before calling GetData()");
           // var t1 = GetDataAsync("ABB");
            Console.WriteLine("Task created... Waiting for results");
            //var result = t1.Result; //Blocks the main thread 
            var result = await GetDataAsync("Abb"); //Method execution is paused.
            Console.WriteLine("Result is {0}", result);
        }
        internal async static Task<string> GetDataAsync(string input)
        {
            var output = string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                output = $"Input value was {input}";
            }
            await Task.Delay(2000); 
            return output;
        }


        internal static void TestTasks()
        {
            Task t1 = new Task(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Task t1 called.");
            });
            t1.Start();
            Task t2 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Task t2 called.");
            });
            Task t3 = Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                Console.WriteLine("Task.Run t3 started.");
            })
                .ContinueWith((t) =>
                {
                    Task.Delay(1000).Wait();
                    Console.WriteLine("Task.Continue With t4 started.");
                });
            Console.WriteLine("Tasks started. Waiting for completion.");

            Task.WaitAll(t1, t2, t3);

            //Console.WriteLine("Press a key to terminate."); 
            //Console.ReadKey();

            Stopwatch watch = Stopwatch.StartNew();
            DrawCircle();
            DrawRectangle();
            DrawSquare();
            DrawTriangle();
            watch.Stop();
            Console.WriteLine($"Sequential access takes {watch.ElapsedMilliseconds} ms.");
            Console.WriteLine("\nParallel Execution: ");
            watch = Stopwatch.StartNew();
            Parallel.Invoke(
                () => DrawCircle(),
                () => DrawRectangle(),
                () => DrawSquare(),
                () => DrawTriangle()
            );
            watch.Stop();
            Console.WriteLine($"Parallel access takes {watch.ElapsedMilliseconds} ms.");

            Console.WriteLine("Press a key to start key generations.....");
            Console.ReadKey(); 
            Console.WriteLine("\nBeginning Sequential generation");
            SequentialKeysGenerator();
            Console.WriteLine("\nBeginning Parallel generation");
            ParallelKeysGenerator(); 

        }

        static void SequentialKeysGenerator()
        {
            Console.WriteLine("Sequential Key generation started...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            for(int i = 0; i < MaxSize; i++)
            {
                var aes = Aes.Create();
                aes.GenerateIV(); 
                aes.GenerateKey();
                var key = aes.Key; 
                string s = Encoding.UTF8.GetString(key);
            }
            stopwatch.Stop();
            Console.WriteLine($"{nameof(SequentialKeysGenerator)} completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
        static void ParallelKeysGenerator()
        {
            Console.WriteLine("ParallelKeysGenerator started...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.For(1, MaxSize+1, i=>
            {
                var aes = Aes.Create();
                aes.GenerateIV();
                aes.GenerateKey();
                var key = aes.Key;
                string s = Encoding.UTF8.GetString(key);
            });
            stopwatch.Stop();
            Console.WriteLine($"{nameof(ParallelKeysGenerator)} completed in {stopwatch.ElapsedMilliseconds} ms.");
        }


        static int MaxSize = 1_000_000;
        static void DrawCircle()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int x = i * i / (i * i + 1);
            }
            Thread.Sleep(500);
            Console.WriteLine($"DrawCircle() called.");
        }
        static void DrawRectangle()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int x = i * i / (i * i + 1);
            }
            Thread.Sleep(900);
            Console.WriteLine($"DrawRectangle() called.");
        }
        static void DrawSquare()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int x = i * i / (i * i + 1);
            }
            Thread.Sleep(300);
            Console.WriteLine($"DrawSquare() called.");
        }
        static void DrawTriangle()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int x = i * i / (i * i + 1);
            }
            Thread.Sleep(100);
            Console.WriteLine($"DrawTriangle() called.");
        }
    }
}
