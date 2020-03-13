using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i,j] + "\t");
                }
                Console.WriteLine();
            }
        }
        
        //This method fill a boolean list with the size of the dimensions of the matrix
        public static List<bool> SolvedLineList(List<bool> list,int max)
        {
            for (var i = 0; i < max; i++)
            {
                list.Add(true);
            }
            return list;
        }

        public static bool EmptyList(List<int> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list.All(element => element == 0);
        }
    }
}