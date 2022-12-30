using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo
{
    class Mainof_MonteCarlo
    {
        public List<Task_InputVal> Tasks = new List<Task_InputVal>();
        public int ESTAverage { get; private set; }
        Random randNum = new Random();

        public void AddTask(Task_InputVal task)
        {
            Tasks.Add(task);
        }

        public int[] CalculateEstimated()
        {
            int min = 0, avg = 0, max = 0;

            foreach (Task_InputVal task in Tasks)
            {
                min += task.TestBC;
                avg += task.TestAC;
                max += task.TestWC;
            }
            if (max < min) throw new InvalidOperationException("As a scenerio , Worst case must be longer than best!!");

            int[] TimeCases = new int[] { min, avg, max };
            return TimeCases;
        }

        public int CreateRandomEST()
        {
            int sum = 0;

            foreach (Task_InputVal task in Tasks)
            {
                int whichCase = randNum.Next(3);
                if (whichCase == 0)
                    sum += task.TestBC;
                if (whichCase == 1)
                    sum += task.TestAC;
                if (whichCase == 2)
                    sum += task.TestWC;
            }
            return sum;
        }

        public Bucket_task Simulate()
        {
            int totalCostOfRandomPlans = 0;
            int iterations = 10000;
            int[] Estimation = CalculateEstimated();
            int min = Estimation[0], max = Estimation[2];
            Bucket_task bucket = new Bucket_task(10, min, max);


            for (int i = 0; i < iterations; i++)
            {
                int randomPlanCost = CreateRandomEST();

                bucket.addValueToBucket(randomPlanCost);

                totalCostOfRandomPlans += randomPlanCost;
            }
            this.ESTAverage = totalCostOfRandomPlans / (iterations);

            return bucket;
        }
    }
    class Bucket_task
    {
        public Dictionary<int, int> buckets = new Dictionary<int, int>();
        public int Bucket_Count { get; private set; }
        public int LOWinterval { get; private set; }
        public int HIGHinterval { get; private set; }
        public int Step_Size { get; private set; }

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
                this.buckets.Add(this.LOWinterval + (this.Step_Size * i), 0);
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

            foreach (KeyValuePair<int, int> keyValue in this.buckets)
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
    class Program
    {
        static void Main(string[] args)
        {
            string getinput;
            do
            {

            Console.Clear();
            Console.WriteLine("Type the pattern of tasks in the following order: BEST,AVARAGE,WORST case scenerios " +
                              "\nIf you type END , it finishes :");
            try
            {
                Mainof_MonteCarlo mainOperation = new Mainof_MonteCarlo();
                int TaskNumber = 1; // default it is 1 , it types any it automatically increase 2 3 ...
                while (true)
                {
                    Console.Write($"Task #{TaskNumber}: ");
                    string taskinput = Console.ReadLine(); // get the values

                    if (taskinput.ToLower() == "end") break;
                    else TaskNumber++; // it increases if it is not END typed

                    mainOperation.AddTask(new Task_InputVal(taskinput));
                }
                int[] EstimatedNumbers = mainOperation.CalculateEstimated(); // list for estimations
                int min_days = EstimatedNumbers[0], max_days = EstimatedNumbers[2];

                Bucket_task bucketsim = mainOperation.Simulate();

                Console.WriteLine("After 10000 randoms plans estimated, the results are: "); // it shows how it look in 1000 random plans
                Console.WriteLine($"Minimum = {min_days} days"); //minimum days
                Console.WriteLine($"Average = {mainOperation.ESTAverage} days"); // avarage  
                Console.WriteLine($"Maximum = {max_days} days"); // maximum days

                Console.WriteLine("Estimation of finishing the plan in: \n" + bucketsim);
                bucketsim.ESTAccumulate();
                Console.WriteLine("Accumulate estimation of finishing the plan in or before: " + bucketsim);
            }
            catch(Exception expectionS)
            {
                Console.WriteLine(expectionS.Message + "Please try again :)!");
            }

            Console.WriteLine("Type any to continue.. or exit");
            getinput = Console.ReadLine();

            } while (getinput != "exit");


        }
    }
    class Task_InputVal
    {
        public int TestBC { get; set; }
        public int TestAC { get; set; }
        public int TestWC { get; set; }
        public int[] Tasks { get; set; }

        public Task_InputVal(string Input)
        {
            setArray(Input);
        }

        //taking string input of user and parsing it to int array
        public void setArray(string Input)
        {
            Input = Input.Trim();
            string[] Estimations = Input.Split(',');

            if (Estimations.Length < 2) throw new InvalidOperationException("You must declare worst,average,best case scenerios. Please Seperate with ','!!");

            this.Tasks = new int[Estimations.Length];

            for (int i = 0; i < Estimations.Length; i++)
            {
                if (!int.TryParse(Estimations[i], out Tasks[i])) Console.WriteLine("Wrong input , Try Again !");
            }
            this.TestBC = Tasks.Min();
            this.TestAC = (int)Tasks.Average();
            this.TestWC = Tasks.Max();
        }
    }
}
