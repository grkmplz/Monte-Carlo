using System;

namespace MonteCarlo
{
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
}
