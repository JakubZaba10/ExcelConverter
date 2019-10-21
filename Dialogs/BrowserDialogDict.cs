using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelDBConverter
{
    public static class BrowserDialogDict
    {
        private static Dictionary<Type, BrowserDialog> browserDialogDictionary = new Dictionary<Type, BrowserDialog>()
        {
            {Type.Folder, new FolderDialogBrowser()},
            {Type.File, new FileDialogBrowser() }
        };

        public static List<Type> GetTypeList()
        {
            var typeList = Enum.GetValues(typeof(Type)).Cast<Type>().ToList();
            return typeList;
        }
        public static BrowserDialog ReturnDialogType(string input)
        {
            Enum.TryParse(input, out Type type);
            var dialogType = browserDialogDictionary[type];
            return dialogType;
        }

        public enum Type
        {
            Folder,
            File
        }
    }
}
