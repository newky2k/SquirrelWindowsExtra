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

namespace DSoft.SquirrelWindows.Exrtras
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private IUpdateManager _updateManager;
        private UpdateInfo _updates;

        public UpdateWindow(IUpdateManager updateManager, UpdateInfo updates, string applicationName)
        {
            InitializeComponent();
        }

        private void OnSkipButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OnOKButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
