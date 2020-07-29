/*
This file is part of SharpPcap.

SharpPcap is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharpPcap is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with SharpPcap.  If not, see <http://www.gnu.org/licenses/>.
*/
/* 
 * Copyright 2005 Tamir Gal <tamir@tamirgal.com>
 * Copyright 2008-2009 Chris Morgan <chmorgan@gmail.com>
 * Copyright 2008-2009 Phillip Lemon <lucidcomms@gmail.com>
 */

using System;

namespace SharpPcap
{
    /// <summary>
    /// Constants and static helper methods
    /// 常量和静态辅助方法
    /// </summary>
    public class Pcap
    {
        /// <summary>
        /// Represents the infinite number for packet captures
        /// 表示数据包捕获的无限个数
        /// </summary>
        internal const int InfinitePacketCount = -1;

        /* interface is loopback 
         * 接口是环回
         */
        internal const uint PCAP_IF_LOOPBACK = 0x00000001;
        internal const int MAX_PACKET_SIZE = 65536;
        internal const int PCAP_ERRBUF_SIZE = 256;

        // Constants for address families
        // These are set in a Pcap static initializer because the values
        // differ between Windows and Linux
        internal readonly static int AF_INET;
        internal readonly static int AF_PACKET;
        internal readonly static int AF_INET6;

        // Constants for pcap loop exit status.
        internal const int LOOP_USER_TERMINATED = -2;
        internal const int LOOP_EXIT_WITH_ERROR = -1;
        internal const int LOOP_COUNT_EXHAUSTED = 0;

        /// <summary>
        /// Returns the pcap version string retrieved via a call to pcap_lib_version()
        /// 通过调用pcap lib version（）返回检索的pcap版本字符串
        /// </summary>
        public static string Version
        {
            get
            {
                try
                {
                    return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(LibPcap.LibPcapSafeNativeMethods.pcap_lib_version());
                }
                catch
                {
                    return "Pcap version can't be identified. It is likely that pcap is not installed " +
                        "but you could be using a very old version.";
                }
            }
        }

        private static bool isUnix()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static Pcap()
        {
            // happens to have the same value on Windows and Linux
            // 碰巧在Windows和Linux上有相同的值
            AF_INET = 2;

            // AF_PACKET = 17 on Linux, AF_NETBIOS = 17 on Windows
            // FIXME: need to resolve the discrepency at some point
            AF_PACKET = 17;

            if (isUnix())
            {
                AF_INET6 = 10; // value for linux from socket.h
            }
            else
            {
                AF_INET6 = 23; // value for windows from winsock.h
            }
        }
    }
}
