using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OS
{
    using System;
    using System.Threading;

    class Program
    {
        private static Mutex mutex = new Mutex();
        private static AutoResetEvent DetectFound = new AutoResetEvent(false);
        private static AutoResetEvent DrillStart = new AutoResetEvent(false);
        private static AutoResetEvent DrillFinish = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            var detectThread = new Thread(Detector) { Name = "Detecror" };
            var drillThread = new Thread(Driller) { Name = "Driller" };
            var cleanThread = new Thread(Cleaner) { Name = "Cleaner" };

            
            
                detectThread.Start();
                drillThread.Start();
                cleanThread.Start();
            Console.WriteLine("===========================");



            return;
            
  
        }

            private static void Detector()
            {
                Console.WriteLine("Detector:\t Detecting...");
              
                Random rnd = new Random();
                int mineral = rnd.Next(0, 3);
                while (mineral == 0)
                {
                    Console.WriteLine("Detector:\t No minerals found!");
                    
                    Console.WriteLine("Detector:\t Detecting...");


                mineral = rnd.Next(0, 3);
                }

                Console.WriteLine("Detector:\t {0} minerals found!", mineral);

            DetectFound.Set();
            
            }

                private static void Driller()
                {


            Console.WriteLine("Driller():\t Waiting for Detector...");
            DetectFound.WaitOne();
                mutex.WaitOne();
                Console.WriteLine("Driller():\t Drilling...");
            DrillStart.Set();
            
                Console.WriteLine("Driller():\t Drilling Complete!");
            DrillFinish.Set();
                
                mutex.ReleaseMutex();
            
                }

                private static void Cleaner()
                {


            DrillStart.WaitOne();
           
            Console.WriteLine("Cleaner():\t Waiting for Driller...");
            
            DrillFinish.WaitOne();
            
                Console.WriteLine("Cleaner():\t Cleaning...");
            
            
            
                }
            }
        }

    

