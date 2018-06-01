using DSoft.SquirrelWindows.Extras;
using System;
using System.Windows;

namespace SquirrelExtrasSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : UpdateableApplication
    {
        MainWindow _mainWindow;

        public override string UpdatePath
        {
            get
            {
                return @"C:\SquirrelReleases";
            }
        }

        public override String ApplicationName
        {
            get
            {
                return "Sample App";
            }
        }

        public override void ContinueStartup()
        {
            
            _mainWindow = new MainWindow();

            Application.Current.MainWindow = _mainWindow;

            _mainWindow.Show();
        }

    }
}
