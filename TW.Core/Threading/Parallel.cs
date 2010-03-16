using System;
using System.Threading;

namespace TW.Core.Threading
{
    public sealed class Parallel
    {
        public delegate void ForLoopBody( int index );

        private static int threadsCount = Environment.ProcessorCount;
        private static readonly object sync = new Object( );
        private static volatile Parallel instance;

        private Thread[] threads;
        private AutoResetEvent[] jobAvailable;
        private ManualResetEvent[] threadIdle;
        private int currentIndex;
        private int stopIndex;
        private ForLoopBody loopBody;

        public static int ThreadsCount
        {
            get { return threadsCount; }
            set
            {
                lock ( sync )
                {
                    threadsCount = Math.Max( 1, value );
                }
            }
        }

        public static void For( int start, int stop, ForLoopBody loopBody  )
        {
            lock ( sync )
            {
                var parallelInstance = Instance;

                parallelInstance.currentIndex   = start - 1;
                parallelInstance.stopIndex      = stop;
                parallelInstance.loopBody       = loopBody;

                for ( var i = 0; i < threadsCount; i++ )
                {
                    parallelInstance.threadIdle[i].Reset( );
                    parallelInstance.jobAvailable[i].Set( );
                }

                for ( var i = 0; i < threadsCount; i++ )
                {
                    parallelInstance.threadIdle[i].WaitOne( );
                }
            }
        }

        private Parallel( ) { }

        private static Parallel Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new Parallel( );
                    instance.Initialize( );
                }
                else
                {
                    if ( instance.threads.Length != threadsCount )
                    {
                        instance.Terminate( );
                        instance.Initialize( );
                    }
                }
                return instance;
            }
        }

        private void Initialize( )
        {
            jobAvailable = new AutoResetEvent[threadsCount];
            threadIdle = new ManualResetEvent[threadsCount];
            threads = new Thread[threadsCount];

            for ( var i = 0; i < threadsCount; i++ )
            {
                jobAvailable[i] = new AutoResetEvent( false );
                threadIdle[i]   = new ManualResetEvent( true );

                threads[i] = new Thread( WorkerThread ) {IsBackground = true};
                threads[i].Start( i );
            }
        }

        private void Terminate( )
        {
            loopBody = null;
            for ( int i = 0, count = threads.Length ; i < count; i++ )
            {
                jobAvailable[i].Set( );
                threads[i].Join( );

                jobAvailable[i].Close( );
                threadIdle[i].Close( );
            }

            jobAvailable    = null;
            threadIdle      = null;
            threads         = null;
        }

        private void WorkerThread( object index )
        {
            var threadIndex = (int) index;

            while ( true )
            {
                jobAvailable[threadIndex].WaitOne( );
                if ( loopBody == null )
                    break;

                while ( true )
                {
                    var localIndex = Interlocked.Increment( ref currentIndex );
                    if ( localIndex >= stopIndex )
                        break;

                    loopBody( localIndex );
                }
                threadIdle[threadIndex].Set( );
            }
        }
    }
}