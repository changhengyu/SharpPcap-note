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
 * Copyright 2010 Chris Morgan <chmorgan@gmail.com>
 */


namespace SharpPcap.Npcap
{
    /// <summary>
    /// 特定于Npcap的发送队列实现所允许的传输模式类型
    /// The types of transmit modes allowed by the Npcap specific send queue implementation
    /// </summary>
    public enum SendQueueTransmitModes
    {
        /// <summary>
        /// Packets are sent as fast as possible
        /// 数据包以最快的速度发送
        /// </summary>
        Normal,

        /// <summary>
        /// Packets are synchronized in the kernel with a high precision timestamp
        /// 数据包在内核中以高精度的时间戳进行同步
        /// </summary>
        Synchronized
    }
}
