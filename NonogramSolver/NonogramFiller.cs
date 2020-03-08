using System;
using System.Collections.Generic;

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
            foreach (List<int> actual in list)
            {
                if (actual.Count == 1)
                {
                    if (actual[0] == matrix.GetLength(dimension))
                        matrix = PaintAll(matrix, dimension, i);
                    if (actual[0] >= matrix.GetLength(dimension) / 2 + 1)
                        matrix = PaintMin(matrix, actual[0], dimension, i);
                }

                i++;
            }
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

    }
}