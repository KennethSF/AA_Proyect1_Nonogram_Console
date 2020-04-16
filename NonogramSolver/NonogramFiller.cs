using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;
using static NonogramSolver.utilities;
using static NonogramSolver.Program;

namespace NonogramSolver
{
    public class NonogramFiller
    {
        private static void UnreachableCells(int[,] matrix, List<List<int>> rowList)
        {
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                if (rowList[i].Count != 1) continue;
                var filledAmount = HasA1(matrix, i);
                if (filledAmount == 0) continue;
                var first1Left = First1Left(matrix, i);
                var first1Right = First1Right(matrix, i);
                var limit = rowList[i].Sum() - filledAmount;
                var limitLeft = first1Left - limit - 1;
                var limitRight = first1Right + limit + 1;

                matrix = UnreachableCells(matrix,i,limitLeft, limitRight);
            }
        }

        private static int First1Left(int[,] matrix, int line)
        {
            for (var i = 0; i < matrix.GetLength(1); i++) {
                if (matrix[line, i] == 1)
                    return i;
            }
            return -1;
        }

        private static int First1Right(int[,] matrix, int line)
        {
            for (var i = matrix.GetLength(1)-1; i >=0; i--) {
                if (matrix[line, i] == 1)
                    return i;
            }
            return -1;
        }

        private static int[,] UnreachableCells(int[,] matrix, int line, int limitLeft,int limitRight)
        {
            while (limitLeft >= 0) {
                matrix[line, limitLeft] = 2;
                limitLeft--;
            }
            while (limitRight<matrix.GetLength(1)) {
                matrix[line, limitRight] = 2;
                limitRight++;
            }
            return matrix;
        }

        private static int HasA1(int[,] matrix, int line)
        {
            var cont = 0;
            for (var i = 0; i < matrix.GetLength(1); i++) {
                if (matrix[line, i] == 1)
                    cont++;
            }
            return cont;
        }

        private static void FillFixedFields(int[,] matrix, List<List<int>> rowList, List<List<int>> columnList) {
            //0 For rows
            //1 for columns
            FillLine(matrix, rowList, 0);
            FillLine(matrix, columnList, 1);
            
        }

        //This method fill the cells that always will be colored according to the clues
        private static void FillLine(int[,] matrix, List<List<int>> list, int dimension) {
            var i = 0;
            foreach (List<int> actual in list) {
                if (actual.Count == 1) {
                    if (actual[0] == matrix.GetLength(dimension))
                        matrix = PaintAll(matrix, dimension, i);
                    if (actual[0] >= matrix.GetLength(dimension) / 2 + 1)
                        matrix = PaintMin(matrix, actual[0], dimension, i);
                }
                i++;
            }
            CheckEmptyCells(matrix,dimension,list);
            
        }

