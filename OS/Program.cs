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
        private static ManualResetEvent DetectFound = new ManualResetEvent(false);
        private static ManualResetEvent DrillStart = new ManualResetEvent(false);
        private static ManualResetEvent DrillFinish = new ManualResetEvent(true);
        private static ManualResetEvent CleanerReady = new ManualResetEvent(false);
        

       
        static void Main(string[] args)
        {
            var detectThread = new Thread(Detector) { Name = "Detector" };
            var drillThread = new Thread(Driller) { Name = "Driller" };
            var cleanThread = new Thread(Cleaner) { Name = "Cleaner" };

                detectThread.Start();
                drillThread.Start();
                cleanThread.Start();

                return;
        }

        private static void Detector()
        {

            while (true)
            {
                DrillFinish.WaitOne();     
                mutex.WaitOne();          //checks if allowed to run 

                
                Console.WriteLine("Detector():\t Detecting...");

                Random rnd = new Random();                                  //RNG
                int mineral = rnd.Next(0, 3);

                while (mineral == 0)
                {
                    Console.WriteLine("Detector():\t No minerals found! Restarting Detection system...");

                    Console.WriteLine("Detector():\t Detecting...");


                    mineral = rnd.Next(0, 3);
                }

                Console.WriteLine("Detector():\t {0} minerals found!", mineral);  //displays number of materials found

                DetectFound.Set();                                              //signals DetectFound event to run
                DrillFinish.Reset();
                mutex.ReleaseMutex();                                           //releases to allow other threads to run

            }
            
           
            
        }

        private static void Driller()
        {

            while (true)
            {



                Console.WriteLine("Driller():\t Waiting for Detector...");
                DetectFound.WaitOne();                                          //checks if materials are found
                mutex.WaitOne();                                                //checks if allowed to run

                

                Console.WriteLine("Driller():\t Drilling...");
                DrillStart.Set();                                               //signals Cleaner() drilling has started
                mutex.ReleaseMutex();
                CleanerReady.WaitOne();                                        //checks if Cleaner is Ready
                mutex.WaitOne();

                Console.WriteLine("Driller():\t Drilling Complete!");
                DrillFinish.Set();                                              //signals Cleaner() drilling has completed
                DetectFound.Reset();                                            //resets events
                CleanerReady.Reset();
                mutex.ReleaseMutex();
            }
                
            
            
            
        }
        private static void Cleaner()
        {

            while (true)
            {

                {


                    DrillStart.WaitOne();                                       //checks if drilling has started
                    mutex.WaitOne();
                    Console.WriteLine("Cleaner():\t Waiting for Driller...");
                    mutex.ReleaseMutex();
                    CleanerReady.Set();                                        //signals Driller() Cleaner is Ready
                    DrillFinish.WaitOne();                                      //checks if drilling is completed
                    mutex.WaitOne();

                    Console.WriteLine("Cleaner():\t Cleaning...");
                    
                    Console.WriteLine("Cleaner():\t Cleaning Complete!");
                    DrillFinish.Reset();
                    DrillStart.Reset();
                    mutex.ReleaseMutex();




                }
            }

        }
        
    }
}

    

