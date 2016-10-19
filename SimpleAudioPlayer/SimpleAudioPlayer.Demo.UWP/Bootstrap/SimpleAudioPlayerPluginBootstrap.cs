using MvvmCross.Platform.Plugins;

namespace SimpleAudioPlayer.Demo.UWP.Bootstrap
{
    // the bootstrap feature has issue on windows 10
    // register the IMvxSimplePlayer interface in Setup class instead

    public class SimpleAudioPlayerPluginBootstrap
        : MvxLoaderPluginBootstrapAction<Teddy.MvvmCross.Plugins.SimpleAudioPlayer.PluginLoader, Teddy.MvvmCross.Plugins.SimpleAudioPlayer.UWP.Plugin>
    { }
}
