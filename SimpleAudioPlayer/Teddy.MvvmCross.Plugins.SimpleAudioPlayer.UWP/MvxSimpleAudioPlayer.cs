using System;
using System.Diagnostics;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.UWP
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        private const string Drive = ":\\";
        private const double InvalidValue = -1;

        private readonly MediaPlayer _player;
        private bool _isPlaying;

        public MvxSimpleAudioPlayer()
        {
            _player = BackgroundMediaPlayer.Current;
            _player.AutoPlay = false;
            _player.MediaEnded += (sender, args) =>
            {
                Seek(0);
                _isPlaying = false;
            };
        }

        public string Path { get; private set; }

        public double Duration
        {
            get
            {
                if (string.IsNullOrEmpty(Path)) return InvalidValue;

                return _player.PlaybackSession.NaturalDuration.TotalMilliseconds;
            }
        }

        public double Position
        {
            get
            {
                if (string.IsNullOrEmpty(Path)) return InvalidValue;

                return _player.PlaybackSession.Position.TotalMilliseconds;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (string.IsNullOrEmpty(Path)) return false;

                return _isPlaying;
            }
        }

        public double Volume
        {
            get
            {
                if (_player == null) return InvalidValue;

                return _player.Volume;
            }

            set
            {
                if (_player == null) return;

                _player.Volume = (float)value;
            }
        }

        public void Dispose()
        {
        }

        public bool Open(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            _player.Pause();

            if (Equals(Path, path))
            {
                // if opened the same audio, simply seek to beginning
                Seek(0);

                return true;
            }

            try
            {
                // if opened a new file set current path and create a new player instance
                Path = path;

                if (Uri.IsWellFormedUriString(Path, UriKind.Absolute) || Path.Contains(Drive))
                    _player.Source = MediaSource.CreateFromUri(new Uri(path, UriKind.Absolute));
                else
                    _player.Source = MediaSource.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/" + path, UriKind.Absolute)));

                _player.PlaybackSession.Position = TimeSpan.Zero;

                return true;
            }
            catch (Exception ex)
            {
                Stop();

                Debug.WriteLine("Error opening " + Path + ": " + ex.Message);
                return false;
            }
        }

        public void Play()
        {
            if (string.IsNullOrEmpty(Path) || _isPlaying) return;

            _player.Play();
            _isPlaying = true;
        }

        public void Stop()
        {
            if (string.IsNullOrEmpty(Path)) return;

            Pause();
            Seek(0);
        }

        public void Pause()
        {
            if (string.IsNullOrEmpty(Path)) return;

            if (_player.PlaybackSession.CanPause)
            {
                _player.Pause();
                _isPlaying = false;
            }
        }

        public void Seek(double pos)
        {
            if (string.IsNullOrEmpty(Path)) return;

            if (_player.PlaybackSession.CanSeek) _player.PlaybackSession.Position = TimeSpan.FromMilliseconds(pos);
        }
    }
}
