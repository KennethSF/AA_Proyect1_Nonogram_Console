using System;
using System.Collections.Generic;
using static  NonogramSolver.Text;
using static NonogramSolver.utilities;
using static NonogramSolver.NonogramFiller;

namespace NonogramSolver
{
    
    class Program
    {
        public static int[,] FinalNonogram;
        public static int[,] AuxNonogram;
        public static int NumSteps=0;
        public static bool Solved = false;
        
        //Creation of the list that are going to have the ammount of color cells in the column and row
        public static List<List<int>> rowList = new List<List<int>>();
        public static List<List<int>> columnList = new List<List<int>>();
        public static void Main(string[] args)
        {
            
            //Boolean lists that controls when a line of the nonogram is done
            List<bool> rowDone= new List<bool>();
            List<bool> columnDone= new List<bool>();
            //Path that contains the necessary data to start the game
            const string path =
                @"C:\Users\Kenneth SF\OneDrive - Estudiantes ITCR\TEC\2020\I Semestre\Análisis de Algoritmos\Proyectos\Proyecto_1\10x10_4.txt";
            //Reads the file and converts it into two list, one for the rows and the other for the columns
            ReadFile(path, rowList, columnList);
            var nonogramMatrix = new int[rowList.Count,columnList.Count ];
            FinalNonogram=new int[rowList.Count,columnList.Count ];
            AuxNonogram=new int[rowList.Count,columnList.Count ];
            rowDone = SolvedLineList(rowDone,rowList.Count);
            columnDone = SolvedLineList(columnDone, columnList.Count);
            StartExe(nonogramMatrix,rowList,columnList);
            
            Console.WriteLine();
            CleanMatrix(FinalNonogram);
        }
    }
}
