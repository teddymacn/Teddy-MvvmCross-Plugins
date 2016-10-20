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
        ///     - Assets, relative path assumed to be in the device specific assets folder
        ///       e.g. test.mp3
        ///       
        ///     - Local, arbitry local absolute file path the app has access
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
}
