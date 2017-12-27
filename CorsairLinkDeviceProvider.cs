using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RGB.NET.Devices.CorsairLink
{
    public class CorsairLinkDeviceProvider : IRGBDeviceProvider
    {

        #region Properties & Fields

        private static CorsairLinkDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="CorsarLinkDeviceProvider"/> instance.
        /// </summary>
        public static CorsairLinkDeviceProvider Instance => _instance ?? new CorsairLinkDeviceProvider();

        public bool HasExclusiveAccess => false; // we don't really need this


        public bool IsInitialized { get; private set; }

        public IEnumerable<IRGBDevice> Devices { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairLinkDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public CorsairLinkDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instanc of type {nameof(CorsairLinkDeviceProvider)}");
            _instance = this;
        }

        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;
            try
            {
                IList<IRGBDevice> devices = new List<IRGBDevice>();
                ICorsairLinkRGBDevice device = new CorsairLinkRGBDevice(new CorsairLinkRGBDeviceInfo());
                device.Initialize();
                devices.Add(device);
                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
            }
            catch
            {
                if (throwExceptions)
                    throw;
                else
                    return false;
            }

            IsInitialized = true;
            return true;
        }

        public void ResetDevices()
        {
            // we don't really need this
        }


    }
}


