namespace ExcelDBConverter
{
    public interface IDeleteCondition
    {
        bool Condition(DataFile data);
    }
}