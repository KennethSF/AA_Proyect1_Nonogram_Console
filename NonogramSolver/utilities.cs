using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static NonogramSolver.NonogramFiller;
using static NonogramSolver.Program;

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

        public static bool EmptyList(List<int> list) {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list.All(element => element == 0);
        }
        
        
        public static int[,] CloneMatrix(int[,] matrix) {
            var clone = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++)
                    clone[i, j] = matrix[i, j];
            }
            return clone;
        }

        public static int[,] CreateAuxMatrix(int[,] matrix,List<int> positions,int line)
        {
            var clone = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) 
                    clone[i, j] = matrix[i,j];
            }
            if (positions == null) return clone; {
                foreach (var i in positions) 
                    clone[line,i] = 1;
            }
            return clone;
        }
        
        public static void CleanMatrix(int[,] matrix) {
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    if (matrix[i, j] !=1)
                        matrix[i, j] = 0;
                }
            }
        }
        
        public static bool HasSolution(int[,] matrix) {//Method that checks if the Nonogram has solution
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    if (matrix[i, j] == 1)
                        return true;
                }
            }
            return false;
        }

        public static void InitializeArrayOfList(List<int>[] array)
        {
            for (var i = 0; i < array.Length; i++) {
                array[i]= new List<int>();
            }
        }
        
        public static void InitializeCombinationList(List<List<int>>[] array) {
            for (int i = 0; i < array.Length; i++) 
                array[i]= new List<List<int>>();
        }
        public static List<int>[] ZeroCells(int[,] matrix, List<int>[] remainingPositions) {
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    if (matrix[i, j] == 0)
                        remainingPositions[i].Add(j);
                }
            }
            return remainingPositions;
        }

        public static void PrintArray(int[] array)
        {
            foreach(var item in array) 
                Console.Write(item+" ");
            Console.WriteLine();
        }
        
        public static void RemainingAmount(int[,] matrix, int[] array, List<List<int>> rowList) {
            for (var i = 0; i < array.Length; i++) 
                array[i] = rowList[i].Sum() - TotalPainted(matrix, array, i);
        }

        private static int TotalPainted(int[,] matrix, int[] array,int line) {
            var cont = 0;
            for (var i = 0; i < matrix.GetLength(0); i++) {
                if (matrix[line, i] == 1)
                    cont++;
            }
            return cont;
        }

        public static void PrintListOfList(IEnumerable<List<int>> list)
        {
            foreach (var sublist in list) {
                foreach (var obj in sublist)
                    Console.Write(obj + " ");
                Console.WriteLine();
            }
        }
        
        public static void PrintArrays(List<string> data) {
            foreach (var array in data) {
                Console.WriteLine();
                foreach (char item in array) {
                    Console.Write(" ");
                    Console.Write(item); 
                }
            }
        }
        
        public static int TotalCellsToPaint(IEnumerable<List<int>> rowList,IEnumerable<List<int>> columnList) {
            return rowList.SelectMany(sublist => sublist).Sum();
        }
        
        public static void PrintArrayOfList(IEnumerable<List<int>> array)
        {
            foreach (var t in array) {
                var aux = "";
                if (t != null) {
                    aux = t.Aggregate(aux, (current, element) => current + (element + " "));
                    Console.WriteLine(aux);
                }
                else
                    break;
            }
        }
        
        public static void BlockLine(int[,] matrix,int dimension, int line)
        {
            for (int i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension == 0) {
                    if (matrix[line, i] == 0)
                        matrix[line, 0] = 3;
                }
                else {
                    if (matrix[i, line] == 0)
                        matrix[i, line] = 3;
                }
            }
        }

        private static void CombinationUtil(IReadOnlyList<int> arr,List<List<int>>[]combinationList, int []data,  
            int start, int end,  
            int index, int r,int pos,List<int> auxList,int[,]matrix) 
        { 
            List<int> auxList2=new List<int>();  
            // Current combination is  
            // ready to be printed,  
            // print it 
            if (index == r)
            {
                for (int j = 0; j < r; j++)
                    auxList2.Add(data[j]);
                var auxMatrix = CreateAuxMatrix(matrix, auxList2, pos);
                if (LineDone(auxMatrix, 0, pos, rowList[pos]))
                    combinationList[pos].Add(auxList2);
                return; 
            } 
  
            // replace index with all 
            // possible elements. The  
            // condition "end-i+1 >=  
            // r-index" makes sure that  
            // including one element 
            // at index will make a  
            // combination with remaining  
            // elements at remaining positions 
            for (int i = start; i <= end &&  
                                end - i + 1 >= r - index; i++) 
            { 
                data[index] = arr[i]; 
                CombinationUtil(arr,combinationList, data, i + 1,  
                    end, index + 1, r,pos,auxList,matrix); 
            } 
        } 
  
        // The main function that stores 
        // all combinations of size r 
        // in arr[] of size n. This  
        // function mainly uses combinationUtil() 
        private static void CreateCombination(List<int>arr,List<List<int>>[] combinationList,  
            int n, int r,int pos,List<int> auxList,int[,] matrix) 
        {  
            // A temporary array to store  
            // all combination one by one 
            var data = new int[r]; 
            
            // Print all combination  
            // using temprary array 'data[]' 
            CombinationUtil(arr,combinationList, data, 0, 
                n - 1, 0, r,pos,auxList,matrix); 
        }

        public static void CreateCombinations(List<int>[] zeroCells,int[] remainingAmount,List<List<int>>[]combinationList,int[,] matrix) {
            for (var i = 0; i < zeroCells.Length; i++) {
                if (zeroCells[i] != null) {
                    List<int> auxList=new List<int>();
                    CreateCombination(zeroCells[i],combinationList, zeroCells[i].Count, remainingAmount[i],i,auxList,matrix);
                    
                }
                else
                    combinationList[i]=null;
            }
        }

        public static void CleanCombination(int[,] matrix, List<int> combinationList, int row)
        {
            foreach (var val in combinationList) 
                matrix[row, val]=0;
        }
    }
}