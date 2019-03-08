using DSoft.SquirrelWindows.Extras.Enums;
using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DSoft.SquirrelWindows.Extras
{
    public class UpdateWindowViewModel : INotifyPropertyChanged
    {
        #region Fields
        private IUpdateManager _updateManager;
        private UpdateInfo _updates;
        private string _CurrentVersionNo;
        private string _ApplicationName;
        private string _NewVersion;
        private long _updateFileSize;
        #endregion

        #region Events
        public event EventHandler<CurrentStatus> OnStatusChanged = delegate { };
        public event EventHandler<int> OnProgressChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
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

        private CurrentStatus _currentStatus;

        public CurrentStatus CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                NotifyPropertyChanged(nameof(CurrentStatus));
                NotifyPropertyChanged(nameof(InteractionEnabled));
                NotifyPropertyChanged(nameof(ProgressVisibility));
                NotifyPropertyChanged(nameof(OKCommand));
                NotifyPropertyChanged(nameof(SkipCommand));
                OnStatusChanged?.Invoke(this, _currentStatus);
            }
        }

        public bool InteractionEnabled
        {
            get
            {
                return (_currentStatus != CurrentStatus.Updating);

            }
        }

        public ICommand SkipCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    CurrentStatus = CurrentStatus.Skipped;

                }, (obj) =>
                {
                    return (CurrentStatus != CurrentStatus.Updating);
                });
                
            }
        }

        public ICommand OKCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Task.Run(async () =>
                    {
                        await UpdateApplication();
                    });

                   

                }, (obj) =>
                {
                    return (CurrentStatus != CurrentStatus.Updating);
                });

            }
        }

        public Visibility ProgressVisibility
        {
            get
            {
                return (CurrentStatus == CurrentStatus.Updating) ? Visibility.Visible : Visibility.Hidden;
            }
        }



        public long UpdateFileSize
        {
            get { return _updateFileSize; }
            set { _updateFileSize = value; NotifyPropertyChanged(nameof(UpdateFileSize)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateWindowViewModel"/> class.
        /// </summary>
        /// <param name="updateManager">The update manager.</param>
        /// <param name="updates">The updates.</param>
        /// <param name="applicationName">Name of the application.</param>
        public UpdateWindowViewModel(IUpdateManager updateManager, UpdateInfo updates, string applicationName)
        {
            _ApplicationName = applicationName;
            _updateManager = updateManager;
            _updates = updates;

            CurrentVersionNo = _updates.CurrentlyInstalledVersion?.Version.ToString() ?? "1.0.0";

            NewVersionNo = _updates.FutureReleaseEntry.Version.ToString();

            UpdateFileSize = _updates.FutureReleaseEntry.Filesize;
        }
        #endregion

        #region Methods

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task UpdateApplication()
        {
            try
            {
                CurrentStatus = CurrentStatus.Updating;

                await UpdateManager.UpdateApp((value) =>
                {
                    OnProgressChanged?.Invoke(this, value);
                });

                CurrentStatus = CurrentStatus.Complete;

            }
            catch (Exception ex)
            {
                CurrentStatus = CurrentStatus.Failed;

                MessageBox.Show(ex.Message);
            }

        }
        #endregion
    }

    internal class RelayCommand : ICommand
    {
        private Action execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute();
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = null;
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
