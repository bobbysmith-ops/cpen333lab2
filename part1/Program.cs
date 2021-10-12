using System;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace MergeSort
{
    class Program
    {
        
        static void Main(string[] args)
        {

            int ARRAY_SIZE = 10000000;
            int[] arraySingleThread = new int[ARRAY_SIZE]; //' array declaration
            int[] arrayMultiThread = new int[ARRAY_SIZE];//'declare multithread array

            Stopwatch stopwatch_multi = new Stopwatch();//'instantiate the stopwatch class/create stopwatch object
            Stopwatch stopwatch_single = new Stopwatch();


            // TODO : Use the "Random" class in a for loop to initialize an array

            var rand = new Random(); //'instantiate Random class/construct Random object (Random is a random number generator)

            for (int z = 0; z < ARRAY_SIZE;  z++ )
            {
                arraySingleThread[z] = rand.Next(0, 100); //' use next method from the rand object to initialize array element with a random integer btwn 0 and 100
            }


            ////////////start stopwatch for multithread merge sort/////
            stopwatch_multi.Start();


            // copy array by value.. You can also use array.copy()
            //int[] arrayMultiThread = arraySingleThread.Slice(0,arraySingleThread.Length);
            Array.Copy(arraySingleThread, arrayMultiThread, ARRAY_SIZE); //'slice is bad choice, just use Copy method from the Array class, copy contents of arraysinglethread to arraymultithread


            //'method to divide unsorted array into sub arrays based on value of numofthreads

            int numofthreads = 8;

            //'number of subarrays to make is just the num of threads i have
            int numofsubarrays = numofthreads;

            //'subarray size is array size divided by the number of threads i have
            int subarraysize = ARRAY_SIZE / numofsubarrays;


            //'declare jagged array (subarrays)
            int[][] subarrays = new int[numofsubarrays][];






            //'//////////////////divide main array contents into jagged array using copy function//////////////////////



            //'declare an array of threads using our numofthreads variable
            Thread[] thread = new Thread[numofthreads];


            //'for Array.Copy note 1st and 3rd args are arrays. 3rd arg isn't an index its an array, bc recall temparray is a jagged array. The 3rd arg is pointing to the subarray located at temparray[i]
            //'j is the index in arrayMultiThread where the copying will start from

            //int i = 0;
            //int j = 0;
            //while (i < numofsubarrays)
            //{
            //    Array.Copy(arrayMultiThread, j, temparray[i], 0, subarraysize);//'copy a portion (of size subarraysize) of the big array to a subarray in the jagged array
            //    thread[i] = new Thread(() => MergeSort(temparray[i]));//'initialize an element of my array of threads with a new thread that will run Mergesort on the subarray I just copied
            //    thread[i].Start();//'start the thread at that index
            //    j = j + subarraysize;
            //    i++;
            //}



            //'divide the large array into subarrays
            for (int i = 0; i < numofthreads; i++)
            {
                subarrays[i] = new int[subarraysize];
                Array.Copy(arrayMultiThread, i * subarraysize, subarrays[i], 0, subarraysize);

            }

            //'test division into subarrays
            //PrintArray(arrayMultiThread);

            Console.WriteLine();


            for (int i = 0; i < numofsubarrays; i++)
            {
               // PrintArray(subarrays[i]);
            }

            Console.WriteLine();


            //'initialize an element of my array of threads with a new thread that will run Mergesort on one of the subarrays that I just filled w/ some of the unsorted values from arrayMultiThread. Use for loop to do this to all of the subarrays
            //'have to be careful with indexing variable here
            for (int i = 0; i < numofthreads; i++)
            {
                int m = i;
                thread[m] = new Thread(() => MergeSort(subarrays[m]));
                thread[m].Start();//'start the thread
            }


            //'for loop to join all the threads
            for (int i = 0; i < numofthreads; i++)
            {
                thread[i].Join();//'join the thread
            }



            //'TEST that subarrays are indeed sorted
            for (int i = 0; i < numofsubarrays; i++)
            {
               // PrintArray(subarrays[i]);
            }

            Console.WriteLine();


            //'/////////////combine sorted subarrays back into one array

            int[] combinedsubarrays = new int[ARRAY_SIZE];

            //'//////////////try using lists/////////////////

            var list = new List<int>();



            for (int i = 0; i < numofsubarrays; i++)
            {
                list.AddRange(subarrays[i]);//'AddRange appends the elements from subarrays[i] to the end of the list
            }


            combinedsubarrays = list.ToArray(); //'changes list to an array

            Array.Sort(combinedsubarrays);

            ///////////////stop stopwatch for multithread merge sort/////////////////
            stopwatch_multi.Stop();
            TimeSpan multi_ts = stopwatch_multi.Elapsed;

            string multilapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            multi_ts.Hours, multi_ts.Minutes, multi_ts.Seconds,
            multi_ts.Milliseconds / 10);
            Console.WriteLine("Multithreading mergesort RunTime = " + multilapsedTime);



            //'test that sorted subarrays were properly combined
          //  PrintArray(combinedsubarrays);


            

            Console.WriteLine();



            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */
            //TODO :start the stopwatch
            stopwatch_single.Start();

            MergeSort(arraySingleThread);

            stopwatch_single.Stop();
            TimeSpan single_ts = stopwatch_single.Elapsed;
            long single_ticks = 0;
            single_ticks = stopwatch_single.ElapsedTicks;
            //TODO :Stop the stopwatch

           // PrintArray(arraySingleThread);

            long multi_ticks = 0;
            multi_ticks = stopwatch_multi.ElapsedTicks;


            Console.Write("is sorted = ");
            Console.WriteLine(IsSorted(arraySingleThread));

            string singleelapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            single_ts.Hours, single_ts.Minutes, single_ts.Seconds,
            single_ts.Milliseconds / 10);

            Console.WriteLine("Singlethreaded Mergesort RunTime = " + singleelapsedTime);
            Console.WriteLine($"Singlethreaded Mergesort RunTime in ticks =  {single_ticks}");


            Console.WriteLine("Multithreading mergesort RunTime = " + multilapsedTime);
            Console.WriteLine($"Multithreaded Mergesort RunTime in ticks =  {multi_ticks}");




            /*********************** Methods **********************
             *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */

            static void Merge(int[] LA, int[] RA, int[] A) //'this will be called inside MergeSort parent function. Puts left and right array values back into parent array in ascending order
            {

                int nL = LA.Length;
                int nR = RA.Length;
                int i = 0; int j = 0; int k = 0;

                while (i < nL && j < nR)
                {
                    if (LA[i] <= RA[j])//'if value at that index is less/equal in left array than in right array, main array at that index gets the left array value
                    {
                        A[k] = LA[i];
                        i++;
                        k++;
                    }
                    else
                    {
                        A[k] = RA[j];
                        j++;
                        k++;
                    }

                }
                    //'check for leftovers in LA or RA, only 1 of these while loops will execute bc only one array could have leftovers

                    while (i < nL)
                    {
                        A[k] = LA[i];
                        i++;
                        k++;
                    }

                    while (j < nR)
                    {
                        A[k] = RA[j];
                        j++;
                        k++;
                    }

                return;

            }


            




             /*
             implement MergeSort method: takes an integer array by reference
             and makes some recursive calls to intself and then sorts the array
             */

        
            static void MergeSort(int[] A) //'mergesort contains the code that breaks down arrays into smaller and smaller pieces, and it calls the merge function to put them back together
            {
                

                int n = A.Length; //'n stores length of array

                if (n < 2)//'base case
                {
                    return; 
                }
                

                int midIndex = n / 2;


                int[] leftHalf = new int[midIndex]; //'declare left half and right half sub arrays
                int[] rightHalf = new int[n - midIndex];

                for (int i = 0; i < midIndex; i++)//'fill leftHalf sub array with left half of A
                {
                    leftHalf[i] = A[i];
                }

                for (int i = midIndex; i < n; i++)//'fill rightHalf sub array with right half of A
                {
                    rightHalf[i - midIndex] = A[i];//'use i-midIndex here bc need need rightHalf indexing to start at 0
                }


                MergeSort(leftHalf);
                MergeSort(rightHalf);
                Merge(leftHalf, rightHalf, A); //'call the function to put the left and right subarrays back into the parent array in ascending order


            }







        

            //'helper functions for testing

            // a helper function to print your array
            static void PrintArray(int[] myArray)
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();

            }

            // a helper function to confirm your array is sorted
            // returns boolean True if the array is sorted
            static bool IsSorted(int[] a)
            {
                int j = a.Length - 1;
                if (j < 1) return true;
                int ai = a[0], i = 1;
                while (i <= j && ai <= (ai = a[i])) i++;
                return i > j;
            }


        }


    }
}
