using MvvmCross.Core.ViewModels;

namespace SimpleAudioPlayer.Demo.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
