using System.Collections.Generic;
using System;

namespace GradeBook
{
    // The convention in C# is to pass these 2 params, sender and args when creating a delegate that's going to be used to create an Event
    public delegate void GradedAddedDelegate(object sender, EventArgs args);


    // Interfaces are far more common than abstract classes.
    // Interfaces are another way of achieving encapsulation and polymorphism but unlike abstract classes they
    // don't provide any implementation details. Abstract classes can provide implementation details if they 
    // want to
    public interface IBook
    {
        // we're not using the public keyword here because we want all objects that implement this
        // interface to have these members
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event GradedAddedDelegate GradeAdded;
    }
    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name)
        {
        }

        public virtual event GradedAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public virtual Statistics GetStatistics()
        {
            throw new NotImplementedException();
        }
    }


    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    // here, Book inherits from NamedObject so Book *is* a NamedObject
    public class InMemoryBook : Book
    {
        // This is a constructor. It doesn't have a return type, and must have the same name as the class it's in. It's going to be initialised before
        // any method call

        public InMemoryBook(string name) : base(name)
        {
            Name = name;
            grades = new List<double>();
        }

        // The override keyworkd means this method will override the method inherited.
        // You can only override abstract methods and "virtual methods" that use the
        // "virtual" keyword
        public override void AddGrade(double grade)
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
        public override event GradedAddedDelegate GradeAdded;

        public override Statistics GetStatistics()
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

        public const string CATEGORY = "Science";

    }
}