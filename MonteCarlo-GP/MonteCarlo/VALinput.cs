using System;
using System.Linq;

namespace MonteCarlo
{
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
