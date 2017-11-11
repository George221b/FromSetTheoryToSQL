namespace SQLSimpleInterpreter.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class ColumnCollectionTests
    {
        [Test]
        public void InitializeEmptyColumnCollection()
        {
            //Arrange
            var cols = new ColumnCollection();

            //Assert
            CollectionAssert.AreEqual(new List<string>(), cols);
        }

        [Test]
        public void InitializeColumnCollectionWithElements()
        {
            //Arrange
            var cols = new ColumnCollection("FirstName", "LastName");

            //Act
            var expected = new List<string> { "FirstName", "LastName" };

            //Assert
            CollectionAssert.AreEqual(expected, cols);
        }

        [Test]
        public void AddElement()
        {
            //Arrange
            var cols = new ColumnCollection();

            //Act
            cols.Add("FirstName");
            var expected = new List<string> { "FirstName" };

            //Assert
            CollectionAssert.AreEqual(expected, cols);
        }

        [Test]
        public void RemoveExistingElement()
        {
            //Arrange
            var cols = new ColumnCollection("FirstName", "LastName", "Salary");

            //Act
            cols.Remove("LastName");
            var expected = new List<string> { "FirstName", "Salary" };

            //Assert
            CollectionAssert.AreEqual(expected, cols);
        }

        [Test]
        public void RemoveNonExistingElement()
        {
            //Arrange
            var cols = new ColumnCollection("FirstName", "LastName", "Salary");

            //Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                cols.Remove("Tarantino");
            });
        }

        [Test]
        public void ColumnCollectionCount()
        {
            //Arrange
            var cols = new ColumnCollection("FirstName", "LastName", "Salary");

            //Assert
            Assert.That(cols.Count() == 3, "ColumnCollection count is not working properly.");
        }
    }
}
