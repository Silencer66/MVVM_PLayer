using MVVM_PLayer.Infrastructure.Commands;
using MVVM_PLayer.Model;
using MVVM_PLayer.Store;
using System.Windows;
using System.Windows.Input;
using WPFCustomMessageBox;

namespace MVVM_PLayer.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly MusicPlayer _musicPlayer;
        private readonly NavigationStore _navigationStore;
        /// <summary> Текущая ViewModel </summary>
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        #region Заголовок окна
        private string _title = "Artemix Player";
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => Set<string>(ref _title, value);
        }
        #endregion

        #region Команды

        #region Свернуть приложение в трей или закрыть
        public ICommand CloseApplicationButton { get; }
        private void OnCloseApplicationButton(object obj)
        {
            switch (CustomMessageBox.ShowOKCancel(
                "Закрыть приложение или свернуть в трей?",
                _title,
                "Закрыть",
                "Свернуть"))
            {
                case MessageBoxResult.OK:
                    _musicPlayer.DisposeWave();
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Cancel:
                    Application.Current.MainWindow.Hide();
                    break;
            }
        }
        private bool CanCloseApplicationButton(object obj) => true;
        #endregion

        #region Скрыть окно приложения
        public ICommand MinimizeApplicationButton { get; }
        private void OnMinimizeApplicationButton(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private bool CanMinimizeApplicationButton(object obj) => true;
        #endregion

        #region Раскрыть окно приложения
        public ICommand MaximizeApplicationButton { get; }
        private void OnMaximizeApplicationButton(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Normal) Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
        private bool CanMaximizeApplicationButton(object obj) => true;
        #endregion

        #region HeaderMouseDownCommand
        public ICommand HeaderMouseDownCommand { get; }
        private void OnHeaderMouseDownCommand(object obj)
        {
            Application.Current.MainWindow.DragMove();
        }
        private bool CanHeaderMouseDownCommand(object obj) => true;
        #endregion

        #region HeaderDoubleClick
        public ICommand HeaderDoubleClick { get; }
        private bool CanHeaderDoubleClick(object obj) => true;
        private void OnHeaderDoubleClick(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            else
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        #endregion

        #region TrayLeftMouseDownCommand
        public ICommand TrayLeftMouseDownCommand { get; }
        private void OnTrayLeftMouseDownCommand(object obj)
        {
            Application.Current.MainWindow.Show();
        }
        private bool CanTrayLeftMouseDownCommand(object obj)
        {
            if (Application.Current.MainWindow.Visibility == Visibility.Hidden) { return true; }
            else { return false; }
        }
        #endregion
        #endregion

        public MainViewModel(NavigationStore navigationStore, MusicPlayer musicPlayer)
        {
            #region Команды
            CloseApplicationButton = new LambdaCommand(OnCloseApplicationButton, CanCloseApplicationButton);
            MinimizeApplicationButton = new LambdaCommand(OnMinimizeApplicationButton, CanMinimizeApplicationButton);
            MaximizeApplicationButton = new LambdaCommand(OnMaximizeApplicationButton, CanMaximizeApplicationButton);
            HeaderMouseDownCommand = new LambdaCommand(OnHeaderMouseDownCommand, CanHeaderMouseDownCommand);
            HeaderDoubleClick = new LambdaCommand(OnHeaderDoubleClick, CanHeaderDoubleClick);
            TrayLeftMouseDownCommand = new LambdaCommand(OnTrayLeftMouseDownCommand, CanTrayLeftMouseDownCommand);
            #endregion

            _musicPlayer = musicPlayer;
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
