using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.DownloadCache;
using MvvmCross.Plugins.File;
using System.Windows.Input;
using Teddy.MvvmCross.Plugins.SimpleAudioPlayer;
using Xamarin.Forms;

namespace SimpleAudioPlayer.Demo.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxSimpleAudioPlayer _player;
        private readonly IMvxFileStore _fileStore;

        public MainViewModel(IMvxSimpleAudioPlayer player
            , IMvxFileStore fileStore
            )
        {
            _player = player;
            _fileStore = fileStore;
        }

        public ICommand OpenCommand { get { return new MvxCommand(() => OpenAudio()); } }

        public ICommand PlayCommand { get { return new MvxCommand(() => _player.Play()); } }

        public ICommand PauseCommand { get { return new MvxCommand(() => _player.Pause()); } }

        public ICommand StopCommand { get { return new MvxCommand(() => _player.Stop()); } }

        private void OpenAudio()
        {
            // for testing with remote audio, you need to setup a web server to serve the test.mp3 file
            // and please change the server address below
            // according to your local machine, device or emulator's network settings

            string server = (Device.OS == TargetPlatform.Android) ?
                "http://169.254.80.80" // default host address for Android emulator
                :
                "http://192.168.2.104"; // my local machine's intranet ip, change to your server's instead

            // by default, testing playing audio from Assets
            _player.Open("test.mp3");
            _player.Play();

            // comment the code above and uncomment the code below
            // if you want to test playing a remote audio by URL
            //_player.Open(server + "/test.mp3");
            //_player.Play();

            // comment the code above and uncomment the code below
            // if you want to test playing a downloaded audio
            //var request = new MvxFileDownloadRequest(server + "/test.mp3", "test.mp3");
            //request.DownloadComplete += (sender, e) =>
            //{
            //    _player.Open(_fileStore.NativePath("test.mp3"));
            //    _player.Play();
            //};
            //request.Start();
        }
    }
}
