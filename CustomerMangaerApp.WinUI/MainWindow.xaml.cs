using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CustomerManagerApp.ViewModel;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.Repository.Customer;

namespace CustomerManagerApp.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
       public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            this.Activated += MainWindow_Activated;

            var dataContainer = new DataService<JsonCustomerRepository>();
            ViewModel = new MainWindowViewModel(dataContainer);
            
            this.InitializeComponent();
        }

        //When the Window is activated we load the Customer Data
        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args) => ViewModel.Load();
    }
}