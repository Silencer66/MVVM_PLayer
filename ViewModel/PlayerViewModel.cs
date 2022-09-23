using MVVM_PLayer.Infrastructure.Commands;
using MVVM_PLayer.Model;
using MVVM_PLayer.Store;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVM_PLayer.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private DispatcherTimer timerPlayTime;
        private bool ignoreChange;

        private readonly NavigationStore _navigationStore;
        private readonly MusicPlayer _musicPlayer;
        private readonly ObservableCollection<SoundViewModel> _sounds;
        public ObservableCollection<SoundViewModel> Sounds => _sounds;

        #region Свойства

        #region SelectedSound
        private SoundViewModel _selectedSound;
        /// <summary> Выбранный элемент в ListView </summary>
        public SoundViewModel SelectedSound
        {
            get => _selectedSound;
            set
            {
                Set<SoundViewModel>(ref _selectedSound, value);
                StartPlaying(_selectedSound);
            }
        }
        #endregion

        #region SoundName
        private string _soundName;
        /// <summary> Имя трека</summary>
        public string SoundName
        {
            get { return _soundName; }
            set
            {
                Set<string>(ref _soundName, value);
            }
        }
        #endregion

        #region CurrentValueSlider
        private long _currentValueSlider = 0;
        public long CurrentValueSlider
        {
            get => _currentValueSlider;
            set
            {
                if (!ignoreChange)
                    _musicPlayer.CurrentPosition = _currentValueSlider;
                Set<long>(ref _currentValueSlider, value);
            }
        }
        #endregion

        #region CurrentValueVolume
        private float _currentValueVolume;
        public float CurrentValueVolume
        {
            get =>_currentValueVolume;
            set 
            { 
                Set<float>(ref _currentValueVolume, value);                
            }
        }

        #endregion

        #region MaximumValueSlider
        private long _maximumValueSlider = 0;
        public long MaximumValueSlider
        {
            get => _maximumValueSlider;
            set => Set<long>(ref _maximumValueSlider, value);
        }
        #endregion

        #region ImageOfSound
        private string _imageOfSound;
        /// <summary> Изображение трека </summary>
        public string ImageOfSound
        {
            get { return _imageOfSound; }
            set
            {
            }
        }
        #endregion

        #region ElapsedTime
        private string _elapsedTime = "0:00";
        /// <summary> Время проейденное треком </summary>
        public string ElapsedTime
        {
            get => _elapsedTime;
            set => Set<string>(ref _elapsedTime, value);
        }
        #endregion

        #region Duration
        private string _duration = "0:00";
        /// <summary> Все время трека </summary>
        public string Duration
        {
            get { return _duration; }
            set => Set<string>(ref _duration, value);
        }
        #endregion

        #region IsPlayImage
        private bool _isPlayImage = true;

        /// <summary> Изображение кнопки PlayPause </summary>
        /// true - кнопка play; false - кнопка pause 
        public bool IsPlayImage
        {
            get { return _isPlayImage; }
            set => Set<bool>(ref _isPlayImage, value);
        }
        #endregion

        #region IsRepeatTrack
        private bool _isRepeatTrack;
        public bool IsRepeatTrack
        {
            get { return _isRepeatTrack; }
            set => Set<bool>(ref _isRepeatTrack, value);
        }
        #endregion



        #endregion

        #region Команды

        #region ShuffleCommand
        public ICommand ShuffleCommand { get; }
        private bool CanShuffleCommand(object obj)
        {
            if (_sounds.Count == 0) return false;
            else return true;
        }
        private void OnShuffleCommand(object obj)
        {
            Random rnd = new Random();
            int value = rnd.Next(0, _sounds.Count);
            _selectedSound = _sounds[value];
            StartPlaying(_selectedSound);
        }
        #endregion

        #region PreviousTrackCommand
        public ICommand PreviousTrackCommand { get; }
        private bool CanPreviousTrackCommand(object obj)
        {
            if (_selectedSound == null) return false;
            else return true;
        }
        private void OnPreviousTrackCommand(object obj)
        {
            var index = _sounds.IndexOf(_selectedSound);
            SoundViewModel previousSound;
            if (index == 0)
                previousSound = _sounds[_sounds.Count - 1];
            else
                previousSound = _sounds[index - 1];
            SelectedSound = previousSound;
        }
        #endregion

        #region NextTrackCommand
        public ICommand NextTrackCommand { get; }
        private bool CanNextTrackCommand(object obj)
        {
            if (_selectedSound == null) return false;
            else return true;
        }
        private void OnNextTrackCommand(object obj)
        {
            var index = _sounds.IndexOf(_selectedSound);
            SoundViewModel nextSound;
            if (index == _sounds.Count - 1)
                nextSound = _sounds[0];
            else
                nextSound = _sounds[index + 1];
            SelectedSound = nextSound;
        }
        #endregion

        #region PlayPauseCommand
        public ICommand PlayPauseCommand { get; }
        private bool CanPlayPauseCommand(object obj)
        {
            if (_selectedSound == null) return false;
            else return true;
        }
        private void OnPlayPauseCommand(object obj)
        {
            _musicPlayer.PlayPausePlayback();
            IsPlayImage = !IsPlayImage;
        }
        #endregion

        #region RepeatCommand
        public ICommand RepeatCommand { get; }
        private bool CanRepeatCommand(object obj)
        {
            if (_selectedSound == null) return false;
            else return true;

        }
        private void OnRepeatCommand(object obj)
        {
            IsRepeatTrack = !IsRepeatTrack;
        }
        #endregion

        #region VolumeCommand
        public ICommand VolumeCommand { get; }
        private void OnVolumeCommand(object obj)
        {
            _musicPlayer.CurrentVolume = _currentValueVolume;
        }
        private bool CanVolumeCommand(object obj)
        {
            if (_sounds.Count == 0) return false;
            else return true;
        }
        #endregion

        #region ChangePositionCommand
        public ICommand ChangePositionCommand { get; }
        private bool CanChangePositionCommand(object obj)
        {
            if (_selectedSound == null) return false;
            else return true;
        }
        private void OnChangePositionCommand(object obj)
        {
            timerPlayTime.Stop();
            _musicPlayer.CurrentPosition = CurrentValueSlider;
            timerPlayTime.Start();
        }
        #endregion

        #region SettingsCommand
        public ICommand SettingsCommand { get; }
        private bool CanSettingsCommand(object obj) => true;
        private void OnSettingsCommand(object obj)
        {
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore, _musicPlayer);
        }
        #endregion

        #endregion

        public PlayerViewModel(NavigationStore navigationStore, MusicPlayer musicPlayer)
        {
            _navigationStore = navigationStore;
            _musicPlayer = musicPlayer;
            _sounds = new ObservableCollection<SoundViewModel>();
            if (_selectedSound != null)
                CurrentValueSlider = _musicPlayer.CurrentPosition;
            UpdateSounds();

            #region Команды
            SettingsCommand = new LambdaCommand(OnSettingsCommand, CanSettingsCommand);
            PlayPauseCommand = new LambdaCommand(OnPlayPauseCommand, CanPlayPauseCommand);
            NextTrackCommand = new LambdaCommand(OnNextTrackCommand, CanNextTrackCommand);
            PreviousTrackCommand = new LambdaCommand(OnPreviousTrackCommand, CanPreviousTrackCommand);
            ChangePositionCommand = new LambdaCommand(OnChangePositionCommand, CanChangePositionCommand);
            ShuffleCommand = new LambdaCommand(OnShuffleCommand, CanShuffleCommand);
            RepeatCommand = new LambdaCommand(OnRepeatCommand, CanRepeatCommand);
            VolumeCommand = new LambdaCommand(OnVolumeCommand, CanVolumeCommand);
            #endregion
        }

        /// <summary> Обновление списка с аудиозаписями </summary>
        private void UpdateSounds()
        {
            _sounds.Clear();

            foreach (var item in _musicPlayer.AllSounds)
            {
                SoundViewModel sound = new SoundViewModel(item);
                _sounds.Add(sound);
            }
        }

        /// <summary> Начало проигрывания аудиозаписи </summary>
        /// <param name="selectedSound">аудиозапись</param>
        private void StartPlaying(SoundViewModel selectedSound)
        {
            _musicPlayer.StartPlayback(selectedSound.SoundPath);
            Duration = selectedSound.Duration.Minutes + ":" + selectedSound.Duration.Seconds;
            SoundName = selectedSound.SoundName;
            IsPlayImage = false;
            MaximumValueSlider = _musicPlayer.TotalTime;
            StartSlider();
        }

        private void StartSlider()
        {
            MaximumValueSlider = _musicPlayer.TotalTime;

            // Создаем таймер, который обновляет таймер и позицию слайдера
            timerPlayTime = new DispatcherTimer();
            timerPlayTime.Interval = TimeSpan.FromSeconds(1);
            timerPlayTime.Tick += new EventHandler(timer_Tick);
            timerPlayTime.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //Когда дошло до конца трека будет проигрывать следующий в списке
            if (ElapsedTime == Duration)
            {
                if (IsRepeatTrack)
                {
                    StartPlaying(_selectedSound);
                }
                else
                {
                    object obj = null;
                    OnNextTrackCommand(obj);
                }
            }
            ElapsedTime = _musicPlayer.CurrentTime.Minutes + ":" + _musicPlayer.CurrentTime.Seconds;

            ignoreChange = true;
            CurrentValueSlider = _musicPlayer.CurrentPosition;
            ignoreChange = false;
        }
    }
}
