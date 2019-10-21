using System.Windows;

namespace ExcelDBConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LocalPage localPage;
        DataBasePage dataBasePage;

        public MainWindow()
        {
            InitializeComponent();
            localPage = new LocalPage();
            dataBasePage = new DataBasePage();
            Main.Content = localPage;            
        }

        private void LocalBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = localPage;
        }

        private void DataBaseBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = dataBasePage;
        }        
    }
}
