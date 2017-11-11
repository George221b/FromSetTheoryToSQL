namespace SQLSimpleInterpreter.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class SQLTableTests
    {
        private ColumnCollection cols1;
        private ColumnCollection cols2;

        [SetUp]
        public void Initialize()
        {
            this.cols1 = new ColumnCollection("FirstName", "LastName");
            this.cols2 = new ColumnCollection("DepartmentName");
        }

        [Test]
        public void InitializingSQLTableWithOnlyName()
        {
            //Arrange
            var t = new SQLTable("Employees");

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(t.Name == "Employees");
                Assert.IsNull(t.Alias);
                Assert.IsNull(t.Columns);
            });
        }

        [Test]
        public void InitializingSQLTable()
        {
            //Arrange
            var t = new SQLTable("Employees", "e", this.cols1);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(t.Name == "Employees");
                Assert.That(t.Alias == "e");
                Assert.That(t.Columns == this.cols1);
            });
        }

        [Test]
        public void GenerateQueryWithColumnCollection()
        {
            //Arrange
            var t = new SQLTable("Employees", this.cols1);

            //Act
            var expected = $@"SELECT e.[FirstName]
      ,e.[LastName]
  FROM [dbo].[Employees] AS e";
            var actual = t.GenerateQuery();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GenerateQueryWithNoColumns()
        {
            //Arrange
            var t = new SQLTable("Employees");

            //Act
            var expected = $@"SELECT *
  FROM [dbo].[Employees] AS e";
            var actual = t.GenerateQuery();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GenerateQueryWithColumnsAndTopResult()
        {
            //Arrange
            var t = new SQLTable("Employees", this.cols1);

            //Act
            var expected = $@"SELECT TOP (1000) e.[FirstName]
      ,e.[LastName]
  FROM [dbo].[Employees] AS e";
            var actual = t.GenerateQuery(1000);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
