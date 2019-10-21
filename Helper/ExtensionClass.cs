using System.Collections.Generic;
using System.Linq;

namespace ExcelDBConverter
{
    public static class ExtensionClass
    {
        public static T[,] List2DArray<T>(this List<T[]> source)
        {
            int max = source[0].Count();
            var result = new T[source.Count, max];

            for (int i = 0; i < source.Count; i++)
            {
                for (int j = 0; j < source[i].Count(); j++)
                {
                    result[i, j] = source[i][j];
                }
            }
            return result;
        }
    }
}
