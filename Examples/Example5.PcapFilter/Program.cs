using System;
using System.Net.Sockets;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace Example5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Print SharpPcap version
            string ver = SharpPcap.Version.VersionString;
            Console.WriteLine("SharpPcap {0}, Example5.PcapFilter.cs\n", ver);

            // Retrieve the device list
            var devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            Console.WriteLine("The following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            int i = 0;

            // Scan the list printing every entry
            foreach (var dev in devices)
            {
                Console.WriteLine("{0}) {1}", i, dev.Description);
                i++;
            }

            Console.WriteLine();
            Console.Write("-- Please choose a device to capture: ");
            i = int.Parse(Console.ReadLine());

            var device = devices[i];

            //Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

            //Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            // tcpdump filter to capture only TCP/IP packets
            string filter = "ip and tcp";
            device.Filter = filter;

            Console.WriteLine();
            Console.WriteLine
                ("-- The following tcpdump filter will be applied: \"{0}\"",
                filter);
            Console.WriteLine
                ("-- Listening on {0}, hit 'Ctrl-C' to exit...",
                device.Description);

            // Start capture packets
            device.Capture();

            // Close the pcap device
            // (Note: this line will never be called since
            //  we're capturing infinite number of packets
            device.Close();
        }

        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var time = e.Packet.Timeval.Date;
            var len = e.Packet.Data.Length;
            var aa = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            if (aa.PayloadPacket is IPv4Packet iPPacket)
            {
                var header = new PcapHeader((uint)e.Packet.Timeval.Seconds, (uint)e.Packet.Timeval.MicroSeconds,
                                       (uint)e.Packet.Data.Length, (uint)e.Packet.Data.Length);
                if (iPPacket.Protocol == PacketDotNet.ProtocolType.Tcp)
                {
                    var tcpPacket = (iPPacket.PayloadPacket as TcpPacket).PayloadData;
                    for (int i = 0; i < tcpPacket.Length; i++)
                    {
                        if (tcpPacket[i] > 31 && tcpPacket[i] < 127)
                            Console.Write(System.Text.Encoding.UTF8.GetString(BitConverter.GetBytes(tcpPacket[i])));
                    }
                }
                Console.WriteLine("\r\n");
            }
        }
    }
}
