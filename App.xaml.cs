using MVVM_PLayer.Model;
using MVVM_PLayer.Store;
using MVVM_PLayer.ViewModel;
using System.Windows;

namespace MVVM_PLayer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly MusicPlayer _musicPlayer;
        public App()
        {
            _navigationStore = new NavigationStore();
            _musicPlayer = new MusicPlayer();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new PlayerViewModel(_navigationStore, _musicPlayer);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _musicPlayer)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _musicPlayer.DisposeWave();
            base.OnExit(e);
        }
    }
}
