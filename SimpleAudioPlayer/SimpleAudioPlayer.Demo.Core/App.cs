using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using Xamarin.Forms;
using MvvmCross.Platform;

namespace SimpleAudioPlayer.Demo.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes().
                EndingWith("Repository")
                .AsTypes()
                .RegisterAsLazySingleton();

            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                Resources.AppResources.Culture = Mvx.Resolve<Services.ILocalizeService>().GetCurrentCultureInfo();
            }

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
