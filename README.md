## Teddy's MvvmCross Plugins

### SimpleAudioPlayer

The `SimpleAudioPlayer` plugin provides cross-platform simple audio play API. It supports Droid, iOS and UWP platforms.

To deal with absolute file path cross platform, it is suggested to use the [File](https://github.com/MvvmCross/MvvmCross-Plugins/tree/master/File) plugin, use IMvxFileStore.NativePath() method.

For examples, please check the [SimpleAudioPlayer.Demo](https://github.com/teddymacn/Teddy-MvvmCross-Plugins/tree/master/SimpleAudioPlayer).

    public interface IMvxSimpleAudioPlayer : IDisposable
    {
        /// <summary>
        /// Gets the current audio path.
        /// </summary>
        string Path { get;}

        /// <summary>
        /// Gets the duration of the audio in milliseconds.
        /// </summary>
        double Duration { get; }

        /// <summary>
        /// Gets the current position in milliseconds.
        /// </summary>
        double Position { get; }

        /// <summary>
        /// Whether or not it is playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets or sets the current volume.
        /// </summary>
        double Volume { get; set; }

        /// <summary>
        /// Opens a specified audio path.
        /// 
        /// The following formats of path are supported:
        ///     - Absolute URL, 
        ///       e.g. http://abc.com/test.mp3
        ///       
        ///     - Assets Deployed with App, relative path assumed to be in the device specific assets folder
        ///       Android and UWP relative to the Assets folder while iOS relative to the App root folder
        ///       e.g. test.mp3
        ///       
        ///     - Local File System, arbitry local absolute file path the app has access
        ///       e.g. /sdcard/test.mp3
        /// </summary>
        /// <param name="path">
        ///     The audio path.
        /// </param>
        bool Open(string path);

        /// <summary>
        /// Plays the opened audio.
        /// </summary>
        void Play();

        /// <summary>
        /// Stops playing.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the playing.
        /// </summary>
        void Pause();

        /// <summary>
        /// Seeks to specified position in milliseconds.
        /// </summary>
        /// <param name="pos">The position to seek to.</param>
        void Seek(double pos);
    }

### Blogs

- [My First MvvmCross Plugin - SimpleAudioPlayer (Chinese)](http://www.cnblogs.com/teddyma/p/xamarin_mvvmcross_plugin_simpleaudioplayer.html)
