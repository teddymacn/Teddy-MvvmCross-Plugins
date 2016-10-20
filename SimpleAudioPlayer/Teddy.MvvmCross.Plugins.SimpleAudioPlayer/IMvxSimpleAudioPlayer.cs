using System;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer
{
    public interface IMvxSimpleAudioPlayer : IDisposable
    {
        /// <summary>
        /// Gets the current audio path.
        /// </summary>
        string Path { get;}

        /// <summary>
        /// Gets the duration of the audio.
        /// </summary>
        double Duration { get; }

        /// <summary>
        /// Plays a specified audio path.
        /// 
        /// 4 formats of path are supported:
        ///     - URL, 
        ///       e.g. http://abc.com/test.mp3
        ///       
        ///     - Assets, files in device specific assets folder
        ///       e.g. assets/file.mp3
        ///       
        ///     - Local Relative, arbitry local file path relative to the app's device specific local files folder
        ///       e.g. myaudios/test.mp3
        ///       
        ///     - Local Absolute, arbitry local absolute file path the app has access
        ///       e.g. /sdcard/test.mp3
        /// </summary>
        /// <param name="path">
        ///     A absolute URL, or a local path. If not specified, replay the current audio if exists.
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
}
