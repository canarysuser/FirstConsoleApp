using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static System.Console;

namespace FirstConsoleApp
{
    internal class LINQOperators
    {
        static List<string> cities = new List<string>()
        {
            "Bengaluru", "Chennai", "Hyderabad", "Panaji", "Mumbai", "Thiruvananthapuram", "Jaipur",
            "Lucknow", "New Delhi", "Shimla", "Leh", "Patna", "Raipur", "Kolkata", "Kohima",
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
            //ProjectionOperator(); 
            //RestrictionQueries();
            //SortingQueries();
            //AggregationQueries();
            //GroupingQueries();
            // PartitionQueries();
            //ElementOperators();
            //LinqToDbEntities();
            CustomExpressionTree();
        }
        static void CustomExpressionTree()
        {
            IQueryable<string> queryableData = cities.AsQueryable();
            //cities.Where(c=>c.ToLower()=="shimla" || c.Length>10);
            ParameterExpression pe = Expression.Parameter(typeof(string), "c");
            Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            Write("Filter by City: ");
            string city = Console.ReadLine(); 
            Expression right = Expression.Constant(city);
            Expression e1 = Expression.Equal(left, right);

            left = Expression.Property(pe, typeof(string).GetProperty("Length"));
            Write("Enter desired length: ");
            int.TryParse(Console.ReadLine(), out int length);
            right = Expression.Constant(length, typeof(int));
            Expression e2 = Expression.GreaterThan(left, right);
            Expression predicateBody = Expression.OrElse(e1, e2);
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable), "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));

            IQueryable<string> results = queryableData.Provider.CreateQuery<string>(whereCallExpression);
            foreach (var result in results)
            {
                WriteLine(result);
            }
        }
        static void LinqToDbEntities()
        {
            ProductDbContext db = new ProductDbContext();
            var products = from product in db.Products
                           select product;
            var projection = from c in db.Products
                             select new
                             {
                                 c.CategoryId,
                                 c.ProductId,
                                 c.ProductName
                             };
            var stockValue = db.Products.Sum(c => c.UnitPrice * c.UnitsInStock);
            var categoryWise = from c in db.Products
                               where c.CategoryId == 1 || c.CategoryId == 2
                               select c;
            var complex = db.Products
                .OrderBy(c => c.UnitPrice)
                .Where(c => c.ProductName.Contains("ai"))
                .Select(c => c)
                .Skip(5)
                .Take(5);
            /*db.Products.FromSqlRaw("exec sp_procedureName {0}, {1}", 10, 20);
            db.Database.ExecuteSql("....");*/

            products.ToList().ForEach(x => WriteLine(x));
            WriteLine(line);
            
            WriteLine($"Stock Value: {stockValue}");
            WriteLine(line);
            
            categoryWise.ToList().ForEach(x => WriteLine(x)); WriteLine(line);
            
            complex.ToList().ForEach (x => WriteLine(x));
            WriteLine(line);

           


        }
        static void ElementOperators()
        {
            //First, Last, ElementAt(x).... 
            var first = cities.First();
            var last = cities.Last();

            var firstCondition = cities.First(c => c.Length == 18);
            var lastCondition = cities.Last(c => c.Length == 3);
            //can throw Exceptions when there are no matches. 
            //to avoid exceptions, use FirstOrDefault 
            var firstCondition2 = cities.FirstOrDefault(c => c.Length == 18);
            var lastCondition2 = cities.LastOrDefault(c => c.Length == 4);

            WriteLine($"First: {first}, Last: {last}");
            WriteLine($"FirstCondition: {firstCondition}, LastCondition: {lastCondition}");
            WriteLine($"FirstCondition2: {firstCondition2}, LastCondition2: {lastCondition2}");


        }


        static void PartitionQueries()
        {
            //Take(5) - takes the first 5 values and skips the rest
            //Skip(5) -> skips the first 5 values and takes the rest 
            var take5 = cities.Take(5);
            var skip5 = cities.Skip(5);
            PrintList(take5, $"{counter++} Take 5");
            PrintList(skip5, $"{counter++} Skip 5");
            var takeSkip = cities.Skip(5).Take(15).Skip(4).Take(2);
            PrintList(takeSkip, $"{counter++} Take Skip Combination");

            var takeWhile = cities.TakeWhile(c => c.Contains("ga") || c.Contains("na"));
            var skipWhile = cities.SkipWhile(c => c.Length < 15);
            PrintList(takeWhile, $"{counter++} Take While ");
            PrintList(skipWhile, $"{counter++} Skip While ");
            var takeWhere = cities.Where(c => c.Contains("na")).Take(4);
            PrintList(takeWhere, $"{counter++} Take Where ");
        }
        static void GroupingQueries()
        {
            var q1 = from c in cities
                     orderby c
                     group c by c[0] into g
                     select g;
            foreach (var group in q1)
            {
                PrintList(group.ToList(), $"{counter++} Key: {group.Key}");
            }
            var q2 = cities
                .OrderBy(c => c.Length)
                .GroupBy(g => g.Length)
                .Select(c => c);
            foreach (var group in q2)
            {
                PrintList(group.ToList(), $"{counter++} Length Key: {group.Key}");
            }
        }
        static void AggregationQueries()
        {
            var count = cities.Count();
            var sum = cities.Sum(c => c.Length);
            var min = cities.Min(c => c.Length);
            var max = cities.Max(c => c.Length);
            var avg = cities.Average(c => c.Length);
            WriteLine($"Count:{count}, Sum:{sum}, Min:{min}, Max:{max}, Avg: {avg}");

        }
        static void SortingQueries()
        {
            //OrderBy, ThenBy, OrderByDescending, ThenByDescending
            var q1 = from c in cities
                     where c.Length > 8
                     orderby c[0] descending, c[1] ascending
                     select c;
            var q2 = cities
                .Where(c => c.Length > 8)
                .OrderBy(c => c[0])
                .ThenByDescending(c => c[1])
                .Select(c => c);
            PrintList(q1, $"{counter++} Ordered by first descending and second ascending letters");
            PrintList(q2, $"{counter++} Ordered by first ascending and second descending letters");
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
