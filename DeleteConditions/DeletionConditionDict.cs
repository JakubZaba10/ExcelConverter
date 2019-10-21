using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelDBConverter
{
    public static class DeletionConditionDict
    {
        private static Dictionary<Type, IDeleteCondition> DeletionConditionDictionary = new Dictionary<Type, IDeleteCondition>()
        {
            {Type.Converted, new IsConvertedCondition() },
            {Type.Inactive, new IsInactiveCondition() }
        };

        public static List<Type> GetTypeList()
        {
            var typeList = Enum.GetValues(typeof(Type)).Cast<Type>().ToList(); ;
            return typeList;
        }
        public static IDeleteCondition ReturnConditionType(string input)
        {
            Enum.TryParse(input, out Type type);
            var conditionType = DeletionConditionDictionary[type];
            return conditionType;
        }
        public enum Type
        {
            Converted,
            Inactive
        }
    }
}
