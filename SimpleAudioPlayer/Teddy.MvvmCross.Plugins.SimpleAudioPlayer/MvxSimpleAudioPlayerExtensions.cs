using System;
using System.Collections.Generic;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer
{
    public static class SimpleAudioPlayerExtensions
    {
        public static void SetUpLooping(this IMvxSimpleAudioPlayer player)
        {
            var playbackHandler = new EventHandler(delegate (Object o, EventArgs a)
            {
                player.Play();
            });

            registeredPlaybackHandler[player] = playbackHandler;
            player.Completion += playbackHandler;
        }

        public static void TearDownLooping(this IMvxSimpleAudioPlayer player)
        {
            player.Completion -= registeredPlaybackHandler[player];
            registeredPlaybackHandler.Remove(player);
        }

        private static Dictionary<IMvxSimpleAudioPlayer, EventHandler> registeredPlaybackHandler;
    }
}
