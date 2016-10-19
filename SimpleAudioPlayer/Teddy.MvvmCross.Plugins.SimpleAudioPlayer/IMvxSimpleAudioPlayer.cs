using System;

namespace Teddy.MvvmCross.Plugins.SimpleAudioPlayer
{
    public interface IMvxSimpleAudioPlayer : IDisposable
    {
        /// <summary>
        /// Gets the current audio path.
        /// </summary>
        string CurrentPath { get;}

        /// <summary>
        /// Plays a specified audio URL or local file.
        /// </summary>
        /// <param name="path">
        ///     A absolute URL, or a local path. If not specified, replay the current audio if exists.
        /// </param>
        void Play(string path = null);

        /// <summary>
        /// Stops playing.
        /// </summary>
        /// <param name="keepCache">Whether or not to keep the audio cache in memory.</param>
        void Stop(bool keepCache = false);

        /// <summary>
        /// Gets the duration of the audio.
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// Seeks to specified position.
        /// </summary>
        /// <param name="pos">The position to seek to.</param>
        void SeekTo(int pos);

        /// <summary>
        /// Pauses the playing.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the playing.
        /// </summary>
        void Resume();
    }
}
