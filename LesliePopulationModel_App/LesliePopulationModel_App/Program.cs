using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesliePopulationModel_App
{
    class AgeGroup
    {
        // Number of individuals in the age group.
        public int PopulationNum { get; set; }
        public float BirthRate { get; set; }
        public float SurvivalRate { get; set; }

        public AgeGroup()
        {
            Console.Write("Enter population number: ");
            PopulationNum = int.Parse(Console.ReadLine());

            Console.Write("Enter birth rate: ");
            BirthRate = float.Parse(Console.ReadLine());

            Console.Write("Enter survival rate: ");
            SurvivalRate = float.Parse(Console.ReadLine());

            Console.WriteLine();
        }

        public static List<AgeGroup> Input(int num) {

            List<AgeGroup> list = new List<AgeGroup>();

            for (int i = 0; i < num; i++)
            {
                list.Add(new AgeGroup());
            }

            return list;
        }
    }

    // Main class
    class Program
    {
        // Returns a list that represents the age matrix for a given year.
        public static List<float> SolutionMatrix(float[,] Lmatrix, List<float> Xmatrix)
        {
            List<float> solution = new List<float>();

            for (int row = 0; row < Xmatrix.Count; row++)
            {
                float rezultat = 0;

                for (int col = 0; col < Xmatrix.Count; col++)
                {
                    rezultat += Lmatrix[row, col] * Xmatrix[col];
                }

                solution.Add(rezultat);
            }

            return solution;
        }

        // Writes out a list.
        private static void ListWriting(List<float> solution)
        {
            foreach (float item in solution)
            {
                Console.WriteLine(item);
            }
        }

        // Method that sets values from the lists of each age group into 2D array. AKA the Leslie Matrix.
        private static float[,] LMatrix(List<float> birth, List<float> survival)
        {
            float[,] leslieMatrix = new float[birth.Count, birth.Count];

            for (int row = 0; row < birth.Count; row++)
            {
                if (row == 0)
                {
                    for (int col = 0; col < birth.Count; col++)
                    {
                        leslieMatrix[row, col] = birth[col];
                    }
                }
                else
                {
                    for (int col = 0; col < birth.Count; col++)
                    {
                        if (col == row - 1)
                        {
                            leslieMatrix[row, col] = survival[col];
                        }
                        else
                        {
                            leslieMatrix[row, col] = 0;
                        }
                    }
                }
            }

            return leslieMatrix;
        }

        // Method for calculating matrix raised to the power of 'a'.
        private static float[,] MatrixPow(float[,] Lmatrix, int year, int agegroups)
        {
            float[,] solution = new float[agegroups, agegroups];
            float[,] tempSolution = new float[agegroups, agegroups];
            int a = 1;

            for (int row = 0; row < agegroups; row++)
            {
                for (int col = 0; col < agegroups; col++)
                {
                    for (int i = 0; i < agegroups; i++)
                    {
                        solution[row, col] += (Lmatrix[row, i]) * (Lmatrix[i, col]);
                    }
                }
            }

            while (a < year - 1)
            {
                tempSolution = solution;

                solution = new float[agegroups, agegroups];

                for (int row = 0; row < agegroups; row++)
                {
                    for (int col = 0; col < agegroups; col++)
                    {
                        for (int i = 0; i < agegroups; i++)
                        {
                            solution[row, col] += (tempSolution[row, i]) * (Lmatrix[i, col]);
                        }
                    }
                }

                a++;
            }
            return solution;
        }

        // Writes out 2D array.
        private static void MatrixOutput(float[,] arr, int agegroups)
        {
            for (int i = 0; i < agegroups; i++)
            {
                for (int j = 0; j < agegroups; j++)
                {
                    Console.Write(arr[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        // Method that returns Leslie matrix and end-solution matrix but with variables (general parameters).
        private static void GeneralParameters(int ageGroupNum, int yearNum, string X, string b, string a)
        {
            // Xmatrix
            Console.WriteLine(X + "(0) =");
            Console.WriteLine();
            for (int i = 0; i < ageGroupNum; i++)
            {
                Console.WriteLine("|" + X + "(" + (i + 1) + ")|");
            }

            Console.WriteLine();
            Console.WriteLine("L(" + yearNum + ")");

            // Lmatrix
            for (int i = 0; i < ageGroupNum; i++)
            {
                int k = 1;
                Console.Write(a + "(" + (i + 1) + "," + k + ") " + "\t");
                k++;
            }
            Console.WriteLine();

            for (int i = 0; i < ageGroupNum; i++)
            {
                for (int j = 0; j < ageGroupNum; j++)
                {
                    if (i == ageGroupNum - 1 && j == ageGroupNum - 1)
                    {
                        Console.Write("0 " + "\t");
                    }
                    else
                    {
                        if (j == i)
                        {
                            Console.Write(b + "(" + (i + 1) + "," + (j + 1) + ") " + "\t");
                        }
                        else
                        {
                            Console.Write("0 " + "\t");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Leslie Population Model";
            Console.WriteLine("WELCOME\n");
            Console.WriteLine("Here you can pick do you want to calculate for general(variables) or specific(numbers) values.");
            string  continuation = "";

            do
            {
                continuation = "";
                // Number of age groups.
                int ageGroupNum = 0;
                // Number of years to simulate.
                int yearNum = 0;
                int ageGroupRange = 0;

                List<float> matrixX0 = new List<float>();
                List<AgeGroup> listOfGroups = null;
                List<float> birthRate = new List<float>();
                List<float> survivalRate = new List<float>();

                string menuPick = "";

                Console.WriteLine("---------------------------");
                Console.WriteLine("Specific values (numbers) (1)");
                Console.WriteLine("General values (variables)  (2)");
                Console.WriteLine("Exit (9)");
                Console.WriteLine("---------------------------");
                Console.WriteLine("Your pick(1/2/9): ");
                menuPick = Console.ReadLine();
                if (menuPick == "9") Environment.Exit(0);


                Console.Clear();

                Console.WriteLine("How many age groups do you want to run this simulation for? ");
                ageGroupNum = int.Parse(Console.ReadLine());

                Console.Write("How many years do you want to simulate? ");
                yearNum = int.Parse(Console.ReadLine());

                Console.Write("What is the year range (ex. 0-5 is 5 etc.) of the age group?(in years) : ");
                ageGroupRange = int.Parse(Console.ReadLine());

                yearNum = yearNum / ageGroupRange;

                Console.WriteLine();

                if (menuPick == "1")
                {
                    // Lista svih dobnih skupina
                    listOfGroups = AgeGroup.Input(ageGroupNum);

                    // Punjenje birthRate liste
                    foreach (AgeGroup item in listOfGroups)
                    {
                        birthRate.Add(item.BirthRate);
                    }
                    // Punjenje survivalRate liste
                    foreach (AgeGroup item in listOfGroups)
                    {
                        survivalRate.Add(item.SurvivalRate);
                    }

                    // Punjenje Xmatrice
                    foreach (AgeGroup item in listOfGroups)
                    {
                        matrixX0.Add(item.PopulationNum);
                    }

                    Console.WriteLine();

                    Console.WriteLine("L matrix(1)");
                    MatrixOutput(LMatrix(birthRate, survivalRate), ageGroupNum);

                    Console.WriteLine("X(0) matrix");
                    ListWriting(matrixX0);

                    Console.WriteLine();
                    Console.WriteLine("---------------------------------");

                    if (yearNum == 1)
                    {
                        Console.WriteLine("L matrix(" + yearNum + ")");
                        MatrixOutput(LMatrix(birthRate, survivalRate), ageGroupNum);

                        Console.WriteLine();

                        Console.WriteLine("Solutin for matrix X(" + yearNum + ")");
                        ListWriting(SolutionMatrix(LMatrix(birthRate, survivalRate), matrixX0));

                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("L matrix(" + yearNum + ")");
                        MatrixOutput(MatrixPow(LMatrix(birthRate, survivalRate), yearNum, ageGroupNum), ageGroupNum);

                        Console.WriteLine();

                        Console.WriteLine("Solution for matrix X(" + yearNum + ")");
                        ListWriting(SolutionMatrix(MatrixPow(LMatrix(birthRate, survivalRate), yearNum, ageGroupNum), matrixX0));

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
                else if (menuPick == "2")
                {
                    string X = "";
                    string a = "";
                    string b = "";
                    Console.WriteLine("--------------------------------------");
                    Console.Write("Enter the parameter for population number (ex. X): ");
                    X = Console.ReadLine();
                    Console.Write("Enter the parameter for the birth rate (ex. a): ");
                    a = Console.ReadLine();
                    Console.Write("Enter the parameter for the survival rate (ex. b): ");
                    b = Console.ReadLine();
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine();

                    GeneralParameters(ageGroupNum, yearNum, X, b, a);
                }
                else if (menuPick == "4")
                {
                    MatrixOutput(LMatrix(birthRate, survivalRate), ageGroupNum);
                }

                Console.WriteLine();

                Console.WriteLine("Do you want to make more simulations? (y/n)");
                continuation = Console.ReadLine();

                if (continuation == "n") Environment.Exit(0);

            } while (continuation == "y");

            Console.Read();
        }
    }
}
