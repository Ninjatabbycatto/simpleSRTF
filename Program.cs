namespace simpleSRTF;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Job {
    public int id, aTime, bTime, ibTime, rbTime;
    public bool waiting = false;
    public bool running = false;
    public bool done = false;
    public Job (int id, int aTime, int bTime, int ibTime, int rbTime) {
        this.id = id;
        this.aTime = aTime;
        this.bTime = bTime;
        this.ibTime = bTime;
        this.rbTime = bTime;
    }
}

public class Program
{

    static void printHeader(Job []jobs) {
        Console.WriteLine("SRTF simulation");
        Console.WriteLine("x = running");
        Console.WriteLine("- = idle");
        Console.WriteLine("~ = waiting");
        Console.WriteLine("= = finished");
        Console.WriteLine("   Job ID    |    Arrival Time   |   Burst Time   ");
        Console.WriteLine("-----------------------------------------------");
        foreach (Job job in jobs) {
            Console.WriteLine("      " + job.id + "      |      " +  "   " + job.aTime + "         |      " + job.bTime  );
        }
    }
    public static void DequeueSpecificValue(int value) {
        Queue<int> tempQueue = new Queue<int>();

        while (queue.Count > 0)
        {
            int dequeuedElement = queue.Dequeue();
            if (dequeuedElement != value)
            {
                tempQueue.Enqueue(dequeuedElement);
            }
            else
            {
            
            }
        }

        // Replace the original queue with the temporary queue
        queue = tempQueue;
    }



    private static readonly object consoleLock = new object();

    public static Queue<int> queue = new Queue<int>();
    private static Job[] jobs;
    private static int injob = 8;
    public static bool hasprocess = false;


    public static void runLowestib(int timer) {
            
            

            foreach (Job job in jobs) {
                int lowest_iB = 10;
                foreach (int pos in queue) {
                    if (jobs[pos].ibTime < lowest_iB && !jobs[pos].done) lowest_iB = jobs[pos].ibTime;
                }
                if (job.id == injob && job.ibTime == lowest_iB){ //kung ung current running job ay ung lowest LB
                    DequeueSpecificValue(job.id); 
                    job.running = true;
                    job.waiting = false;
                }
                else if (job.waiting && job.id != injob && job.ibTime == lowest_iB){ //kung  ung job waiting tas may lowest initial burst time
                    DequeueSpecificValue(job.id);       //dequeue
                    job.running = true;             //paandarin
                    job.waiting = false;
                }
                
                
                else {
                    job.running = false;             //paandarin
                    job.waiting = true;
                }
                
                
            }

    }


    static async Task printSimulation(int time, Job job) {
        int timer = 0;
        Console.SetCursorPosition(0, job.id + 13);
        Console.Write(job.id + "   ");

        do {
            lock (consoleLock)
            {
                //if (hasprocess) {                   //kung may process
                //    if (injob == job.id) {          //if ito current job
                //        runLowestib(timer);     
                //    }
                //    else if (injob != job.id) {     //if this isnt the current job
                //        if (queue.Count > 0) {      //kung my queue
                //            queue.Enqueue(job.id);  
                //            job.waiting = true;     //wating default
                //            job.running = false;    
                //        }
                //        else if (queue.Count > 0) { 
                //            queue.Enqueue(job.id);
                //        }
                //        
                //    }
                //    else if (jobs[injob].aTime == job.aTime ) { //if current job has = arival time
                //        if (jobs[injob].ibTime > job.ibTime) { //if running job has greater initial burst time..
                //            job.running = true;
                //            jobs[injob].running = false;
                //            queue.Enqueue(jobs[injob].id);
                //        }
                //        else {
                //            job.running = false;
                //            job.ibTime = 0;
                //            jobs[injob].running = true;
                //            queue.Enqueue(job.id);
                //        }
//
                //    }
//
                //}
                //else if (!hasprocess) {
                //    if(job.aTime == timer) job.running = true;
                //    if (queue.Count > 0) {
                //        runLowestib(timer);
                //    }
                //}
                //if (job.rbTime <= 0) {
                //    job.running = false;
                //    job.done = true;
//
                //}
               
                if (queue.Count > 0) {      //may que na
                    if (hasprocess) {       //tas may pinprocess
                        if (job.aTime == timer ) {  //tas ung job arrival time is now
                            queue.Enqueue(job.id);
                            job.running = false;
                            job.waiting = true;
                            runLowestib(timer);
                        }
                    }
                    else { 
                        if (job.aTime == timer && !hasprocess) {
                            hasprocess=true;
                            queue.Enqueue(job.id);
                            runLowestib(timer);
                        }
                        else if (job.done) {
                            runLowestib(timer);
                        }
                    }
                }
                else {
                    if (job.aTime == timer) {
                        hasprocess=true;
                        queue.Enqueue(job.id);
                        job.waiting = false;
                        job.running = true;
                    }
                    if (job.waiting == true) {
                        queue.Enqueue(job.id);
                    }
                }

                if (job.rbTime <= 0) {
                    job.running = false;
                    job.done = true;
                }



                Console.SetCursorPosition(timer + 3, job.id + 13);
                if (job.waiting) {
                    Console.Write("~");
                    job.ibTime = job.rbTime;
                }
                if (job.running) {
                    Console.Write("x");
                    injob = job.id;
                    hasprocess = true;
                    job.rbTime--;
                }
                if (job.done) {
                    Console.Write("=");
                    DequeueSpecificValue(job.id);
                    hasprocess = false;
                    job.waiting = false;
                    job.running = false;
                }
                else {
                    Console.Write("-");
                }
            
            }
            await Task.Delay(1000);
            timer++;
        }   while (timer < time);
        
    }

    static async Task Main(string[] args)
    {
        //Initialize ui

        //Create Processes
        Random random = new Random();

        int time = 30;  //zero dapat
        //bool isdone= false;

        

        jobs = new Job[] {
                      new Job(0, random.Next(1, 10), random.Next(1,10), 0, 0),
                      new Job(1, random.Next(1, 10), random.Next(1,10), 0, 0),
                      new Job(2, random.Next(1, 10), random.Next(1,10), 0, 0),
                      new Job(3, random.Next(1, 10), random.Next(1,10), 0, 0),
                      new Job(4, random.Next(1, 10), random.Next(1,10), 0, 0)
                     };

        Task[] lines = new Task[5];
        printHeader(jobs);
        foreach (Job job in jobs) {
            lines[job.id] = Task.Run(() => printSimulation(time, job));
        }
        await Task.WhenAll(lines);
        Console.SetCursorPosition(0, 40);

        //Console.Clear();
        //while----------------


        //create processs
    }
}
