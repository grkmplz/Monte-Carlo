using System;
using System.Collections.Generic;

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

        public int CreateRandomEST() {
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
}
