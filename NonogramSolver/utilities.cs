using System;
using System.Collections.Generic;
using System.Linq;
using static NonogramSolver.NonogramFiller;

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
            //Console.Write("Fila 0:" );
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }

                Console.WriteLine();
               // Console.Write("Fila {0}: ",i+1);
            }
        }

        //This method fill a boolean list with the size of the dimensions of the matrix
        public static List<bool> SolvedLineList(List<bool> list, int max)
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
            foreach (var element in list)
            {
                //Console.Write(element);
                if (element != 0)
                {
                    //Console.WriteLine("Problema en EmptyList no está vacía");
                    return false;
                }
            }
            return true;
        }
        
        
        public static int[,] CloneMatrix(int[,] matrix)
        {
            var clone = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    clone[i, j] = matrix[i, j];
                }
            }

            return clone;
        }

        public static int[,] CreateAuxMatrix(int[,] matrix,List<int> positions)
        {
            var clone = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    clone[i, j] = matrix[i,j];
                }
            }
            clone[positions[0],positions[1]] = 1;
            return clone;
        }
        
        public static void CleanMatrix(int[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 2)
                        matrix[i, j] = 0;
                }
            }
        }

        public static List<List<int>> ZeroCells(int[,] matrix)
        {
            var zeroCells = new List<List<int>>();
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        var aux = new List<int>() {i, j};
                        zeroCells.Add(aux);
                    }
                }
            }

            return zeroCells;
        }

        public static void PrintListOfList(IEnumerable<List<int>> list)
        {
            foreach (var sublist in list)
            {
                foreach (var obj in sublist)
                {
                    Console.Write(obj + " ");
                }

                Console.WriteLine();
            }
        }

        // Function to generate all binary strings 
        // Function to print the output 
        static void InsertCombination(int[] arr, int n)
        {
            string aux = "";
            for (int i = 0; i < n; i++)
            {
                aux += arr[i];
            } 
            //Console.WriteLine(aux.Length);
            BinaryCombinations.Add(aux);
        }

        private static void WriteToConsole(List<int> items)
        {
            foreach (var o in items)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine("-----------------");
        }
    // Function to generate all binary strings 
        public static void generateAllBinaryStrings(int n, int[] arr, int i)
        {
            if (i == n)  
            { 
                InsertCombination(arr, n); 
                return; 
            } 
  
            // First assign "0" at ith position 
            // and try for all other permutations 
            // for remaining positions 
            arr[i] = 0; 
            generateAllBinaryStrings(n, arr, i + 1); 
  
            // And then assign "1" at ith position 
            // and try for all other permutations 
            // for remaining positions 
            arr[i] = 1; 
            generateAllBinaryStrings(n, arr, i + 1); 
            
        }

        public static void printArrays(List<string> data)
        {
            foreach (var array in data) {
                Console.WriteLine();

                foreach (char item in array) {
                    Console.Write(" ");
                    Console.Write(item); 
                }
            }
        }
        public static int[,] TryMatrixOption(int[,] matrix,List<int>[] positions)
        {
            var auxMatrix = matrix;
            foreach (List<int> element in positions) {
                auxMatrix[element[0], element[1]] = 1;
            }            
            return auxMatrix;
        }
        
        public static int TotalCellsToPaint(IEnumerable<List<int>> rowList,IEnumerable<List<int>> columnList)
        {
            var numberOfCells=0;
            foreach (var sublist in rowList) {
                foreach (var obj in sublist) {
                    numberOfCells += obj;
                }
            }
            return numberOfCells;
        }
        
        public static int RemainingCells(int[,] matrix,int TotalCells)
        {

            var remainingCells = TotalCells;
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    if (matrix[i, j] == 1) {
                        remainingCells--;
                    }
                }
            }
            return remainingCells;
        }

        public static void PrintRow(int[,] matrix,int line)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write(matrix[line,i]+" ");
            }
            Console.WriteLine();
        }
        
        public static bool AllVisited(int[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    if (matrix[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void PrintArrayOfList(List<int>[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                foreach (int element in array[i]) {
                    Console.Write(element);
                }
                Console.WriteLine();
            }
        }

        public static bool NotOnTheList(List<int>[] array, List<int> option)
        {
            foreach (var t in array) {
                if (t == option)
                    return false;
            }
            return true;
        }
    }
}