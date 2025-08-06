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
            Program program = new Program();
            program.DoWork(); 

            string email = "someone@example.com";
            if (email.IsValidEmail("@"))
            //if (Utitlity.IsValidEmail(email))
            {
                Console.WriteLine($"{email} is valid.");
            } else
                Console.WriteLine($"{email} is not valid.");

        }
    }
}
