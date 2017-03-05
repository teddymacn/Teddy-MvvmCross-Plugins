using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer
{
    public class MvxSimpleLoopingAudioPlayer : IMvxSimpleLoopingAudioPlayer
    {
        private IMvxSimpleAudioPlayer _player;

        public MvxSimpleLoopingAudioPlayer(
            IMvxSimpleAudioPlayer player)
        {
            _player = player;
            SetupReplayOnCompletion();
        }

        public double Duration
        {
            get { return _player.Duration; }
        }

        public bool IsPlaying
        {
            get { return _player.IsPlaying; }
        }

        public string Path
        {
            get { return _player.Path; }
        }

        public double Position
        {
            get { return _player.Position; }
        }

        public double Volume
        {
            get { return _player.Volume; }
            set { _player.Volume = value; }
        }

        public bool Open(string path)
        {
            return _player.Open(path);
        }
        
        public void Pause()
        {
            _player.Pause();
        }

        public void PlayOnLoop()
        {
            _player.Play();
        }

        public void Seek(double pos)
        {
            _player.Seek(pos);
        }

        public void Stop()
        {
            _player.Stop();
        }

        private void SetupReplayOnCompletion()
        {
            _player.Completion += new EventHandler(delegate (Object o, EventArgs a)
            {
                _player.Play();
            });
        }
        
    }
}
