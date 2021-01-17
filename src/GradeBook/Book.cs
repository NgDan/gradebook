using System.Collections.Generic;
using System;

namespace GradeBook
{
    // The convention in C# is to pass these 2 params, sender and args when creating a delegate that's going to be used to create an Event
    public delegate void GradedAddedDelegate(object sender, EventArgs args);
    public class Book
    {
        // This is a constructor. It doesn't have a return type, and must have the same name as the class it's in. It's going to be initialised before
        // any method call

        public Book(string name)
        {
            Name = name;
            grades = new List<double>();
        }
        public void AddGrade(double grade)
        {
            if (grade <= 100 && grade >= 0)
            {
                grades.Add(grade);

                // this checks that we have at least one method added to the GradeAdded delegat
                // if there aren't any methods attatched to it there's no point in dispatching
                // an event
                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid grade {nameof(grade)}");
            }
        }

        public void AddGrade(char letter)
        {
            switch (letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
        }

        // the event keyword adds some restrictions and some functionality to the delegate
        // this GradeAdded delagate can be used by member in the application to fire an event
        public event GradedAddedDelegate GradeAdded;

        public Statistics GetStatistics()
        {
            var result = new Statistics();
            result.High = double.MinValue;
            result.Low = double.MaxValue;
            result.Average = 0.0;

            foreach (var grade in grades)
            {
                result.High = Math.Max(result.High, grade);
                result.Low = Math.Min(result.Low, grade);
                result.Average += grade;
            }
            result.Average /= grades.Count;

            switch (result.Average)
            {
                case var d when d >= 90:
                    result.Letter = 'A';
                    break;
                case var d when d >= 80:
                    result.Letter = 'B';
                    break;
                case var d when d >= 70:
                    result.Letter = 'C';
                    break;
                case var d when d >= 60:
                    result.Letter = 'D';
                    break;
                default:
                    result.Letter = 'F';
                    break;
            }

            return result;
        }

        private List<double> grades;

        public string Name
        {
            get;
            set;
        }

        public const string CATEGORY = "Science";
        private string name;
    }
}