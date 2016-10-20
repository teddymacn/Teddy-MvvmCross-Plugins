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

        public ICommand PlayCommand { get { return new MvxCommand(() => PlayAudio()); } }

        public ICommand PauseCommand { get { return new MvxCommand(() => _player.Pause()); } }

        public ICommand ResumeCommand { get { return new MvxCommand(() => _player.Resume()); } }

        public ICommand Stop1Command { get { return new MvxCommand(() => _player.Stop(true)); } }

        public ICommand Stop2Command { get { return new MvxCommand(() => _player.Stop(false)); } }

        private void PlayAudio()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                //_player.Play("http://192.168.2.104/test.mp3"); // play from URL

                var request = new MvxFileDownloadRequest("http://169.254.80.80/test.mp3", "test.mp3");
                request.DownloadComplete += (sender, e) => _player.Play(_fileStore.NativePath("test.mp3"));
                request.Start();
            }
            else
            {
                _player.Play("http://169.254.80.80/test.mp3"); // play from URL
            }

            //_player.Play("test.mp3"); // play from assets

            // play from downloaded file
            //var request = new MvxFileDownloadRequest("http://169.254.80.80/test.mp3", "test.mp3");
            //request.DownloadComplete += (sender, e) => _player.Play(_fileStore.NativePath("test.mp3"));
            //request.Start();
        }
    }
}
