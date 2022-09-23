using MVVM_PLayer.Infrastructure.Commands;
using MVVM_PLayer.Model;
using MVVM_PLayer.Store;
using System.Windows;
using System.Windows.Input;

namespace MVVM_PLayer.ViewModel
{
    internal class SettingsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly MusicPlayer _musicPlayer;

        #region Команды

        #region PlayerCommand
        public ICommand PlayerCommand { get; }
        private bool CanPlayerCommand(object obj) => true;
        private void OnPlayerCommand(object obj)
        {
            _navigationStore.CurrentViewModel = new PlayerViewModel(_navigationStore, _musicPlayer);
        }
        #endregion

        #region AddDirectoryCommand
        public ICommand AddDirectoryCommand { get; }
        private bool CanAddDirectoryCommand(object obj) => true;
        private void OnAddDirectoryCommand(object obj)
        {
            _musicPlayer.AdddDirectory();
        }
        #endregion


        #endregion

        public SettingsViewModel(NavigationStore navigationStore, MusicPlayer musicPlayer)
        {
            _navigationStore = navigationStore;
            _musicPlayer = musicPlayer;
         
            PlayerCommand = new LambdaCommand(OnPlayerCommand, CanPlayerCommand);
            AddDirectoryCommand = new LambdaCommand(OnAddDirectoryCommand, CanAddDirectoryCommand);
        }
    }
}
