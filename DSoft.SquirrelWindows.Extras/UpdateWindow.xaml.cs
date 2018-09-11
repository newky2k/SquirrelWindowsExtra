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

            ViewModel.OnStatusChanged += OnStatusChanged;
            ViewModel.OnProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(object sender, int e)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (e > 0)
                {
                    pgrProgress.IsIndeterminate = false;
                    pgrProgress.Value = e;
                }
                else
                {
                    pgrProgress.IsIndeterminate = true;
                }
            }));
                
        }

        private void OnStatusChanged(object sender, Enums.CurrentStatus e)
        {
           
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    switch (e)
                    {
                        case Enums.CurrentStatus.Complete:
                            {
                                this.DialogResult = true;
                            }
                            break;
                        case Enums.CurrentStatus.Skipped:
                            {
                                this.DialogResult = false;
                            }
                            break;
                    }

                }));

        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModel.CurrentStatus == Enums.CurrentStatus.Updating)
                e.Cancel = true;
        }
    }
}
