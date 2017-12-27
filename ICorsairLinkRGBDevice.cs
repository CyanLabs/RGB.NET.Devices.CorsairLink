using RGB.NET.Core;

namespace RGB.NET.Devices.CorsairLink
{
    /// <summary>
    /// Represents a Corsair Link RGB-device.
    /// </summary>
    internal interface ICorsairLinkRGBDevice : IRGBDevice
    {
        void Initialize();
    }
}