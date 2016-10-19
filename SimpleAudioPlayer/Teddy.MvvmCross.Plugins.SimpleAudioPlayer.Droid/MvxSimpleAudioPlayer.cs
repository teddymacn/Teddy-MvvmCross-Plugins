using Android.App;
using Android.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.Droid
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        private readonly Dictionary<string, MediaPlayer> _playerCache = new Dictionary<string, MediaPlayer>();
        private MediaPlayer _currentPlayer;

        #region Public Methods

        public string CurrentPath { get; private set; }

        public void Play(string path = null)
        {
            if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(CurrentPath))
                return;
            
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    CurrentPath = path.ToLowerInvariant();

                    if (!_playerCache.TryGetValue(CurrentPath, out _currentPlayer))
                    {
                        _currentPlayer = new MediaPlayer();

                        if (CurrentPath.StartsWith("http://") || CurrentPath.StartsWith("https://"))
                            _currentPlayer.SetDataSource(CurrentPath);
                        else if (CurrentPath.StartsWith("files/"))
                            _currentPlayer.SetDataSource(Application.Context.FilesDir + CurrentPath.Substring(5));
                        else if (CurrentPath.StartsWith("cache/"))
                            _currentPlayer.SetDataSource(Application.Context.CacheDir + CurrentPath.Substring(5));
                        else
                        {
                            var descriptor = Application.Context.Assets.OpenFd(CurrentPath);
                            long start = descriptor.StartOffset;
                            long end = descriptor.Length;
                            _currentPlayer.SetDataSource(descriptor.FileDescriptor, start, end);
                        }
                        _currentPlayer.SetAudioStreamType(Stream.Ring);
                        _currentPlayer.Prepare();
                        _playerCache.Add(CurrentPath, _currentPlayer);
                    }
                    else
                    {
                        StopAndSeekToBegin();
                    }
                }
                
                _currentPlayer.Start();
            }
            catch(Exception ex)
            {
                Stop();

                Debug.WriteLine("Error playing " + CurrentPath + ": " + ex.Message);
                throw;
            }
        }

        public void Stop(bool keepCache = false)
        {
            if (string.IsNullOrEmpty(CurrentPath)) return;

            try
            {
                StopAndSeekToBegin();

                if (!keepCache)
                {
                    _playerCache.Remove(CurrentPath);
                    CurrentPath = null;

                    _currentPlayer.Reset();
                    _currentPlayer.Release();
                    _currentPlayer = null;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error stopping " + CurrentPath + ": " + ex.Message);
                throw;
            }
        }

        public int Duration
        {
            get
            {
                if (_currentPlayer == null) return 0;

                return _currentPlayer.Duration;
            }
        }

        public void SeekTo(int pos)
        {
            if (_currentPlayer == null) return;

            _currentPlayer.SeekTo(pos);
        }

        public void Pause()
        {
            if (_currentPlayer == null) return;

            _currentPlayer.Pause();
        }

        public void Resume()
        {
            if (_currentPlayer == null) return;

            _currentPlayer.Start();
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

                // stop and release all playing audios
                CurrentPath = null;
                _currentPlayer = null;
                foreach (var path in _playerCache.Keys.ToList())
                {
                    var mp = _playerCache[path];

                    Release(path, mp);
                }

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

        private void Release(string path, MediaPlayer mp)
        {
            _playerCache.Remove(path);
            if (mp.IsPlaying) mp.Stop();
            mp.Reset();
            mp.Release();
        }

        private void StopAndSeekToBegin()
        {
            if (_currentPlayer.IsPlaying)
            {
                _currentPlayer.Stop();
                _currentPlayer.Prepare();
            }
            _currentPlayer.SeekTo(0);
        }

        #endregion
    }
}
