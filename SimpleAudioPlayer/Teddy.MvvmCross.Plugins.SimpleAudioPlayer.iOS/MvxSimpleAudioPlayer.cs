using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.iOS
{
    public class MvxSimpleAudioPlayer : IMvxSimpleAudioPlayer
    {
        public string CurrentPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Duration
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play(string path = null)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void SeekTo(int pos)
        {
            throw new NotImplementedException();
        }

        public void Stop(bool keepCache = false)
        {
            throw new NotImplementedException();
        }
    }
}
