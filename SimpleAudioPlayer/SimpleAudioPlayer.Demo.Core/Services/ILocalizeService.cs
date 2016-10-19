using System.Globalization;

namespace SimpleAudioPlayer.Demo.Core.Services
{
    public interface ILocalizeService
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
