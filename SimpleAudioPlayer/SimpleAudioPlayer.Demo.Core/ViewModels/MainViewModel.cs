using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.DownloadCache;
using System.Windows.Input;
using Teddy.MvvmCross.Plugins.SimpleAudioPlayer;

namespace SimpleAudioPlayer.Demo.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxSimpleAudioPlayer _player;
        private readonly IMvxHttpFileDownloader _downloader;

        public MainViewModel(IMvxSimpleAudioPlayer player, IMvxHttpFileDownloader downloader)
        {
            _player = player;
            _downloader = downloader;
        }

        public ICommand PlayCommand { get { return new MvxCommand(() => PlayAudio()); } }

        public ICommand PauseCommand { get { return new MvxCommand(() => _player.Pause()); } }

        public ICommand ResumeCommand { get { return new MvxCommand(() => _player.Resume()); } }

        public ICommand Stop1Command { get { return new MvxCommand(() => _player.Stop(true)); } }

        public ICommand Stop2Command { get { return new MvxCommand(() => _player.Stop(false)); } }

        private void PlayAudio()
        {
            //_player.Play("http://169.254.80.80/test.mp3"); // play from URL

            _player.Play("test.mp3"); // play from assets

            // play from downloaded file
            //var request = new MvxFileDownloadRequest("http://169.254.80.80/test.mp3", "test.mp3");
            //request.DownloadComplete += (sender, e) => _player.Play("files/test.mp3");
            //request.Start();
        }
    }
}
