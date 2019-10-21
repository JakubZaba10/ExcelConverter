using System.Collections.Generic;

namespace ExcelDBConverter
{
    public class DataFileComparer : IEqualityComparer<DataFile>
    {
        public bool Equals(DataFile x, DataFile y)
        {
            return (x.FileName == y.FileName && x.FilePath == y.FilePath) ? true : false;
        }

        public int GetHashCode(DataFile obj)
        {
            int hCode = obj.FileName.GetHashCode() +  obj.FilePath.GetHashCode();
            return hCode;
        }
    }
}
