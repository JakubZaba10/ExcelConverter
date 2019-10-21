namespace ExcelDBConverter
{
    public class IsInactiveCondition : IDeleteCondition
    {
        public bool Condition(DataFile data) => !data.Active;
    }
}