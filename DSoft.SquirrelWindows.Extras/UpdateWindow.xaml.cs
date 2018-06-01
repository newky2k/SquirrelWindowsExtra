using Squirrel;
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
using System.Windows.Shapes;

namespace DSoft.SquirrelWindows.Extras
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {

        private UpdateWindowViewModel _ViewModel;

        public UpdateWindowViewModel ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = value; DataContext = _ViewModel; }
        }



        public UpdateWindow(IUpdateManager updateManager, UpdateInfo updates, string applicationName)
        {
            InitializeComponent();

            ViewModel = new UpdateWindowViewModel(updateManager, updates, applicationName);
        }

        private void OnSkipButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void OnOKButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.UpdateManager.UpdateApp();

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
