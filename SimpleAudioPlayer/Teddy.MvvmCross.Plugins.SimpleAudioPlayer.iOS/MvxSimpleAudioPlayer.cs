﻿using AVFoundation;
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
        private readonly Dictionary<string, AVPlayer> _playerCache = new Dictionary<string, AVPlayer>();
        private AVPlayer _currentPlayer;
        private int _timeScale;

        #region Public Methods

        public string CurrentPath { get; private set; }

        public void Play(string path = null)
        {
            if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(CurrentPath))
                return;
            
            try
            {
                NSError error = null;
                AVAudioSession.SharedInstance().SetCategory(AVAudioSession.CategoryPlayback, out error);

                if (!string.IsNullOrEmpty(path))
                {
                    CurrentPath = path.ToLowerInvariant();

                    if (!_playerCache.TryGetValue(CurrentPath, out _currentPlayer))
                    {
                        if (CurrentPath.StartsWith("http://") || CurrentPath.StartsWith("https://"))
                        {
                            var audioAsset = AVAsset.FromUrl(NSUrl.FromString(CurrentPath));
                            _timeScale = audioAsset.Duration.TimeScale;
                            var audioItem = AVPlayerItem.FromAsset(audioAsset);
                            _currentPlayer = AVPlayer.FromPlayerItem(audioItem);
                        }
                        else
                        {
                            var audioAsset = AVAsset.FromUrl(NSUrl.FromString("file://" + CurrentPath));
                            _timeScale = audioAsset.Duration.TimeScale;
                            var audioItem = AVPlayerItem.FromAsset(audioAsset);
                            _currentPlayer = AVPlayer.FromPlayerItem(audioItem);
                        }
                        _playerCache.Add(CurrentPath, _currentPlayer);
                    }
                    else
                    {
                        StopAndSeekToBegin();
                    }
                }
                
                _currentPlayer.Play();
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
                    
                    _currentPlayer.Dispose();
                    _currentPlayer = null;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error stopping " + CurrentPath + ": " + ex.Message);
                throw;
            }
        }

        public double Duration
        {
            get
            {
                if (_currentPlayer == null) return 0;

                return _currentPlayer.CurrentTime.Seconds;
            }
        }

        public void SeekTo(double pos)
        {
            if (_currentPlayer == null) return;

            _currentPlayer.Seek(CMTime.FromSeconds(pos, _timeScale));
            _currentPlayer.Pause();
        }

        public void Pause()
        {
            if (_currentPlayer == null) return;

            _currentPlayer.Pause();
        }

        public void Resume()
        {
            if (_currentPlayer == null) return;

            _currentPlayer.Play();
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

        private void Release(string path, AVPlayer mp)
        {
            _playerCache.Remove(path);
            mp.Dispose();
        }

        private void StopAndSeekToBegin()
        {
            _currentPlayer.Pause();
            _currentPlayer.Seek(CMTime.FromSeconds(0, _timeScale));
        }

        #endregion
    }
}
