using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console; 

namespace FirstConsoleApp
{
    internal class LINQOperators
    {
        static List<string> cities = new List<string>()
        {
            "Bengaluru", "Chennai", "Hyderabad", "Panaji", "Mumbai", "Thiruvananthapuram", "Jaipur",
            "Lucknow", "New Delhi", "Shimla", "leh", "Patna", "Raipur", "Kolkata", "Kohima",
            "Agartala", "Dispur", "Gandhinagar","Chandigarh", "Dehra dun", "Srinagar", "Bhopal",
            "Amaravati", "Ranchi", "Aizwal", "Imphal", "Shillong"
        };

        static string line = "".PadLeft(45, '=');
        static int counter = 1; 
        static void PrintList(IEnumerable<string> list, string header)
        {
            WriteLine(line);
            WriteLine($"                  {header} ");
            WriteLine(line);
            foreach (var item in list) Write($"{item},\t");
            WriteLine($"\n{line}");
        }
        internal static void Test()
        {
            // BasicQueries(); 
            ProjectionOperator(); 
            RestrictionQueries();
        }
        static void RestrictionQueries()
        {
            var q1 = from city in cities 
                     where city.Length > 8 
                     select city;
            PrintList(q1, $"{counter++} Where Length > 8");
            var q2 = cities
                .Where(c => c.Contains("gar") && c.Length > 7)
                .Select(c => c);
            PrintList(q2, $"{counter++} Where City contains 'gar' && Length > 7");
        }
        static void ProjectionOperator()
        {
            Action<IEnumerable<ProjectionType>, string> printList = (list, header) =>
            {
                WriteLine(line);
                WriteLine($"                  {header} ");
                WriteLine(line);
                foreach (var item in list)
                    WriteLine($"{item.FirstLetter}, {item.StringLength:00}, {item.Name}");
                WriteLine($"\n{line}");
            };

            var q1 = from c in cities
                     select new ProjectionType
                     {
                         Name = c,
                         StringLength = c.Length,
                         FirstLetter = c[0]
                     };
            printList(q1, $"{counter++} Projection Query Syntax");
            var q2 = cities.Select(c => new ProjectionType
            {
                Name = c,
                StringLength = c.Length,
                FirstLetter = c[0]
            }); 
            printList(q2, $"{counter++} Projection Method Syntax");

        }
        class ProjectionType
        {
            public string Name { get; set; }
            public int StringLength { get; set; }
            public char FirstLetter { get; set; }
        }
        static void BasicQueries()
        {
            //Query Syntax
            var q1 = from city in cities select city;
            //WriteLine(q1);
            PrintList(q1, $"{counter++} Basic Query Query Syntax");
            //Method Syntax 
            var q2 = cities.Select(city => city);
           // WriteLine(q2);
            PrintList(q2, $"{counter++} Basic Query Method Syntax");

        }

    }
}
