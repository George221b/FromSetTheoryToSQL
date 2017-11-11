namespace MyApp
{
    using SQLSimpleInterpreter;
    using System;

    public class StartUp
    {
        private const string JoinBeginning = "----{0}----";

        public static void Main()
        {
            ColumnCollection columns1 = new ColumnCollection("FirstName", "Salary", "JobTitle");
            SQLTable table1 = new SQLTable("Employees", columns1);

            //GenerateSelectQueryOnSingleTable(table1);

            ColumnCollection columns2 = new ColumnCollection("Name");
            SQLTable table2 = new SQLTable("Departments", columns2);

            
            SQLJoin join = new SQLJoin(table1, table2, "DepartmentId", "DepartmentId");

            GenerateJoinQueries(join);
        }

        private static void GenerateSelectQueryOnSingleTable(SQLTable table1)
        {
            Console.WriteLine(table1.GenerateQuery());
            Console.WriteLine();
            Console.WriteLine(table1.GenerateQuery(100));
        }

        private static void GenerateJoinQueries(SQLJoin join)
        {
            Console.WriteLine(string.Format(JoinBeginning, "Inner Join"));
            Console.WriteLine(join.InnerJoin());
            Console.WriteLine();

            Console.WriteLine(string.Format(JoinBeginning, "Left Join"));
            Console.WriteLine(join.LeftJoin());
            Console.WriteLine();

            Console.WriteLine(string.Format(JoinBeginning, "Right Join"));
            Console.WriteLine(join.RightJoin());
            Console.WriteLine();

            Console.WriteLine(string.Format(JoinBeginning, "Left Set Difference"));
            Console.WriteLine(join.LeftSetDifference());
            Console.WriteLine();

            Console.WriteLine(string.Format(JoinBeginning, "Full Outer Join"));
            Console.WriteLine(join.FullOuterJoin());
            Console.WriteLine();

            Console.WriteLine(string.Format(JoinBeginning, "Symmetric Difference"));
            Console.WriteLine(join.SymmetricDifference());
            Console.WriteLine();
        }
    }
}
