using System;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer
{
    public static class SimpleAudioPlayerExtensions
    {
        public static void PlayOnRepeat(this IMvxSimpleAudioPlayer player)
        {
            player.Completion += new EventHandler(delegate (Object o, EventArgs a)
            {
                player.Play();
            });

            player.Play();
        }
    }
}
