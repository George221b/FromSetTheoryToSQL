namespace SQLSimpleInterpreter.Contracts
{
    internal interface IGeneratable
    {
        string GenerateQuery();

        string GenerateQuery(int selectTopX);
    }
}
