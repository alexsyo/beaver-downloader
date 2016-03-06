using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Beaver_Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DownloadItem> downloadItems = new ObservableCollection<DownloadItem> { };

        public MainWindow()
        {
            InitializeComponent();

            downloadList.ItemsSource = downloadItems;
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            string url = urlBox.Text;

            DownloadItem item = new DownloadItem(url);

            downloadItems.Add(item);
        }
    }
}
