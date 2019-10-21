using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelDBConverter
{
    public static class DBExtractorDict
    {
        private static Dictionary<Type, DBExtractor> dbExtractorDictionary = new Dictionary<Type, DBExtractor>()
        {
            {Type.Oracle, new DBExtractorOracle()},
            {Type.MS, new DBExtractorMS() }
        };

        public static List<Type> GetTypeList()
        {
            var typeList = Enum.GetValues(typeof(Type)).Cast<Type>().ToList();
            return typeList;
        }

        public static DBExtractor ReturnDialogType(string input)
        {
            Enum.TryParse(input, out Type type);
            var dialogType = dbExtractorDictionary[type];
            return dialogType;
        }

        public enum Type
        {
            Oracle,
            MS
        }
    }
}
