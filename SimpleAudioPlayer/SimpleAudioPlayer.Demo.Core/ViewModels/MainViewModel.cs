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
        private bool _showOpenButton = true;
        private bool _showPlayButton = false;
        private bool _showPauseButton = false;
        private bool _showStopButton = false;

        public MainViewModel(IMvxSimpleAudioPlayer player
            , IMvxFileStore fileStore
            )
        {
            _player = player;
            _fileStore = fileStore;

            _player.Completion += (sender, e) =>
            {
                ShowOpenButton = true;
                ShowPlayButton = true;
                ShowPauseButton = false;
                ShowStopButton = false;
            };
        }

        public bool ShowOpenButton
        {
            get { return _showOpenButton; }
            set
            {
                _showOpenButton = value;
                RaisePropertyChanged(() => ShowOpenButton);
            }
        }
        public bool ShowPlayButton
        {
            get { return _showPlayButton; }
            set
            {
                _showPlayButton = value;
                RaisePropertyChanged(() => ShowPlayButton);
            }
        }

        public bool ShowPauseButton
        {
            get { return _showPauseButton; }
            set
            {
                _showPauseButton = value;
                RaisePropertyChanged(() => ShowPauseButton);
            }
        }

        public bool ShowStopButton
        {
            get { return _showStopButton; }
            set
            {
                _showStopButton = value;
                RaisePropertyChanged(() => ShowStopButton);
            }
        }

        public ICommand OpenCommand { get { return new MvxCommand(() => OpenAudio()); } }

        public ICommand PlayCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _player.Play();

                    ShowOpenButton = false;
                    ShowPlayButton = false;
                    ShowPauseButton = true;
                    ShowStopButton = true;
                });
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _player.Pause();

                    ShowPlayButton = true;
                });
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _player.Stop();

                    ShowOpenButton = true;
                    ShowPlayButton = true;
                    ShowPauseButton = false;
                    ShowStopButton = false;
                });
            }
        }

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
            _player.Volume = 1;
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

            ShowPauseButton = true;
            ShowStopButton = true;
            ShowPlayButton = false;
            ShowOpenButton = false;
        }
    }
}
