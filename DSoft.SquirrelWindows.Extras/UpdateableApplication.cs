using Squirrel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DSoft.SquirrelWindows.Extras
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

            //need to create a temp window so that the user can load their own window later
            _tempMainWindow = new Window()
            {
                Visibility = Visibility.Hidden
            };

            var shouldRestart = false;

            try
            {
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
                            shouldRestart = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
               
            }


            if (shouldRestart == true)
            {
                UpdateManager.RestartApp();
            }
            else
            {
                var clstyoe = this.GetType();

                //check to see if the ContinueStartupAsync has been overridden
                var method = clstyoe.GetMethod("ContinueStartupAsync");
                // var method2 = clstyoe.GetMethod("ContinueStartup"); 


                if (method.DeclaringType != typeof(UpdateableApplication))
                {
                    //call the overriden method
                    await ContinueStartupAsync();
                }

                //check to see if the ContinueStartup has been overridden
                var method2 = clstyoe.GetMethod("ContinueStartup");
                if (method2.DeclaringType != typeof(UpdateableApplication))
                {
                    //call the overriden method
                    ContinueStartup();
                }

                _tempMainWindow.Close();
                _tempMainWindow = null;
            }

        }
    }
}
