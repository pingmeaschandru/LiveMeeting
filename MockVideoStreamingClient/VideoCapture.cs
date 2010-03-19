using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using TW.Core.DirectX;
using TW.Core.Native;

namespace MockVideoStreamingClient
{
    internal class VideoCapture : ISampleGrabberCB, IDisposable
    {
        private IFilterGraph2 graphBuilder;
        private IMediaControl mediaCtrl;
        private ManualResetEvent pictureReady;
        private volatile bool gotOne;
        private bool running;
        private IntPtr handle = IntPtr.Zero;

        public VideoCapture(int iDeviceNum, int iFrameRate, int iWidth, int iHeight)
        {
            var capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (iDeviceNum + 1 > capDevices.Length)
                throw new Exception("No video capture devices found at that index!");

            try
            {
                SetupGraph(capDevices[iDeviceNum], iFrameRate, iWidth, iHeight);
                pictureReady = new ManualResetEvent(false);
                gotOne = true;
                running = false;
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public VideoCapture()
            : this(0, 0, 0, 0)
        {
        }

        public VideoCapture(int iDeviceNum)
            : this(iDeviceNum, 0, 0, 0)
        {
        }

        public VideoCapture(int iDeviceNum, int iFrameRate)
            : this(iDeviceNum, iFrameRate, 0, 0)
        {
        }

        public void Dispose()
        {
            CloseInterfaces();
            if (pictureReady == null) return;
            pictureReady.Close();
            pictureReady = null;
        }

        ~VideoCapture()
        {
            Dispose();
        }

        public IntPtr GetBitMap()
        {
            handle = Marshal.AllocCoTaskMem(Stride * Height);
            try
            {
                pictureReady.Reset();
                gotOne = false;
                Start();
                if (!pictureReady.WaitOne(5000, false))
                    throw new Exception("Timeout waiting to get picture");
            }
            catch
            {
                Marshal.FreeCoTaskMem(handle);
                throw;
            }
            return handle;
        }

        public void Start()
        {
            if (running) return;
            var hr = mediaCtrl.Run();
            DsError.ThrowExceptionForHR(hr);

            running = true;
        }

        public void Pause()
        {
            if (!running) return;
            var hr = mediaCtrl.Pause();
            DsError.ThrowExceptionForHR(hr);

            running = false;
        }

        private void SetupGraph(DsDevice dev, int iFrameRate, int iWidth, int iHeight)
        {
            ISampleGrabber sampGrabber = null;
            IBaseFilter capFilter = null;
            ICaptureGraphBuilder2 capGraph = null;

            graphBuilder = (IFilterGraph2)new FilterGraph();
            mediaCtrl = graphBuilder as IMediaControl;
            try
            {
                capGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                sampGrabber = (ISampleGrabber)new SampleGrabber();
                var hr = capGraph.SetFiltergraph(graphBuilder);
                DsError.ThrowExceptionForHR(hr);
                hr = graphBuilder.AddSourceFilterForMoniker(dev.Moniker, null, "Video Capture Device", out capFilter);
                DsError.ThrowExceptionForHR(hr);
                var baseGrabFlt = (IBaseFilter)sampGrabber;
                ConfigureSampleGrabber(sampGrabber);
                hr = graphBuilder.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                DsError.ThrowExceptionForHR(hr);
                if (iFrameRate + iHeight + iWidth > 0)
                    SetConfigParms(capGraph, capFilter, iFrameRate, iWidth, iHeight);

                hr = capGraph.RenderStream(PinCategory.Capture, MediaType.Video, capFilter, null, baseGrabFlt);
                DsError.ThrowExceptionForHR(hr);
                SaveSizeInfo(sampGrabber);
            }
            finally
            {
                if (capFilter != null)
                    Marshal.ReleaseComObject(capFilter);

                if (sampGrabber != null)
                    Marshal.ReleaseComObject(sampGrabber);

                if (capGraph != null)
                    Marshal.ReleaseComObject(capGraph);
            }
        }

        private void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            var media = new AMMediaType();
            var hr = sampGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
                throw new NotSupportedException("Unknown Grabber Media Format");

            var videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            Width = videoInfoHeader.BmiHeader.Width;
            Height = videoInfoHeader.BmiHeader.Height;
            Stride = Width * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
        }

        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            var media = new AMMediaType
                            {
                                majorType = MediaType.Video,
                                subType = MediaSubType.RGB24,
                                formatType = FormatType.VideoInfo
                            };
            var hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);

            hr = sampGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }

        private void SetConfigParms(ICaptureGraphBuilder2 capGraph, IBaseFilter capFilter, int iFrameRate, int iWidth, int iHeight)
        {
            object o;
            AMMediaType media;

            capGraph.FindInterface(PinCategory.Capture, MediaType.Video, capFilter, typeof(IAMStreamConfig).GUID, out o);
            var videoStreamConfig = o as IAMStreamConfig;
            if (videoStreamConfig == null)
                throw new Exception("Failed to get IAMStreamConfig");

            var hr = videoStreamConfig.GetFormat(out media);
            DsError.ThrowExceptionForHR(hr);
            var v = new VideoInfoHeader();
            Marshal.PtrToStructure(media.formatPtr, v);
            if (iFrameRate > 0)
                v.AvgTimePerFrame = 10000000/iFrameRate;

            if (iWidth > 0)
                v.BmiHeader.Width = iWidth;

            if (iHeight > 0)
                v.BmiHeader.Height = iHeight;

            Marshal.StructureToPtr(v, media.formatPtr, false);
            hr = videoStreamConfig.SetFormat(media);
            DsError.ThrowExceptionForHR(hr);
            DsUtils.FreeAMMediaType(media);
        }

        private void CloseInterfaces()
        {
            try
            {
                if (mediaCtrl != null)
                {
                    mediaCtrl.Stop();
                    running = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (graphBuilder == null) return;
            Marshal.ReleaseComObject(graphBuilder);
            graphBuilder = null;
        }

        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            if (!gotOne)
            {
                gotOne = true;
                IntPtr pBuffer;

                pSample.GetPointer(out pBuffer);
                pSample.GetSize();

                if (pSample.GetSize() > Stride*Height)
                    throw new Exception("Buffer is wrong size");

                Kernel32.CopyMemory(handle, pBuffer, Stride * Height);
                pictureReady.Set();
            }

            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            if (!gotOne)
            {
                if (BufferLen <= Stride*Height)
                    Kernel32.CopyMemory(handle, pBuffer, Stride*Height);
                else
                    throw new Exception("Buffer is wrong size");

                gotOne = true;
                pictureReady.Set();
            }
            else
            {
                Dropped++;
            }
            return 0;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Stride { get; private set; }
        public int Dropped { get; set; }
    }
}