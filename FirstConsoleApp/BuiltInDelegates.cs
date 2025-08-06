using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleApp
{
    internal class BuiltInDelegates
    {
        internal static void Test()
        {
            Action a1 = () => Console.WriteLine("Hello from Action delegate!");
            a1(); 
            Action<int, int> a2 = delegate(int x, int y)
            {
                Console.WriteLine($"Sum of {x} and {y} is {x + y}");
            };
            a2(10, 20); 

            Func<string> f1 = () => "Hello from Func delegate!";
            Console.WriteLine($"From Func<string> : {f1()}");
            Func<string, int, double, float, bool, string> f2 = (s, i, d, f, b) =>
            {
                return $"{s} {i} {d} {f} {b}";
            };
            Console.WriteLine($"{f2("Str", 10, 100.00, 89f, false)}");

            Predicate<string> p1 = (s) => s.Length > 5;
            Console.WriteLine($"Is 'HelloWorld' longer than 5 characters? {p1("HelloWorld")}");
            
            var numbers = new List<int> { 1, 7, 8, 2, 4, 9, 5, 6, 10, 3,12 };
            ListNumbers(numbers, (n)=> n > 5);
            ListNumbers(numbers, (n) => n % 5==0);

        }
        static void ListNumbers(List<int> arr, Predicate<int> criteria)
        {
            foreach(var item in arr)
            {
                if (criteria(item))
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
