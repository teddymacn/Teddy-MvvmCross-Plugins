﻿using Android.App;
using Android.Media;
using System;
using System.Diagnostics;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.Droid
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        private const string Root = "/";
        private const double InvalidValue = -1;

        private MediaPlayer _player;
        private double _volume = 0.5;

        #region IMvxSimpleAudioPlayer Members

        public string Path { get; private set; }

        public double Duration
        {
            get
            {
                if (_player == null) return InvalidValue;

                return _player.Duration;
            }
        }

        public double Position
        {
            get
            {
                if (_player == null) return InvalidValue;

                return _player.CurrentPosition;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (_player == null) return false;

                return _player.IsPlaying;
            }
        }

        public double Volume
        {
            get
            {
                if (_player == null) return InvalidValue;

                return _volume;
            }

            set
            {
                if (_player == null) return;

                _volume = value;
                _player.SetVolume((float)value, (float)value);
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
                _player = new MediaPlayer();
                
                if (Path.StartsWith(Root) || Uri.IsWellFormedUriString(Path, UriKind.Absolute))
                {
                    // for URL or local file path, simply set data source
                    _player.SetDataSource(Path);
                }
                else
                {
                    // search for files with relative path in Assets folder
                    // files in the Assets folder requires to be opened with a FileDescriptor
                    var descriptor = Application.Context.Assets.OpenFd(Path);
                    long start = descriptor.StartOffset;
                    long end = descriptor.Length;
                    _player.SetDataSource(descriptor.FileDescriptor, start, end);
                }
                _player.SetAudioStreamType(Stream.Ring);  // use the ring audio level
                _player.Completion += (sender, e) =>
                {
                    OnCompletion();

                    // seek to beginning on playing completion
                    _player.SeekTo(0);
                };
                _player.Prepare();
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
            if (_player == null || _player.IsPlaying) return;

            _player.Start();
        }

        public void Stop()
        {
            if (_player == null) return;

            if (_player.IsPlaying)
            {
                _player.Stop();

                OnCompletion();

                // after _player.Stop(), re-prepare the audio, otherwise, re-play will fail
                _player.Prepare();

                _player.SeekTo(0);
            }
        }

        public void Pause()
        {
            if (_player == null || !_player.IsPlaying) return;

            _player.Pause();
        }

        public void Seek(double pos)
        {
            if (_player == null) return;

            _player.SeekTo((int)pos);
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        public event EventHandler Completion;

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
            // stop
            if (_player.IsPlaying) _player.Stop();

            // for android, thr call to Reset() is required before calling Release()
            // otherwise, an exception will be thrown when Release() is called
            _player.Reset();

            // release the player, after what the player could not be reused anymore
            _player.Release();
        }

        private void OnCompletion()
        {
            Completion?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
