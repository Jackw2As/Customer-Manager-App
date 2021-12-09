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
using CustomerManagerApp.Backend.Services.CustomerDataLoader;
using CustomerManagerApp.Backend.Services.DrinkRoleLoader;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
            this.InitializeComponent();
            ViewModel = new MainWindowViewModel(new CustomerDataJsonLoader(), new MockDrinkTypesLoader());
            this.Activated += MainWindow_Activated;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            ViewModel.SaveToStorage();
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if(ViewModel.Customers.Count == 0)
            {
                ViewModel.Load();
            }
        }
    }
}
