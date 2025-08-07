using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleApp
{
    //Step 1: Define a delegate
    public delegate int ArithmeticDelegate(int x, int y);


    public class WorkingWithDelegates1
    {
        public WorkingWithDelegates1()
        {
            Console.WriteLine($"{nameof(WorkingWithDelegates1)}.ctor() invoked.");
        }
        //2: Create a method that matches the delegate signature
        public static int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;
        public static int Multiply(int x, int y) => x * y;
        public int Divide(int x, int y) => (y==0)? 0 : x / y;


        internal static void Test()
        {
            //Step 2: Instantiate the delegate with a method
            ArithmeticDelegate ad = new ArithmeticDelegate(Add);
            //Step 3: Invoke the delegate
            int result = ad(10, 20);
            Console.WriteLine($"Add: {result}");
            result  = ad.Invoke(5,4);
            Console.WriteLine($"Add: {result}");
            //Step 4: Use the delegate with different methods
            WorkingWithDelegates1 wd1 = new WorkingWithDelegates1(); 
            ad += new ArithmeticDelegate(wd1.Subtract);
            ad+= new ArithmeticDelegate(Multiply);
            ad += wd1.Divide;
            //Anonymous Method
            ad += delegate (int x, int y)
            {
                Console.WriteLine($"Result from modulus: {result}");
                return x % y;
            };
            //Multiply(10, 20); 
            //Lambda expression
            ad += (x, y) => 
            {
                Console.WriteLine($"Result from lambda: {x} + {y} = {x + y}");
                return x + y;
            };  //Statement lambda 
            ad += (x, y) => x - y; //Expression lambda 
            // Passing parameters 
            // 1. Zero parameters:          => () => return
            // 2. One parameter:            => x =>x | (x) => x 
            // 3. Multiple parameters:      => (x, y) => x + y



            //step 5: Invoke the delegate with multiple methods
            result = ad(100, 50);
            Console.WriteLine($"Result of multiple operations: {result}");

            InvokeManually(ad);



        }
        static void InvokeManually(ArithmeticDelegate ad)
        {
            object result = 10; 
            foreach(Delegate del in ad.GetInvocationList())
            {
                result = del.DynamicInvoke(Convert.ToInt32(result), 10)!; 
                Console.WriteLine($"Result of {del.Method.Name}: {result}");
            }
        }
    }
}
