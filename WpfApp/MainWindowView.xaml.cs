using CustomerManagerApp.WpfApp.ViewModels;
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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            
            InitializeComponent();
        }



        public override void EndInit()
        {
            base.EndInit();
            
            while (ListView.DataContext as CustomerListViewModel == null && EditView.DataContext as CustomerEditViewModel == null)
            {
                continue;
                //Keep Looping until both data contexts are assigned to.
            }

            assignChildViewModelsToViewModel();
        }

        private void assignChildViewModelsToViewModel()
        {
            var listViewModel = ListView.DataContext as CustomerListViewModel;
            var editViewModel = EditView.DataContext as CustomerEditViewModel;

            ViewModel.CustomerList = listViewModel!;
            ViewModel.CustomerEdit = editViewModel!;
        }
    }
}
