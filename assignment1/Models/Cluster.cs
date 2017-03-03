using System;

namespace Models
{
    class Cluster
    {
        public Cluster(int cluster, float[] datapoints)
        {
            this.cluster = cluster;
            this.datapoints = datapoints;
        }

        public int cluster { get; set; }
        public float[] datapoints { get; set; }
    }
}