using Microsoft.WindowsAPICodePack.Dialogs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVVM_PLayer.Model
{
    public class MusicPlayer
    {
        private CommonOpenFileDialog _ofd;
        private string filename;
        private WaveOutEvent _outputDevice;

        private AudioFileReader _audioFile;


        ///<summary> Получаем текущую позицию трека </summary>
        public long CurrentPosition
        {
            get => _audioFile.Position;
            set => _audioFile.Position = value;
        }

        ///<summary> Получаем всю длину трека </summary>
        public long TotalTime
        {
            get => _audioFile.Length;
        }

        /// <summary> Получаем текущую временную метку в аудиофайле </summary>
        public TimeSpan CurrentTime 
        { 
            get => _audioFile.CurrentTime; 
        }


        public float CurrentVolume
        {
            get => _outputDevice.Volume;
            set => _outputDevice.Volume = value;
        }
        /// <summary> Список всех треков </summary>
        public List<Sound> AllSounds { get; set; }

        public MusicPlayer()
        {
            AllSounds = new List<Sound>();
            _ofd = new CommonOpenFileDialog();
        }

        public void StartPlayback(string file)
        {
            if (_outputDevice != null)
                _outputDevice.Stop();
            
            _audioFile = new AudioFileReader(file);
            _outputDevice.Init(_audioFile);
            _outputDevice.Play();
        }

        public void PausePlayback()
        {
            _outputDevice?.Pause();
        }
        public void PlayPausePlayback()
        {
            if (_outputDevice != null)
            {
                if (_outputDevice.PlaybackState == PlaybackState.Playing) _outputDevice.Pause();
                else if (_outputDevice.PlaybackState == PlaybackState.Paused) _outputDevice.Play();
            }
        }

        /// <summary> Уничтожение объектов </summary>
        public void DisposeWave()
        {
            if (_outputDevice != null)
            {
                if (_outputDevice.PlaybackState == PlaybackState.Playing) _outputDevice.Stop();
                _outputDevice.Dispose();
                _outputDevice = null;
            }
            if (_audioFile != null)
            {
                _audioFile.Dispose();
                _audioFile = null;
            }
        }

        /// <summary> Добавления папки с музыкой </summary>
        public void AdddDirectory()
        {
            _ofd.IsFolderPicker = true;
            if (_ofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (filename != _ofd.FileName)
                {
                    filename = _ofd.FileName;
                    DisposeWave();
                    _outputDevice = new WaveOutEvent();
                    Mp3FileReader mp3;
                    List<string> Sounds = Directory.GetFiles(_ofd.FileName).Where(p => p.Contains(".mp3")).ToList();
                    foreach (var sound in Sounds)
                    {
                        mp3 = new Mp3FileReader(sound);
                        AllSounds.Add(new Sound(Path.GetFileNameWithoutExtension(sound), sound, mp3.TotalTime));
                    }
                }
            }
        }
    }
}
