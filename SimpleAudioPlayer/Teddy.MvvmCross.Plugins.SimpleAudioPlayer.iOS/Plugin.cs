using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.iOS
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxSimpleAudioPlayer, MvxSimpleAudioPlayer>();
        }
    }
}