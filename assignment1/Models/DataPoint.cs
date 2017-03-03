using System;
using System.Collections.Generic;

namespace Models
{
    class DataPoint
    {

        public int cluster { get; set; }
        public int[] allOffers { get; set; }
        public List<int> boughtOffers { get; set; }
        public DataPoint(int[] data)
        {
            this.allOffers = data;

            boughtOffers = new List<int>();
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] == 1)
                {
                    boughtOffers.Add(i + 1);
                }
            }
        }
    }
}