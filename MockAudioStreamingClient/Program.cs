using System;
using System.Net;
using System.Runtime.InteropServices;
using TW.Core.Native;
using TW.Core.Sockets;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Messages;
using TW.WaveLib;

namespace MockAudioStreamingClient
{
    public class Program
    {
        public static TcpSocketClient  publisher = new TcpSocketClient();
        
        static void Main()
        {
            publisher.Connect(new IPEndPoint(IPAddress.Loopback, 5000));

            Console.ReadLine();
            
            var recorder = new WaveInRecorder(-1, new Winmm.WaveFormat(44100, 16, 2), 16384, 3, DataArrived);

            Console.ReadLine();

            recorder.Dispose();
        }

        private static void DataArrived(IntPtr data, int size)
        {
            var recBuffer = new byte[size];
            Marshal.Copy(data, recBuffer, 0, size);

            var message = new AudioFrameMessage(AudioFormatType.Wave, recBuffer);
            var dataToSend = new RdapMessage(RdapMessageType.AudioFrameMessage, message.ToBytes()).ToBytes();

            publisher.Send(dataToSend, 0, dataToSend.Length);
        }
    }
}