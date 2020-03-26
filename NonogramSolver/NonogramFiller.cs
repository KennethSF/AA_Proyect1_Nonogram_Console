using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using static NonogramSolver.utilities;
using static NonogramSolver.Program;

namespace NonogramSolver
{
    public class NonogramFiller
    {
        //public static int 
        public static List<string> BinaryCombinations=new List<string>();
        public static int[,] matrixCopy;
        
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
                //Console.WriteLine("Position: {0} Min: {1}",position,min);
                matrix[position, min] = 1;
                //In case the array is pair
                if (matrix.GetLength(dimension) % 2 == 0)
                {
                    
                    matrix[position, min-1] = 1;
                    color -= 1;
                    previous -= 1;
                }
            }
            else
            {
                matrix[min, position] = 1;
                //In case the array is pair
                if (matrix.GetLength(dimension) % 2 == 0)
                {
                    
                    matrix[min -1, position] = 1;
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
            var i = 0;
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

        public static void FullSeparatedClues(int[,] matrix,List<List<int>>list,int dimension)
        {
            var dimensionLenght = matrix.GetLength(dimension);
            foreach (var sublist in list)
            {
                var totalCells = sublist.Sum()+sublist.Count-1;
                if (dimensionLenght == totalCells)
                    matrix=FullSeparatedCluesAux(matrix, sublist, dimension,list.IndexOf(sublist));
            }
        }

        public static int[,] FullSeparatedCluesAux(int[,] matrix, List<int> list,int dimension,int line)
        {
            var pos = 0;
            var auxList = list;
            for (var i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension == 0) {
                    matrix[line, i] = 1;
                }else {
                    matrix[i, line] = 1;
                }
                auxList[pos]--;
                if (auxList[pos]==0)
                {
                    pos++;
                    i++;
                }

            }
            return matrix;
        }
        public static bool LineDone(int[,] matrix, int dimension, int line, List<int> clues)
        {

            var listMax = clues.Count - 1;
            var listPos = 0;
            //var cont = 0;
            var nextEmpty = false;

            List<int> CloneList=new List<int>(clues);
            //Console.WriteLine("Dimension: {0} Linea: {1}",dimension,line);
            //CloneList.ForEach(Console.WriteLine);
            //Console.WriteLine();
            for (var i = 0; i < matrix.GetLength(dimension); i++)
            {
                if (nextEmpty == true) {// This if search for the space that has to be between two numbers
                    nextEmpty = false;
                    if (dimension == 0) {
                        if (matrix[line, i] == 1) {//In case there is a filled cell, return false
                            return false;
                        }
                            
                    }
                    else {
                        if (matrix[i, line] == 1) {
                            return false;
                        }
                            
                    }
                }
                else {
                    if (dimension == 0) {
                        if (matrix[line, i] == 1) {
                            //cont++;
                            CloneList[listPos]--;
                        }
                    }
                    else {
                        if (matrix[i, line] == 1)
                            CloneList[listPos]--;
                    }
                    if (CloneList[listPos] == 0) {
                        nextEmpty = true;
                        //Console.WriteLine("Cambio");
                        listPos++;
                    }
                    if (listPos == clues.Count)
                        break;
                }
            }
            
            //Console.Write("ListPos: {0} Count: {1}",listPos,clues.Count-1);
            //Console.WriteLine("La siguiente lista será enviada:");
            //auxList.ForEach(Console.Write);
            //Console.WriteLine();
            
            //auxList.ForEach(Console.WriteLine);
            //Console.WriteLine();
            return EmptyList(CloneList);
        }
        
        public static bool ValidOption(int[,] matrix, List<int> cluesX,List<int> cluesY,int x,int y)
        {
            int contX = 0;
            int contY = 0;
            int totalX = cluesX.Sum();
            int totalY = cluesY.Sum();
            //Console.WriteLine("Posicion X: {0} Posicion Y: {1}",x,y);
            for (int i = 0; i < matrix.GetLength(0); i++) {
                
                if (matrix[x, i] == 1)
                    contX++;
            }
            for (int j = 0; j < matrix.GetLength(1); j++) {
                if (matrix[j,y] == 1)
                    contY++;
            }

            if (contX > totalX || (contY > totalY))
            {
                /*Console.WriteLine("ContX: {0} TotalX: {1}",contX,totalX);
                Console.WriteLine("ContY: {0} TotalY: {1}",contY,totalY);
                Console.WriteLine("-------------------------------------");*/
                return false;
            }
                
            
            
            return true;
        }
        
        public static bool NonogramSolved(int[,] matrix,List<List<int>> rowList, List<List<int>> columnList)
        {
            //PrintListOfList(rowList);
            for (var i = 0; i < matrix.GetLength(0); i++) {
                List<int> aux=new List<int>(rowList[i]);
                if (LineDone(CloneMatrix(matrix), 0, i, rowList[i])==false)
                {
                    //Console.WriteLine("Fila {0} no está completa",i);
                    return false;
                }
                    
            }
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                List<int> aux=new List<int>(columnList[i]);
                //aux.ForEach(Console.WriteLine);
                if (LineDone(matrix, 1, i, aux)==false)
                {
                    //Console.WriteLine("Columnas no están completas");
                    return false;
                }
                    
            }
            return true;
        }
        
        
        //Method to pass through all the matrix
        public static void CheckMatrix(int[,] matrix, int row, int column,int cont) {
            //Print2DArray(matrix);
            Console.WriteLine(cont);
            //Console.WriteLine("----------------------");
            if (AllVisited(matrix))
            {
                Console.WriteLine("Matriz resuelta");
                //Print2DArray(matrix);
                NumSteps = 1;
            }
            else
            {

                if (matrix[row, column] != 1 && NumSteps==0)
                {
                    matrix[row, column] = 1;
                    if(row>0)
                        CheckMatrix(CloneMatrix(matrix),row-1,column,cont+1);
                    if(row<matrix.GetLength(0)-1)
                        CheckMatrix(CloneMatrix(matrix),row+1,column,cont+1);
                    if(column>0) 
                        CheckMatrix(CloneMatrix(matrix),row,column-1,cont+1);
                    if(column<matrix.GetLength(1)-1)
                        CheckMatrix(CloneMatrix(matrix),row,column+1,cont+1);   
                }
            }
        }

