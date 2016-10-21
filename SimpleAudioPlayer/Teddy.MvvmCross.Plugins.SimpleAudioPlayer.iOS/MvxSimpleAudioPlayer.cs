using AVFoundation;
using CoreMedia;
using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.iOS
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        private const string Root = "/";
        private const double InvalidDuration = -1;

        private AVPlayer _player;
        private int _timeScale;

        #region IMvxSimpleAudioPlayer Members

        public string Path { get; private set; }

        public double Duration
        {
            get
            {
                if (_player == null) return InvalidDuration;

                return _player.CurrentTime.Seconds * 1000;
            }
        }

        public bool Open(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // release the previous inner player if opened a new file
            if (_player != null && !Equals(Path, path))
                ReleasePlayer();

            if (Equals(Path, path))
            {
                // if opened the same audio, simply stop and seek to beginning
                Stop();
                Seek(0);

                return true;
            }

            try
            {
                // if opened a new file set current path and create a new player instance
                Path = path;

                NSError error = null;
                AVAudioSession.SharedInstance().SetCategory(AVAudioSession.CategoryPlayback, out error);

                AVAsset audioAsset;
                if (Uri.IsWellFormedUriString(Path, UriKind.Absolute))
                    audioAsset = AVAsset.FromUrl(NSUrl.FromString(Path));
                else if (Path.StartsWith(Root))
                    audioAsset = AVAsset.FromUrl(NSUrl.FromString("file://" + Path));
                else
                    audioAsset = AVAsset.FromUrl(NSUrl.FromFilename(Path));

                _timeScale = audioAsset.Duration.TimeScale;
                var audioItem = AVPlayerItem.FromAsset(audioAsset);
                _player = AVPlayer.FromPlayerItem(audioItem);
                _player.AddBoundaryTimeObserver(
                    times: new[] { NSValue.FromCMTime(audioAsset.Duration) },
                    queue: null,
                    handler: () => Seek(0));

                return true;
            }
            catch(Exception ex)
            {
                Stop();

                Debug.WriteLine("Error opening " + Path + ": " + ex.Message);
                return false;
            }
        }

        public void Play()
        {
            if (_player == null) return;

            _player.Play();
        }

        public void Stop()
        {
            if (_player == null) return;

            _player.Pause();
            _player.Seek(CMTime.FromSeconds(0, _timeScale));
        }
        
        public void Pause()
        {
            if (_player == null) return;

            _player.Pause();
        }

        public void Seek(double pos)
        {
            if (_player == null) return;

            _player.Seek(CMTime.FromSeconds(pos / 1000, _timeScale));
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                }

                ReleasePlayer();

                Path = null;
                _player = null;

                disposedValue = true;
            }
        }

        ~MvxSimpleAudioPlayer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        private void ReleasePlayer()
        {
            _player.Dispose();
        }

        #endregion
    }
}
