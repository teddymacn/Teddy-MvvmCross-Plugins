using System;
using System.Collections.Generic;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer
{
    public static class SimpleAudioPlayerExtensions
    {
        public static void SetUpLooping(this IMvxSimpleAudioPlayer player)
        {
            player.Completion += ReplayOnCompletion;
        }

        public static void TearDownLooping(this IMvxSimpleAudioPlayer player)
        {
            player.Completion -= ReplayOnCompletion;
        }

        private static void ReplayOnCompletion(object sender, EventArgs e)
        {
            IMvxSimpleAudioPlayer player = sender as IMvxSimpleAudioPlayer;
            player.Play();
        }

        
    }
}