        public static void Bactracking(int pos,List<List<int>> zeroCells,int[,] matrix,List<int>[] backtrackingArray
            ,List<List<int>> rowList,List<List<int>> columnList)
        {
            foreach (var t in zeroCells)
            {
                var auxMatrix = CreateAuxMatrix(matrix,t);
                
                
                if (ValidOption(auxMatrix,rowList[t[0]],columnList[t[1]]
                    ,t[0],t[1])&& NotOnTheList(backtrackingArray,t))
                {
                    
                    //matrix[t[0], t[1]] = 1;
                    backtrackingArray[pos] = t;
                    if (pos == backtrackingArray.Length - 1)
                    {
                        int[,] option = TryMatrixOption(matrix,backtrackingArray);
                        //PrintArrayOfList(backtrackingArray);
                        
                        
                        //Console.WriteLine("Llegué al final");
                        if (NonogramSolved(option,rowList,columnList))
                        {
                            Console.WriteLine("----------------------------------");
                            Print2DArray(option);
                            Console.WriteLine("----------------------------------");
                            
                        }
                    }
                    else
                    {
                        Bactracking(pos+1,zeroCells,matrix,backtrackingArray,rowList,columnList);
                    }
                    matrix[t[0], t[1]] = 0;
                }
            }
        }

        

        public static void StartExe(int[,] matrix, List<List<int>> rowList, List<List<int>> columnList)
        {
            FillFixedFields(matrix, rowList, columnList);
            CheckEmptyCells(matrix, 0, rowList);
            CheckEmptyCells(matrix, 1, columnList);
            FullSeparatedClues(matrix,rowList,0);
        
            //int[] possiblePositions= new int[zeroCells.Count];
            var cellsToPaint = TotalCellsToPaint(rowList, columnList); //Sums all the cells that have to be painted
            var remainingCells = RemainingCells(matrix, cellsToPaint); //Cells that are left

            //Things that are going to be used in bactracking method
            var zeroCells = ZeroCells(matrix); //Contains all the positions that have 0
            List<int>[] backtrackingArray = new List<int>[remainingCells];
                        


            Print2DArray(matrix);
            
            //Print2DArray(matrix);
            //PrintListOfList(rowList);
            /*ValidOption(matrix, rowList[0], zeroCells[0][0], zeroCells[0][1]);
            ValidOption(matrix, rowList[1], zeroCells[1][0], zeroCells[1][1]);
            ValidOption(matrix, rowList[2], zeroCells[2][0], zeroCells[2][1]);
            ValidOption(matrix, rowList[3], zeroCells[3][0], zeroCells[3][1]);
            ValidOption(matrix, rowList[4], zeroCells[4][0], zeroCells[4][1]);*/
            //Bactracking(0,zeroCells,matrix,backtrackingArray,rowList,columnList);

        }
    }
}