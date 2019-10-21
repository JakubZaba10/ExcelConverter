using System.Windows;
using System.Windows.Controls;

namespace ExcelDBConverter
{
    /// <summary>
    /// Interaction logic for LocalPage.xaml
    /// </summary>
    public partial class LocalPage : Page
    {

        UIManager uiManager;
        DataManager dataManager;

        public LocalPage()
        {
            InitializeComponent();
            uiManager = new UIManager(DeleteTypeCbx, DialogTypeCbx, List, SearchDialogBtn, DeleteBtn, ConvertBtn);
            dataManager = new DataManager();
            uiManager.SetProgressItemsSource(dataManager.ProgressUIComponent, FileName, ConvertProgressBar);
            List.ItemsSource = dataManager.DataFiles;
            DeleteTypeCbx.SelectionChanged += DeletionTypeCbx_SelectionChanged;
            DialogTypeCbx.SelectionChanged += DialogTypeCbx_SelectionChanged;
        }

        private void DialogTypeCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uiManager.SetBrowserDialog();
        }

        private void SearchDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            uiManager.CreateDialogTask(dataManager, dataManager.DataFiles);
        }

        private void AllChecked_Click(object sender, RoutedEventArgs e)
        {
            var allCheckBox = (CheckBox)sender;
            var isActive = (bool)allCheckBox.IsChecked;
            dataManager.SetItemsActive(isActive);
        }

        private void DeletionTypeCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataManager.SetDeletionType();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            dataManager.DeleteDataFilesBasedOnCondition();
        }

        private void ConvertBtn_Click(object sender, RoutedEventArgs e)
        {
            dataManager.ConvertData();
        }
    }
}
