using System;
using System.Collections.Generic;

namespace NonogramSolver
{
    public class utilities
    {
        public static List<int> ConvertToList(string numbers)
        {
            var auxList = new List<int>();
            var auxString = "";
            for (var i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == ',')
                {
                    auxList.Add(int.Parse(auxString));
                    auxString = "";
                }
                else
                {
                    auxString += numbers[i];
                    if (i == numbers.Length - 1)
                    {
                        auxList.Add(int.Parse(auxString));
                        auxString = "";
                        break;
                    }
                }
            }
            return auxList;
        }

        public static void PrintMaxtrix(int[,] matrix)
        {
            var rowLength = matrix.GetLength(0);
            var colLength = matrix.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", matrix[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.ReadLine();
        }
    }
}