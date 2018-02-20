using HidSharp;
using RGB.NET.Core;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Devices.CorsairLink
{
    public class CorsairLinkRGBDevice : AbstractRGBDevice<CorsairLinkRGBDeviceInfo>, ICorsairLinkRGBDevice
    {
        public override CorsairLinkRGBDeviceInfo DeviceInfo { get; }
        static HidDevice corsairLNP;
        static HidStream lnpStream;

        public CorsairLinkRGBDevice(CorsairLinkRGBDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        public void Initialize()
        {
            InitializeLayout();

            try
            {
                corsairLNP = new HidDeviceLoader().GetDevices().Where(d => d.ProductID == 0x0C10 || d.ProductID == 0x0C0B).First();
                lnpStream = corsairLNP.Open();

                if (corsairLNP != null)
                {
                    byte[] first1 = new byte[] {0x00,
                   0x38, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

                    byte[] first2 = new byte[] {0x00,
                   0x38, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

                    lnpStream.Write(first1);
                    lnpStream.Write(first2);

                }
            }
            catch (System.Exception)
            {
                throw;
            }

            //1 time code
        }

        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Custom1;

        public void InitializeLayout()
        {
            InitializeLed(LedId.Custom1, new Rectangle(0, 0, 10, 10));
        }


        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            Led led = ledsToUpdate.FirstOrDefault(x => x.Color.A > 0);
            if (led == null) return;
            try
                    {
                        byte[] first1 = new byte[] {0x00,
                   0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

                        byte[] first2 = new byte[] {0x00,
                   0x34, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

                        lnpStream.Write(first1);
                        lnpStream.Write(first2);


                byte[][] channel = AddChannel(led.Color, 0x00);

                for (int i = 0; i < channel.Length; i++)
                {
                    lnpStream.Write(channel[i]);
                }

                channel = AddChannel(led.Color, 0x01);

                for (int i = 0; i < channel.Length; i++)
                {
                    lnpStream.Write(channel[i]);
                }


                byte[] submit = new byte[] {0x00,
                   0x33, 0xFF, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

                        lnpStream.Write(submit);
                    }
                    catch
                    {
                    }

        }

        /*
         *  Zenairo
Zenairo
3 months ago (edited)
With three HD120 fans on the first port your red packet would look like this:
            byte[] red1 = new byte[] {0x00,
                   0x32, 0x00, 0x00, 0x24, 0x00, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R,
                   c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c1.R, c2.R, c2.R, c2.R,
                   c2.R, c2.R, c2.R, c2.R, c2.R, c2.R, c2.R, c2.R, c2.R, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

32 is a lighting change packet, unsure really
00 means devices on channel 1 (of 2)
00 means starting at the first light
24 means going through light 36 (24 in hex is 36 lights because you have 3 HD120 fans with 12 led each)
00 means red (01 is green, 02 is blue)
the rest of the values are the red value for lights 1-36
*/

        internal static byte[][] AddChannel(Color c, byte ch = 0x00) //byte 3
        {
            //0x00, lighting change, channel, 
            byte[] red1 = new byte[] {0x00,
                   0x32, ch, 0x00, 0x30, 0x00, c.R, c.R, c.R, c.R, c.R, //5
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //15
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //25
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //35
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //45
                   c.R, c.R, c.R, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //48
                   0x00, 0x00, 0x00, 0x00};

            byte[] green1 = new byte[] {0x00,
                   0x32, ch, 0x00, 0x30, 0x01, c.G, c.G, c.G, c.G, c.G, //5
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //15
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //25
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //35
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //45
                   c.G, c.G, c.G, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //48
                   0x00, 0x00, 0x00, 0x00};

            byte[] blue1 = new byte[] {0x00,
                   0x32, ch, 0x00, 0x30, 0x02, c.B, c.B, c.B, c.B, c.B, //5
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //15
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //25
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //35
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //45
                   c.B, c.B, c.B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //48
                   0x00, 0x00, 0x00, 0x00};
            
            byte[] red2 = new byte[] {0x00,
                   0x32, ch, 0x30, 0x30, 0x00, c.R, c.R, c.R, c.R, c.R, //5
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //15
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //25
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //35
                   c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, c.R, //45
                   c.R, c.R, c.R, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //48
                   0x00, 0x00, 0x00, 0x00};

            byte[] green2 = new byte[] {0x00,
                   0x32, ch, 0x30, 0x30, 0x01, c.G, c.G, c.G, c.G, c.G, //5
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //15
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //25
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //35
                   c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, c.G, //45
                   c.G, c.G, c.G, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //48
                   0x00, 0x00, 0x00, 0x00};

            byte[] blue2 = new byte[] {0x00,
                   0x32, ch, 0x30, 0x30, 0x02, c.B, c.B, c.B, c.B, c.B, //5
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //15
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //25
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //35
                   c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, c.B, //45
                   c.B, c.B, c.B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //48
                   0x00, 0x00, 0x00, 0x00};

            return new byte[6][] { red1, green1, blue1, red2, green2, blue2 };
        }
    }
}

