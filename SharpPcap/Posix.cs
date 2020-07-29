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
 * Copyright 2020 Ayoub Kaanich <kayoub5@live.com>
 */

using System;
using System.Runtime.InteropServices;

namespace SharpPcap
{
	/// 可移植操作系统接口 Portable Operating System Interface
	/// <summary>
	/// Posix code in here may seem complex but it serves an important purpose.
	/// 这里的Posix代码可能看起来很复杂，但它有一个重要的用途。
	///
	/// Under Unix, pcap_loop(), pcap_dispatch(), pcap_next() and pcap_next_ex()
	/// all perform blocking read() calls at the os level that have NO timeout.
	/// If the user wishes to stop capturing on an adapter they will need to wait
	/// until the next packet arrives for the capture loop to wake up and look to see
	/// if it has been asked to shut down. This may be never in the case of inactive
	/// adapters or far longer than what the user desires.
	/// 在Unix下，pcap_loop（），pcap_dispatch（），pcap_next（）和pcap_next_ex（）
	/// 都在操作系统级别执行阻塞超时的read（）调用。如果用户希望停止捕获适配器，
	/// 则需要等待直到下一个数据包到达，捕获循环才能唤醒并查看是否已要求关闭。
	/// 在非活动状态下可能永远不会适配器或比用户期望的更长的时间。
	///
	/// So, to avoid the issue we invoke the poll() system call. 
	/// The thread sleeps on the poll() and only when woken
	/// up and indicating that data is available do we call one of the pcap
	/// data retrieval routines. This is how we avoid blocking for long periods
	/// or forever.
	/// 因此，为避免该问题，我们调用poll（）系统调用。线程仅在唤醒时在poll（）上睡眠并指示数据可用，我们将其中一个称为pcap数据检索例程。 这就是我们避免长时间阻塞的方式或永远。
	///
	/// Poll enables us to set a timeout. The timeout is chosen to be long
	/// enough to avoid a noticable performance impact from frequent looping
	/// but short enough to satisify the timing constraints of the Thread.Join() in
	/// the stop capture code.
	/// 轮询使我们可以设置超时。 选择超时时间较长足以避免频繁循环对性能产生明显影响，但又足够短，足以满足停止捕获代码中Thread.Join（）的时序约束。
	///
	/// </summary>
	internal static class Posix
    {
#pragma warning disable CS0649
		public struct Pollfd
		{
			public int fd;
			public PollEvents events;
			public PollEvents revents;
		}
#pragma warning restore CS0649

		[Flags]
		public enum PollEvents : short
		{
			POLLIN = 0x0001, // There is data to read
			POLLPRI = 0x0002, // There is urgent data to read 有紧急数据要读取
			POLLOUT = 0x0004, // Writing now will not block
			POLLERR = 0x0008, // Error condition
			POLLHUP = 0x0010, // Hung up 中断
			POLLNVAL = 0x0020, // Invalid request; fd not open
		}

		[DllImport("libc", SetLastError = true, EntryPoint = "poll")]
		internal static extern int Poll([In, Out] Pollfd[] ufds, uint nfds, int timeout);
	}


}
