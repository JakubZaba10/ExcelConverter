using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelDBConverter
{
    /// <summary>
    /// Interaction logic for DataBasePage.xaml
    /// </summary>
    public partial class DataBasePage : Page
    {
        DBExtractor dBExtractor;
        public DataBasePage()
        {
            InitializeComponent();
            DBTypeCbx.ItemsSource = DBExtractorDict.GetTypeList();
            DBTypeCbx.SelectionChanged += DBTypeCbx_SelectionChanged;
            SetDbType();
        }

        private void SetDbType()
        {
            var dialogType = DBExtractorDict.ReturnDialogType(DBTypeCbx.SelectedValue.ToString());
            dBExtractor = dialogType;
        }

        private void ConvertBtn_Click(object sender, RoutedEventArgs e)
        {
            dBExtractor.Connect(ConnectionStringText.Text, UserText.Text, PasswordText.Password);
        }

        private void DBTypeCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDbType();
        }
    }
}
