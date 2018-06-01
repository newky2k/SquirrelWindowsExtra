using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.SquirrelWindows.Extras
{
    public class UpdateWindowViewModel : INotifyPropertyChanged
    {

        private IUpdateManager _updateManager;
        private UpdateInfo _updates;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _CurrentVersionNo;
        private string _ApplicationName;
        private string _NewVersion;

        public string CurrentVersionNo
        {
            get { return _CurrentVersionNo; }
            set { _CurrentVersionNo = value; NotifyPropertyChanged("CurrentVersionNo"); }
        }

        public string NewVersionNo
        {
            get { return _NewVersion; }
            set { _NewVersion = value; NotifyPropertyChanged("NewVersionNo"); }
        }

        public string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; NotifyPropertyChanged("ApplicationName"); }
        }

        public string UpdateMessage
        {
            get
            {
                return $"A new version of {ApplicationName} is available. Do you want to download it now?";
            }
        }

        public IUpdateManager UpdateManager
        {
            get
            {
                return _updateManager;
            }
        }

        public UpdateWindowViewModel(IUpdateManager updateManager, UpdateInfo updates, string applicationName)
        {
            _ApplicationName = applicationName;
            _updateManager = updateManager;
            _updates = updates;

            CurrentVersionNo = _updates.CurrentlyInstalledVersion?.Version.ToString() ?? "1.0.0";

            NewVersionNo = _updates.FutureReleaseEntry.Version.ToString();
        }

    }
}
