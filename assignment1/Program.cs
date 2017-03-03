using System;
using Models;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Assignment 1: K-means algorithm");
            var program = new Program();
            var data = program.ReadCSV();
            var datapoints = program.buildData(data);
            Console.WriteLine("Enter number of clusters:");
            var k = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter amount of iterations:");
            var iterations = Int32.Parse(Console.ReadLine());
            var centroids = program.BuildCentroids(k, datapoints);

            program.KMeans(iterations, centroids, datapoints);
        }

        private void KMeans(int iterations, Cluster[] centroids, DataPoint[] datapoints)
        {
            var sw = Stopwatch.StartNew();
            var isMoving = true;
            var currentIter = 0;

            while (isMoving && currentIter < iterations)
            {
                for (var i = 0; i < datapoints.GetLength(0); i++)
                {
                    var currentCluster = 0;
                    var minimum = Math.Pow(100, 100);

                    for (var j = 0; j < centroids.GetLength(0); j++)
                    {
                        var distance = EuclideanDistance(datapoints[i].allOffers, centroids[j].datapoints);

                        if (distance < minimum)
                        {
                            minimum = distance;
                            currentCluster = j;
                        }
                    }

                    datapoints[i].cluster = currentCluster;
                }

                var groupedData = new Dictionary<int, List<int[]>>();
                for (var i = 0; i < centroids.GetLength(0); i++)
                {
                    groupedData.Add(i, new List<int[]>());
                }

                for (var i = 0; i < datapoints.Length; i++)
                {
                    for (var j = 0; j < datapoints[i].allOffers.Length; j ++)
                    {
                        groupedData[datapoints[i].cluster].Add(datapoints[i].allOffers);
                    }
                }

                foreach (var key in groupedData.Keys)
                {
                    var oldCentroid = centroids[key];
                    var newPoints = new float[32];
                    for (var i = 0; i < groupedData[key].Count; i++)
                    {
                        var data = groupedData[key][i];
                        for (var j = 0; j < data.Length; j++)
                        {
                            newPoints[j] += data[j];
                        }
                    }

                    for ( var i = 0; i < newPoints.Length; i++)
                    {
                        float divideBy = groupedData[key].Count;
                        newPoints[i] = newPoints[i] / divideBy;
                    }

                    var newCentroid = new Cluster(key, newPoints);
                    if (IsEqual(oldCentroid.datapoints, newPoints))
                    {
                        isMoving = false;
                    }
                    else 
                    {
                        centroids[key] = newCentroid;
                    }
                }
                currentIter++;
            }
            var end = sw.Elapsed;
            PrintResult(end, datapoints, currentIter);
        }

        private void PrintResult(TimeSpan timer, DataPoint[] datapoints, int iterations)
        {
            for (var i = 0; i < datapoints.Length; i++)
            {
                Console.Write("Client #" + (i + 1) + " bought: " + String.Join("; ", datapoints[i].boughtOffers) + " in cluster: " + datapoints[i].cluster + "\n");
            }
            Console.WriteLine("Elapsed milliseconds: " + timer + " | Elapsed iterations: " + (iterations + 1));
        }

        private Boolean IsEqual(float[] oldPoints, float[] newPoints)
        {
            for (var i = 0; i < oldPoints.Length; i++)
            {
                if (oldPoints[i] != newPoints[i])
                {
                    return false;
                }
                
            }

            return true;
        }

        private double EuclideanDistance(int[] a, float[] b)
        {
            double sum = 0;
            for (var i = 0; i < a.GetLength(0); i++)
            {
                var power = Math.Pow(a[i] - b[i], 2);
                sum += power;
            }

            return Math.Sqrt(sum);
        }

        private Cluster[] BuildCentroids(int k, DataPoint[] datapoints)
        {
            var centroids = new Cluster[k];
            var rnd = new Random();
            for (var i = 0; i < k; i++)
            {
                var randint = rnd.Next(datapoints.Length - 1);
                var randomData = datapoints[randint].allOffers;
                var newPoints = new float[datapoints[0].allOffers.Length];

                for (var j = 0; j < newPoints.Length; j++)
                {
                    newPoints[j] = (float)randomData[j];
                }
                centroids[i] = new Cluster(i, newPoints);
            }
            return centroids;
        }

        private int[,] ReadCSV()
        {
            var data = new int[100, 32];
            var fs = File.OpenRead("data/WineData.csv");
            var reader = new StreamReader(fs);

            var offer = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                for (var i = 0; i < data.GetLength(0); i++)
                {
                    data[i, offer] = Int32.Parse(values[i]);
                }
                offer++;
            }

            reader.Dispose();
            return data;
        }

        //build datapoints from CSV file
        private DataPoint[] buildData(int[,] data)
        {
            var datapoints = new DataPoint[100];
            for (var i = 0; i < data.GetLength(0); i++)
            {
                var offers = new int[32];
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    offers[j] = data[i, j];
                }

                var datapoint = new DataPoint(offers);
                datapoints[i] = datapoint;
            }

            return datapoints;
        }
    }
}
