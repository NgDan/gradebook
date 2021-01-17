using System;
using Xunit;

namespace GradeBook.Tests
{


    // Here we define the signature of the delegate
    public delegate string WriteLogDelegate(string logMessage);
    public class TypeTests
    {
        int count = 0;
        [Fact]
        public void WriteLogDelegateCanPointToMethod()
        {
            // here we create a new var for our delagate using the type we've defined earlier
            // then, we create a new instance of WriteLogDelegate, passing the method we're interested in

            // this is the longer notation
            // WriteLogDelegate log;
            // log = new WriteLogDelegate(ReturnMessage);
            // but we can shorten it like this:

            WriteLogDelegate log = ReturnMessage;

            // the line above initialised log as WriteLogDelegate type and it
            // assigned log the ReturnMessage method.
            // The lines below will add ReturnMessage again to the delagate and
            // will also add IncrementCount.
            // then, log will call all the methods added to it, even if they are added tiwce
            // like ReturnMessage. To test this we created a global count variable to verify
            // that ReturnMessage has called it twice and IncrementCount once, even though 
            // we've only called log("Hello!") once
            // This is called a multi-cast delagate.
            log += ReturnMessage;
            log += IncrementCount;

            var result = log("Hello!");

            // Assert.Equal("Hello!", result);
            Assert.Equal(3, count);
        }

        // this method is going to be passed to the delegate.
        // it's signature must match the signature of the delegate.
        // that means, in this case it needs to take an argument of 
        // type string and return an argument of type string
        string IncrementCount(string message)
        {
            count++;
            return message.ToLower();
        }

        // this method is going to be passed to the delegate.
        // it's signature must match the signature of the delegate.
        // that means, in this case it needs to take an argument of 
        // type string and return an argument of type string
        string ReturnMessage(string message)
        {
            count++;
            return message;
        }
        [Fact]
        public void StringsBehaveLikeValueTypes()
        {
            string name = "Dan";
            var upper = MakeUpperCase(name);

            // Although the string constructor is a class, it behaves like a value type.
            // This is why here, Dan is not uppercase, the value has been passed by value
            // so MakeUpperCase made a copy of it and modified that, instead of modifying the
            // variable the reference points to.
            Assert.Equal(name, "Dan");
            Assert.Equal(upper, "DAN");
        }

        private string MakeUpperCase(string param)
        {
            // strings are immutable so param.ToUpper is not going to actually modify the param value,
            // it's just going to return a new value and you have to store that in a new variable.
            var toUpperParam = param.ToUpper();
            return toUpperParam;
        }

        [Fact]
        public void ValueTypesAreAlsoPassedByValue()
        {

            // this is a value type not a reference type
            // if this was an instance of Book, it would be a reference type
            var x = GetInt();

            // Here, x is passed by value and it's also a value type so SetInt will create a new place in memory for x and assign it the new number
            // but it's not going to affect the "original" value.
            // If we used the "ref" keyword in front of the "z" parameter in the SetInt method, we would've passed this number by reference and the SetInt method
            // would've changed the original value as well.
            SetInt(x);

            Assert.Equal(3, x);
        }

        private void SetInt(int z)
        {
            z = 4;
        }

        private int GetInt()
        {
            return 3;
        }

        [Fact]
        public void CSharpCanPassByRef()
        {

            // this is a reference type not a value type
            var book1 = GetBook("Book 1");

            GetBookSetName(ref book1, "New Name");

            Assert.Equal(book1.Name, "New Name");
        }

        private void GetBookSetName(ref Book book, string name)
        {
            book = new Book(name);
            // book.Name = name;
        }

        [Fact]
        public void CSharpIsPassByValue()
        {

            // this is a reference type not a value type
            var book1 = GetBook("Book 1");

            // the reference book1 is going to be passed as a value. Here, the value is a referenc.
            GetBookSetName(book1, "New Name");

            Assert.Equal(book1.Name, "Book 1");
        }

        private void GetBookSetName(Book book, string name)
        {
            // Here, book is a new value that is created, because *by default C# always passes parameters by value*.
            // This value is assigned a reference because book1 is a reference type, but when we do book = new Book(name), we're overriding
            // the value of that variable with a new Book reference so the reference passed as a parameter is lost.
            book = new Book(name);
            book.Name = name;
        }

        [Fact]
        public void CanSetNameFromReference()
        {

            var book1 = GetBook("Book 1");

            SetName(book1, "New Name");

            Assert.Equal(book1.Name, "New Name");
        }

        private void SetName(Book book, string name)
        {
            // Here, book is also a new value which holds the reference to book1, because we're passing a reference type.
            // We can access that reference and modify it's properties and call it's methods but if we override book, it's not going
            // to affect the original book1 passed as a param, because C# passes params by value.
            book.Name = name;
        }

        [Fact]
        public void GetBookReturnsDifferentObjects()
        {

            var book1 = GetBook("Book 1");
            var book2 = GetBook("Book 2");

            Assert.Equal("Book 1", book1.Name);
            Assert.Equal("Book 2", book2.Name);
            // check that these 2 objects reference 2 different objects; 
            Assert.NotSame(book1, book2);

        }

        [Fact]
        public void TwoVarsCanReferenceSameObject()
        {

            var book1 = GetBook("Book 1");
            var book2 = book1;

            // the .Same method checks if these 2 objects reference the same object. Basically a reference equality check
            Assert.Same(book1, book2);
            // Another way to check for reference equality:
            Assert.True(Object.ReferenceEquals(book1, book2));

        }

        Book GetBook(string name)
        {
            return new Book(name);
        }
    }
}
