using System;
using System.Diagnostics;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.UWP
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        private const string Drive = ":\\";
        private const double InvalidDuration = -1;

        private readonly MediaPlayer _player;

        public MvxSimpleAudioPlayer()
        {
            _player = BackgroundMediaPlayer.Current;
            _player.AutoPlay = false;
        }

        public string Path { get; private set; }

        public double Duration
        {
            get
            {
                if (string.IsNullOrEmpty(Path)) return InvalidDuration;

                return _player.PlaybackSession.NaturalDuration.TotalMilliseconds;
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

                if (Uri.IsWellFormedUriString(Path, UriKind.Absolute))
                    _player.Source = MediaSource.CreateFromUri(new Uri(Path));
                else if (Path.Contains(Drive))
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
            if (string.IsNullOrEmpty(Path)) return;

            _player.Play();
        }

        public void Stop()
        {
            if (string.IsNullOrEmpty(Path)) return;

            _player.Pause();
        }

        public void Pause()
        {
            if (string.IsNullOrEmpty(Path)) return;

            if (_player.PlaybackSession.CanPause) _player.Pause();
        }

        public void Seek(double pos)
        {
            if (string.IsNullOrEmpty(Path)) return;

            if (_player.PlaybackSession.CanSeek) _player.PlaybackSession.Position = TimeSpan.FromMilliseconds(pos);
        }
    }
}
