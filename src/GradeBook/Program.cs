using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Book("Dan's GradeBook");
            // usually we handle the delegates right after creating an instance of the parent class
            book.GradeAdded += OnGradeAdded;

            // book.AddGrade(89.1);
            // book.AddGrade(90.5);
            // book.AddGrade(77.5);

            while (true)
            {
                System.Console.WriteLine("Enter a grade or 'q' to quit");
                var input = Console.ReadLine();
                if (input == "Q" || input == "q")
                {
                    break;
                }

                try
                {
                    var number = double.Parse(input);
                    book.AddGrade(number);
                }
                catch (ArgumentException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                finally
                {
                    System.Console.WriteLine("**");
                }
            }

            var stats = book.GetStatistics();
            // book.Name = "";

            System.Console.WriteLine(Book.CATEGORY);
            System.Console.WriteLine($"For the book named {book.Name}");
            System.Console.WriteLine($"The lowest grade is {stats.Low:N1}");
            System.Console.WriteLine($"The highest grade is {stats.High:N1}");
            System.Console.WriteLine($"The average grade is {stats.Average:N1}");
            System.Console.WriteLine($"The letter grade is {stats.Letter}");
        }
        static void OnGradeAdded(object sender, EventArgs e)
        {
            System.Console.WriteLine("A grade was added!");
        }
    }
}
