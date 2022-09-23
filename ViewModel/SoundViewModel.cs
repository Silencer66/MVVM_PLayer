using MVVM_PLayer.Model;
using System;

namespace MVVM_PLayer.ViewModel
{
    internal class SoundViewModel : ViewModelBase
    {
        private readonly Sound _sound;
        public string SoundPath =>_sound.SoundPath;
        public string SoundName => _sound?.SoundName;
        public string SoundImage => _sound?.SoundImage;
        public bool IsFavorite => _sound.IsFavorite;
        public TimeSpan  Duration => _sound.Duration;

        public SoundViewModel(Sound sound)
        {
            _sound = sound;
        }
    }
}
