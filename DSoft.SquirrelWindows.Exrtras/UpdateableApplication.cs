using Squirrel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DSoft.SquirrelWindows.Exrtras
{
    public abstract class UpdateableApplication : Application
    {
        private UpdateManager _updateManager;

        public abstract String UpdatePath { get; }

        public abstract String ApplicationName { get; }

        private Window _tempMainWindow;

        public virtual async Task ContinueStartupAsync()
        {

        }

        public virtual void ContinueStartup()
        {

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _tempMainWindow = new Window()
            {
                Visibility = Visibility.Hidden
            };


            using (var _updateManager = new UpdateManager(UpdatePath))
            {
                var updates = await _updateManager.CheckForUpdate();

                if (updates.NeedsUpdate())
                {
                    //show the update screen
                    var upDlg = new UpdateWindow(_updateManager, updates, ApplicationName);

                    var results = upDlg.ShowDialog();

                    if (results == true)
                    {
                        //restart app
                    }
                    else
                    {
                        var clstyoe = this.GetType();

                        var method = clstyoe.GetMethod("ContinueStartupAsync");
                        // var method2 = clstyoe.GetMethod("ContinueStartup"); 

                        if (method.DeclaringType != typeof(UpdateableApplication))
                        {
                            await ContinueStartupAsync();
                        }

                        var method2 = clstyoe.GetMethod("ContinueStartup");
                        if (method2.DeclaringType != typeof(UpdateableApplication))
                        {
                            ContinueStartup();
                        }

                    }
                }
            }

            _tempMainWindow.Close();
            _tempMainWindow = null;



        }
    }
}
