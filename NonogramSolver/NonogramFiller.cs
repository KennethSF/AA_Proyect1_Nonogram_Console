using System.Collections.Generic;

namespace NonogramSolver
{
    public class NonogramFiller
    {
        public static void FillFixedFields(int[,] matrix,List<List<int>> rowList,List<List<int>> columnList)
        {
            //0 For rows
            //1 for columns
            FillLine(matrix,rowList,0);
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
                        matrix = PaintAll(matrix,dimension,i);
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
    }
}