using System;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;



namespace MultiThreadPi
{
    class MainClass
    {
        static void Main(string[] args)
        {
            long numberOfSamples = 1000; //'aka number of raindrops that will land in the square (aka all the raindrops)
            long hits=0;//' aka number of raindrops that land in the circle

            // Random rnd = new Random();//'construct Random object

            //'////////////////////////////////////////ADDING THREADING STUFF//////////////////////////////////

            int numofthreads = 6;

            long sampsperthread = numberOfSamples / numofthreads;

            Thread[] thread = new Thread[numofthreads];

            //'create new function that encapsulates the sample generation and the for loop i use for the checking, can i allow this function to alter hits (pass it by reference) and make it a void function
            //'or do it make it return the number of hits, not sure how that works with threads

























            double[,] samplearray = GenerateSamples(numberOfSamples);//'call GenerateSamples to get a 2D array of (x,y) values



            //'now do the math to calculate how many of these are hits, will need to use a for loop
            //'calculate distance btwn (x,y) from the origin

            for(int i = 0; i < numberOfSamples; i++)
            {
                double randx = samplearray[i, 0];//'extract an x and y value from the 2D array
                double randy = samplearray[i, 1];

                double origin_dist = randx * randx + randy * randy; //'calculate x squared plus y squared to get hypotenuse squared, we know if its less than 1 then point is within unit circle

                if (origin_dist <= 1)//'if hypotenuse squared is less than 1, point is w/in circle, so increment hits
                    hits++;
                
            }



            //'now run the EstimatePI function using the number of hits we've determined
            double pi = EstimatePI(numberOfSamples, ref hits);


            //'print our estimated PI value

            Console.WriteLine($"the estimated pi value is {pi}");










            static double EstimatePI(long numberOfSamples, ref long hits)
            {
                double PIestimate;
                double doublehits = hits;//'avoid integer division/truncation, store our integers in double variables
                double doublesamples = numberOfSamples;
                PIestimate = (4.0*(hits)) / numberOfSamples;//'needed to put 4.0 instead of 4
                return PIestimate;
            }


             

            //'2D array, aka an array of arrays, columns are all the same size
            static double[,] GenerateSamples(long numberOfSamples)
            {
                double[,] samplearray = new double[numberOfSamples, 2];//'declare 2D array of numberOfSamples rows and 2 columns
                for (int i = 0; i < numberOfSamples; i++)
                {
                    double x_rand = Randomvalue_Generator();
                    double y_rand = Randomvalue_Generator();
                    samplearray[i, 0] = x_rand;
                    samplearray[i, 1] = y_rand;

                }

                return samplearray;

            }




            static double Randomvalue_Generator()
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());//'construct Random object, make sure different seed being produced each time
                double rando_value = (rnd.NextDouble() * 2) - 1; //'invoke NextDouble method from my rnd object, do a bit of math so im getting values btwn [-1,1]
                return rando_value;
            }


        


        }
    }
}
