namespace SQLSimpleInterpreter.Contracts
{
    internal interface IJoinGeneratable
    {
        string InnerJoin(int selectTopX = 0);
        string Intersection(int selectTopX = 0);

        string LeftJoin(int selectTopX = 0);
        string RightJoin(int selectTopX = 0);

        string FullOuterJoin(int selectTopX = 0);

        string LeftSetDifference(int selectTopX = 0);
        string RightSetDifference(int selectTopX = 0);

        string SymmetricDifference(int selectTopX = 0);
    }
}
