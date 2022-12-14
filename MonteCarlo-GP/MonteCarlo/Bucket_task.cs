using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo
{
    class Bucket_task
    {
        public Dictionary<int, int> buckets = new Dictionary<int, int>();
        public int Bucket_Count { get; private set; }
        public int LOWinterval { get; private set; }
        public int HIGHinterval { get; private set; }
        public int Step_Size { get; private set;}

        public Bucket_task(int newBucket_Count, int newLOWinterval, int newHIGHinterval)
        {
            this.Bucket_Count = newBucket_Count;
            this.LOWinterval = newLOWinterval;
            this.HIGHinterval = newHIGHinterval;
            this.Step_Size = (Math.Abs(HIGHinterval - Math.Abs(LOWinterval)) / Bucket_Count);

            initBuckets();
        }

        public void initBuckets()
        {
            for (int i = 0; i < this.Bucket_Count; i++)
            {
                this.buckets.Add(this.LOWinterval+(this.Step_Size*i), 0);
            }
        }

        public void addValueToBucket(int val)
        {
            int idx = this.getBucketIdxForValue(val);
            this.buckets[this.buckets.ElementAt(idx).Key]++;
        }

        public int getBucketIdxForValue(int val)
        {
            int idx = 0;

            // Find bucket, put values on the outer size of the range in the last bucket
            while ((idx < this.Bucket_Count - 1) && val > this.LOWinterval + this.Step_Size * (idx + 1))
            {
                idx++;
            }
            return idx;
        }

        public override string ToString()
        {
            string result = string.Empty;

            foreach (KeyValuePair<int,int> keyValue in this.buckets)
            {
                result += $"{keyValue.Key} days: {keyValue.Value / 100}%\n";   // value divided by number of iteration * 100 %
            }

            return result;
        }

        public void ESTAccumulate()
        {
            for (int i = 1; i < buckets.Count; i++)
            {
                buckets[buckets.ElementAt(i).Key] = buckets.ElementAt(i - 1).Value + buckets.ElementAt(i).Value;
            }
        }

    }
}