        private static int[,] PaintAll(int[,] matrix, int dimension, int position) {
            for (var i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension == 0)
                    matrix[position, i] = 1;
                else
                    matrix[i, position] = 1;
            }
            return matrix;
        }

        private static int[,] PaintMin(int[,] matrix, int fill, int dimension, int position) {
            var residue = matrix.GetLength(dimension) - fill;
            var color = fill - residue; //Controls how many cells are left to color
            var min = matrix.GetLength(dimension) / 2;

            var previous = min - 1;
            var next = min + 1;

            //Color the center of the array
            if (dimension == 0) {
                //Console.WriteLine("Position: {0} Min: {1}",position,min);
                matrix[position, min] = 1;
                //In case the array is pair
                if (matrix.GetLength(dimension) % 2 == 0) {
                    matrix[position, min-1] = 1;
                    color -= 1;
                    previous -= 1;
                }
            }
            else {
                matrix[min, position] = 1;
                //In case the array is pair
                if (matrix.GetLength(dimension) % 2 == 0) {
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

        private static int[,] PaintRow(int[,] matrix, int previous, int next, int position) {
            if(matrix[position, previous] == 0)
                matrix[position, previous] = 1;
            if(matrix[position, next] == 0)
                matrix[position, next] = 1;
            return matrix;
        }
        
        private static int[,] PaintColumn(int[,] matrix, int previous, int next, int position) {
            if(matrix[previous, position] == 0)
                matrix[previous, position] = 1;
            if(matrix[next, position] ==0)
                matrix[next, position] = 1;
            return matrix;
        }

        private static int[,] CheckEmptyCells(int[,] matrix, int dimension,List<List<int>>list) //This function checks if a line filled all the spaces and mark the
        {                                                    //cells that never will be filled
            var i = 0;
            foreach (var actual in list) {
                if (actual.Count == 1) {
                    if (actual[0] == NumberOfCells(matrix, dimension, i))
                        matrix=FillEmptyCells(matrix, dimension, i);    
                }
                i++;
            }
            return matrix;
        }
        private static int NumberOfCells (int[,] matrix, int dimension, int line) {
            var aux = 0;
            var length = matrix.GetLength(dimension);
            for (var i = 0; i < length; i++)
            {
                if (dimension == 0) {//Counts how many cells have been filled
                    if (matrix[line, i] == 1)
                        aux += 1;
                }
                else {
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

        private static void FullSeparatedClues(int[,] matrix,List<List<int>>list,int dimension)
        {
            var dimensionLenght = matrix.GetLength(dimension);
            
            foreach (var sublist in list)
            {
                var totalCells = sublist.Sum()+sublist.Count-1;
                if (dimensionLenght == totalCells)
                    matrix=FullSeparatedCluesAux(matrix,new List<int>(sublist) , dimension,list.IndexOf(sublist));
            }
        }

        private static int[,] FullSeparatedCluesAux(int[,] matrix, List<int> list,int dimension,int line)
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
                if (auxList[pos] != 0) continue;
                pos++;
                i++;
                if (i >= matrix.GetLength(dimension)) continue;
                if (dimension == 0) {
                    matrix[line, i] = 2;
                }else {
                    matrix[i, line] = 2;
                }

            }
            /*Console.WriteLine("-----------------------------");
            Print2DArray(matrix);
            Console.WriteLine("-----------------------------");*/
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
            bool flag = false;//The flag controls that there are not separated 1 when the clue is 1
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
                            flag = true;
                        }
                        else
                        {
                            if (matrix[line, i] != 1 && flag)
                                return false;
                        }
                        
                    }
                    else {
                        if (matrix[i, line] == 1)
                        {
                            CloneList[listPos]--;
                            flag = true;
                        }else
                        {
                            if (matrix[i, line] != 1 && flag)
                                return false;
                        }
                    }
                    if (CloneList[listPos] == 0) {
                        nextEmpty = true;
                        flag = false;
                        //Console.WriteLine("Cambio");
                        listPos++;
                    }
                    if (listPos == clues.Count)
                        break;
                }
            }
            return EmptyList(CloneList);
        }
        
        public static bool ValidOption(int[,] matrix,List<int> cluesY,int x) {
            var cloneList=new List<int>(cluesY);
            var cont = 0;
            for (var i = 0; i < matrix.GetLength(1); i++) {
                if (matrix[x, i] != 1) continue;
                cont++;
            }
            return cont <= cluesY.Sum();
        }

        private static bool NonogramSolved(int[,] matrix,List<List<int>> rowList, List<List<int>> columnList)
        {
            //PrintListOfList(rowList);
            for (var i = 0; i < matrix.GetLength(0); i++) {
                List<int> aux=new List<int>(rowList[i]);
                if (LineDone(CloneMatrix(matrix), 0, i, rowList[i])==false)
                    return false;
            }
            for (var i = 0; i < matrix.GetLength(1); i++) {
                var aux=new List<int>(columnList[i]);

                if (LineDone(matrix, 1, i, aux) == false)
                    return false;
            }
            return true;
        }
        
        
        //Method that checks and blocks a line in case is solved
        private static void CheckLines(int[,] matrix,List<int> positions,List<int> columnList) {
            foreach (var variable in positions.Where(variable => LineDone(matrix,1,variable,columnList))) {
                BlockLine(matrix,1,variable);
            }
        }

        private static bool ValidCombination(int[,] matrix,int row,List<int> positions) {
            return positions.All(variable => matrix[row, variable] == 0);
        }

        private static void CheckFullLines(int[,] matrix, List<List<int>> rowList, List<List<int>> columnList){
            for (var i = 0; i < rowList.Count; i++) {
                var total = rowList[i].Sum();
                if (total == CheckFullLinesAux(matrix,i, 0)) 
                    FillFinishedLine(matrix, i, 0);
            }
            for (var i = 0; i < columnList.Count; i++) {
                var total = columnList[i].Sum();
                if (total == CheckFullLinesAux(matrix,i, 1)) 
                    FillFinishedLine(matrix, i, 1);
            }
        }

        private static int CheckFullLinesAux(int[,] matrix,int line, int dimension) {
            var cont = 0;
            for (var i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension==0) {
                    if (matrix[line, i] == 1)
                        cont++;
                }else {
                    if (matrix[i, line] == 1)
                        cont++;
                }
            }
            return cont;
        }

        private static int[,] FillFinishedLine(int[,] matrix,int line,int dimension) {
            for (var i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension==0) {
                    if (matrix[line, i] != 1)
                        matrix[line, i] = 2;
                }else {
                    if (matrix[i, line] != 1)
                        matrix[i, line] = 2;
                }
            }
            return matrix;
        }


        private static void ClueBiggerThanSpace(int[,] matrix , List<List<int>> rowList, List<List<int>> columnList)
        {
            
            //Console.WriteLine("Linea: {0}",j);
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                if (rowList[i].Count != 1) continue;
                var contL = 0;
                    
                bool flagL;
                bool flagR;

                var contR = 0;
                flagL = true;
                flagR = true;
                var posR = matrix.GetLength(0)-1;
                for (var k = 0; k < matrix.GetLength(1); k++) {
                    if (matrix[i, k] != 2 && flagL) 
                        contL++;
                    else
                        flagL = false;
                    if (matrix[i,posR]!=2 && flagR)
                        contR++;
                    else 
                        flagR=false;
                    posR--;
                }
                if (contL < rowList[i].Sum())
                    matrix=BlockLeftUp(matrix, i, 0);
                if(contR<rowList[i].Sum())
                    matrix=BlockRightDown(matrix, i, matrix.GetLength(0)-1, 0);
            }
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                if (columnList[i].Count != 1) continue;
                var contU = 0;
                var contD = 0;
                var flagU = true;
                var flagD = true;
                var posD = matrix.GetLength(1)-1;
                for (var k = 0; k < matrix.GetLength(1); k++) {
                    if (matrix[k, i] != 2 && flagU) 
                        contU++;
                    else
                        flagU = false;
                    if (matrix[posD,i]!=2 && flagD)
                        contD++;
                    else 
                        flagD=false;
                    posD--;
                }
                if (contU < columnList[i].Sum())
                    matrix=BlockLeftUp(matrix, i,1);
                if(contD<columnList[i].Sum())
                    matrix=BlockRightDown(matrix, i, matrix.GetLength(1)-1, 1);
            }

        }

        private static int[,] BlockLeftUp(int[,] matrix, int line,int dimension)
        {
            for (var i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension==0) {
                    if (matrix[line, i] != 2)
                        matrix[line, i] = 2;
                    else
                        break;
                }
                else
                {
                    if (matrix[i, line] != 2) 
                        matrix[i, line] = 2;
                    else
                        break;
                }
            }
            return matrix;
        }

        private static int[,] BlockRightDown(int[,] matrix, int line, int pos,int dimension) {
            
            for (var i = 0; i < matrix.GetLength(dimension); i++) {
                if (dimension==0) {
                    if (matrix[line,pos] != 2) 
                        matrix[line,pos] = 2;
                    else
                        break;
                }else {
                    if (matrix[pos, line] != 2)
                        matrix[pos, line] = 2;
                    else
                        break;
                }
                pos--;
            }
            return matrix;
        }


        private static void Backtracking(int row,int[,] matrix,List<List<int>>[] combinationList
            ,List<List<int>> rowList, List<List<int>> columnList) {
            if (row > combinationList.Length-1) //Doesn't have solution
                return;
            if (combinationList[row] == null)//Go to the next row if the current one is empty
                Backtracking(row+1,matrix,combinationList,rowList,columnList);
            
            for (var i = 0; i < combinationList[row].Count; i++) {
                
                if (ValidCombination(matrix, row, combinationList[row][i]) &&(Solved==false)) {
                    var auxMatrix = CreateAuxMatrix(matrix, combinationList[row][i], row);
                    CheckLines(auxMatrix,combinationList[row][i],columnList[row]);
                    if (row == combinationList.Length-1) {
                        if (NonogramSolved(auxMatrix, rowList, columnList)) {
                            FinalNonogram = auxMatrix;
                            Solved = true;
                            break;
                        }
                    }
                    Backtracking(row+1,auxMatrix,combinationList,rowList,columnList);            
                }
                CleanCombination(matrix,combinationList[row][i],row);
            }
            
        }

        public static void StartExe(int[,] matrix, List<List<int>> rowList, List<List<int>> columnList)
        {

            //--------------------------------------     PODA METHODS    --------------------------------------------
            FillFixedFields(matrix, rowList, columnList); //Cells that always will be 1
            CheckEmptyCells(matrix, 0, rowList); //Look for cells that will never be filled, number 2 is used
            CheckEmptyCells(matrix, 1, columnList);
            FullSeparatedClues(matrix, rowList, 0); //Check when a line with morte than 1 clue can be fulled
            FullSeparatedClues(matrix, columnList, 1);
            CheckFullLines(matrix, rowList, columnList); //Check the matrix 1 last time before backtracking
            ClueBiggerThanSpace(matrix, rowList, columnList);
            UnreachableCells(matrix,rowList);

            //Things that are going to be used in bactracking method
            //var zeroCells = ZeroCells(matrix); //Contains all the positions that have 0
            //Print2DArray(matrix);
            //List<int>[] backtrackingArray = new List<int>[remainingCells];
            var zeroCells = new List<int>[matrix.GetLength(0)]; // Create an array of list
            InitializeArrayOfList(zeroCells);
            zeroCells = ZeroCells(matrix, zeroCells); //that will contain all the cells with 0

            var remainingAmount = new int[matrix.GetLength(0)];
            RemainingAmount(matrix, remainingAmount, rowList);

            var combinationList = new List<List<int>>[matrix.GetLength(1)];
            InitializeCombinationList(combinationList);
            
            CreateCombinations(zeroCells, remainingAmount, combinationList,matrix);
            
            //PrintListOfList(combinationList[1]);
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Backtracking(0,matrix,combinationList,rowList,columnList);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            
            Console.WriteLine("Duración: {0}",elapsedMs );
            
        }
    }
}