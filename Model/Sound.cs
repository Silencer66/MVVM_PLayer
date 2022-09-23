using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_PLayer.Model
{
    public class Sound
    {
        public bool IsFavorite { get; set; }
        public string SoundName { get; }
        public string SoundPath { get;  }
        public string SoundImage { get; }
        public TimeSpan Duration { get; }

        public Sound()
        {
        }
        //public Sound(string soundName, string soundPath, string imagePath, TimeSpan duration)
        //{
        //    this.soundName = soundName;
        //    this.soundPath = soundPath;
        //    this.imagePath = imagePath;
        //    this.duration = duration;
        //}
        public Sound(string soundName, string soundPath, TimeSpan duraton)
        {
            SoundName = soundName;
            SoundPath = soundPath;
            Duration = duraton;
        }
    }
}
