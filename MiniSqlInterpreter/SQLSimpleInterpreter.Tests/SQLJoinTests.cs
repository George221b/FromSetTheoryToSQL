namespace SQLSimpleInterpreter.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class SQLJoinTests
    {
        private const string JoinKeyTable1 = "DepartmentId";
        private const string JoinKeyTable2 = "DepartmentId";

        private ColumnCollection columns1;
        private ColumnCollection columns2;
        private SQLTable table1;
        private SQLTable table2;

        [SetUp]
        public void Initialize()
        {
            this.columns1 = new ColumnCollection("FirstName", "Salary", "JobTitle");
            this.table1 = new SQLTable("Employees", columns1);

            this.columns2 = new ColumnCollection("Name");
            this.table2 = new SQLTable("Departments", columns2);
        }

        [Test]
        public void InitializeSQLJoin()
        {
            //Arrange
            var j = new SQLJoin(this.table1,
                this.table2,
                JoinKeyTable1,
                JoinKeyTable2);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(this.table1, j.TableOne);
                Assert.AreEqual(this.table2, j.TableTwo);
                Assert.AreEqual(JoinKeyTable1, j.TableOneJoinColumn);
                Assert.AreEqual(JoinKeyTable2, j.TableTwoJoinColumn);
            });
        }

        [Test]
        public void InnerJoinResult()
        {
            //Arrange
            var j = new SQLJoin(this.table1,
                this.table2,
                JoinKeyTable1,
                JoinKeyTable2);

            //Act
            var expected = @"SELECT e.[FirstName]
      ,e.[Salary]
      ,e.[JobTitle]
      ,d.[Name]
  FROM [dbo].[Employees] AS e
 INNER JOIN [Departments] AS d
    ON e.[DepartmentId] = d.[DepartmentId]";
            var actual = j.InnerJoin();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
