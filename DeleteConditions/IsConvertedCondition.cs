namespace ExcelDBConverter
{
    public class IsConvertedCondition : IDeleteCondition
    {
        public bool Condition(DataFile data) => data.IsConverted;
    }
}