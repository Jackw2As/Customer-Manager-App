using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Wpf.GenericControls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagerApp.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public App()
        {
           Task.Factory.StartNew(() => Load());
        }

        public MainWindowViewModel viewModel { get; set; }
        private async Task Load()
        {
            var dataService = await DataService.CreateDataServiceObjectAsync();
            viewModel = new(dataService);

            var mainWindow = new MainWindowView();
            mainWindow.DataContext = viewModel;


            Application.Current.MainWindow = mainWindow;

            InitializeComponent();
        }
    }
}
