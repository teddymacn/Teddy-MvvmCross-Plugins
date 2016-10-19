using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer.Droid
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