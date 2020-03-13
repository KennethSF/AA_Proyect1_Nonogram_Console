using System;
using System.Collections.Generic;
using static NonogramSolver.utilities;

namespace NonogramSolver
{
    public class NonogramFiller
    {
        public static void FillFixedFields(int[,] matrix, List<List<int>> rowList, List<List<int>> columnList)
        {
            //0 For rows
            //1 for columns
            FillLine(matrix, rowList, 0);
            FillLine(matrix, columnList, 1);
            
        }

        //This method fill the cells that always will be colored according to the clues
        public static void FillLine(int[,] matrix, List<List<int>> list, int dimension)
        {
            int i = 0;
            int aux = 0;
            foreach (List<int> actual in list)
            {
                if (actual.Count == 1)
                {
                    if (actual[0] == matrix.GetLength(dimension))
                        matrix = PaintAll(matrix, dimension, i);
                    if (actual[0] >= matrix.GetLength(dimension) / 2 + 1)
                        matrix = PaintMin(matrix, actual[0], dimension, i);
                    aux++;
                }
                i++;
            }
            matrix=CheckEmptyCells(matrix,dimension,list);
            
        }

        private static int[,] PaintAll(int[,] matrix, int dimension, int position)
        {
            for (var i = 0; i < matrix.GetLength(dimension); i++)
            {
                if (dimension == 0)
                    matrix[position, i] = 1;
                else
                    matrix[i, position] = 1;
            }

            return matrix;
        }

        private static int[,] PaintMin(int[,] matrix, int fill, int dimension, int position)
        {
            var residue = matrix.GetLength(dimension) - fill;
            var color = fill - residue; //Controls how many cells are left to color
            var min = matrix.GetLength(dimension) / 2;

            var previous = min - 1;
            var next = min + 1;

            //Color the center of the array
            if (dimension == 0)
            {
                matrix[position, min] = 1;
                //In case the array is pair
                if (matrix.GetLength(dimension) % 2 == 0)
                {
                    matrix[position, min] = 1;
                    color -= 1;
                }

            }
            else
            {
                matrix[min, position] = 1;
                //In case the array is pair
                if (matrix.GetLength(dimension) % 2 == 0)
                {
                    matrix[min + 1, position] = 1;
                    color -= 1;
                }
            }

            color -= 1;

            while (color > 0)
            {
                if (dimension == 0)
                    PaintRow(matrix, previous, next, position);
                else
                    PaintColumn(matrix, previous, next, position);
                previous -= 1;
                next += 1;
                color -= 2;
            }

            return matrix;
        }

        private static int[,] PaintRow(int[,] matrix, int previous, int next, int position)
        {
            if(matrix[position, previous] == 0)
                matrix[position, previous] = 1;
            if(matrix[position, next] == 0)
                matrix[position, next] = 1;
            return matrix;
        }

        private static int[,] PaintColumn(int[,] matrix, int previous, int next, int position)
        {
            if(matrix[previous, position] == 0)
                matrix[previous, position] = 1;
            if(matrix[next, position] ==0)
                matrix[next, position] = 1;
            return matrix;
        }

        private static int[,] CheckEmptyCells(int[,] matrix, int dimension,List<List<int>>list) //This function checks if a line filled all the spaces and mark the
        {                                                    //cells that never will be filled
            int i = 0;
            foreach (List<int> actual in list)
            {
                if (actual.Count == 1)
                {
                    if (actual[0] == NumberOfCells(matrix, dimension, i))
                        matrix=FillEmptyCells(matrix, dimension, i);    
                }
                i++;
            }
            return matrix;
        }
        private static int NumberOfCells (int[,] matrix, int dimension, int line)
        {
            var aux = 0;
            var length = matrix.GetLength(dimension);
            for (var i = 0; i < length; i++)
            {
                if (dimension == 0)//Counts how many cells have been filled
                {
                    if (matrix[line, i] == 1)
                        aux += 1;
                }
                else
                {
                    if (matrix[i, line] == 1)
                        aux += 1;        
                }
            }
            //Console.Write(control);
            /*Console.Write("Dimension: {0} Line: {1} Aux: {2}",dimension,line,aux);
            Console.WriteLine();*/
            return aux;
        }

        private static int[,] FillEmptyCells(int[,] matrix, int dimension, int line)
        {
            
            for (var i = 0; i < matrix.GetLength(dimension); i++)
            {
                //Console.Write("Fila:{0} Columna {1}",i,);
                if (dimension == 0)
                {
                    if (matrix[line, i] == 0 && matrix[line, i] != 2)
                        matrix[line, i] = 2;
                }
                else
                    if (matrix[i, line] != 1 && matrix[i, line] != 2)
                        matrix[i, line] = 2;
            }
            return matrix;
        }

        public static bool IsDone(int[,] matrix, int dimension, int line, List<int> clues)
        {
            var listMax = clues.Count - 1;
            var listPos = 0;
            //var cont = 0;
            var nextEmpty = false;

            List<int> auxList;//Create a copy of the clue list, every time a filled cell is found, the list decrement
            auxList = clues;
            for (var i = 0; i < matrix.GetLength(dimension); i++)
            {
                if (nextEmpty == true) {// This if search for the space that has to be between two numbers
                    nextEmpty = false;
                    if (dimension == 0) {
                        if (matrix[line, i] == 1)//In case there is a filled cell, return false
                            return false;
                    }
                    else {
                        if (matrix[i, line] == 1)
                            return false;
                    }
                }
                else {
                    if (dimension == 0) {
                        if (matrix[line, i] == 1) {
                            //cont++;
                            auxList[listPos]--;
                        }
                    }
                    else {
                        if (matrix[i, line] == 1)
                            auxList[listPos]--;
                    }
                    if (clues[listPos] == 0) {
                        nextEmpty = true;
                        listPos++;
                    }
                    if (listPos == clues.Count)
                        break;
                }
            }
            //Console.Write("ListPos: {0} Count: {1}",listPos,clues.Count-1);
            return EmptyList(auxList);
        }
    }
}