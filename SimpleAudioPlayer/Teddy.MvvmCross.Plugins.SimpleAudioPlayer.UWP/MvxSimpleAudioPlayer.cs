using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.UWP
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        private readonly MediaPlayer _player;

        public MvxSimpleAudioPlayer()
        {
            _player = BackgroundMediaPlayer.Current;
            _player.AutoPlay = false;
        }

        public string CurrentPath { get; private set; }

        public double Duration
        {
            get
            {
                return _player.PlaybackSession.NaturalDuration.TotalMilliseconds;
            }
        }

        public void Dispose()
        {
        }

        public void Pause()
        {
            if (_player.PlaybackSession.CanPause) _player.Pause();
        }

        public void Play(string path = null)
        {
            if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(CurrentPath))
                return;

            if (!string.IsNullOrEmpty(path))
            {
                CurrentPath = path.ToLowerInvariant();
            }

            if (path.StartsWith("http://") || path.StartsWith("https://"))
                _player.Source = MediaSource.CreateFromUri(new Uri(path));
            else if (path.StartsWith("assets/"))
                _player.Source = MediaSource.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/" + path.Substring(7), UriKind.Absolute)));
            else
                _player.Source = MediaSource.CreateFromUri(new Uri(path, UriKind.Absolute));

            _player.PlaybackSession.Position = TimeSpan.Zero;
            _player.Play();
        }

        public void Resume()
        {
            _player.Play();
        }

        public void SeekTo(double pos)
        {
            if (_player.PlaybackSession.CanSeek) _player.PlaybackSession.Position = TimeSpan.FromMilliseconds(pos);
        }

        public void Stop(bool keepCache = false)
        {
            _player.Pause();
        }
    }
}
