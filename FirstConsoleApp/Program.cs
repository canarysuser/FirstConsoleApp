namespace FirstConsoleApp
{
    public static class ViewModelExtensions
    {
        public static bool IsValidEmail(this string input, string criteria)
        {
            return input.Contains(criteria);
        }
        public static bool DoWork(this Program program)
        {
            // Do some work here
            return true;
        }
    }
    /*class EmailString : String
    {

    }*/
    public class Program
    {
        static void Main(string[] args)
        {
            /*Program program = new Program();
            program.DoWork(); 

            string email = "someone@example.com";
            if (email.IsValidEmail("@"))
            //if (Utitlity.IsValidEmail(email))
            {
                Console.WriteLine($"{email} is valid.");
            } else
                Console.WriteLine($"{email} is not valid.");

            foreach(var name in GetNames())
            {
                Console.WriteLine($"{name}");
            }
            foreach (var power in Power(2, 10))
            {
                Console.WriteLine($"Power: {power}");
            }*/

            //WorkingWithDelegates1.Test();
            //BuiltInDelegates.Test();
            //LINQOperators.Test();
            //WorkingWithReflection.Test();
            WorkingWithReflection.TestDynamicAssembly();
        }
        static IEnumerable<int> Power(int num, int multiplier)
        {
            //return Enumerable.Range(1, multiplier).ToList();

            int result = 1;
            for (int i = 0; i < multiplier; i++)
            {
                result *= num;
                if (result < 2000)
                    yield return result;
               /* else
                {
                    yield break; // Stop yielding if the result exceeds 200
                }*/
            }
            yield return 100; 
            yield return 200; // This will not be reached if the result exceeds 200
        }

        // Yielding functions cannot be anonymous or lambda functions.
        // They must be defined as a method with a return type of IEnumerable<T> or IEnumerator<T>, 
        //IAsyncEnumerable<T>, or IAsyncEnumerator<T>, IEnumerable,IEnumerator.
        // The yield return statement is used to return each element one at a time.
        // Cannot use ref or out parameters with yield return.

        static IEnumerable<string> GetNames()
        {
            yield return "Alice";           
            Console.WriteLine("Returned Alice");
            yield return "Bob";
            Console.WriteLine("Returned Bob");
            yield return "Charlie";
            Console.WriteLine("Returned Charlie");
        }

        
    }
}
