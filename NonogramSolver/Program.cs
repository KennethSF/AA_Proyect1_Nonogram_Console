using System.Collections.Generic;
using static  NonogramSolver.Text;
using static NonogramSolver.utilities;
using static NonogramSolver.NonogramFiller;

namespace NonogramSolver
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //Creation of the list that are going to have the ammount of color cells in the column and row
            List<List<int>> rowList = new List<List<int>>();
            List<List<int>> columnList = new List<List<int>>();
            //Path that contains the necessary data to start the game
            const string path =
                @"C:\Users\Kenneth SF\OneDrive - Estudiantes ITCR\TEC\2020\I Semestre\Análisis de Algoritmos\Proyectos\Proyecto_1\data.txt";
            //Reads the file and converts it into two list, one for the rows and the other for the columns
            ReadFile(path, rowList, columnList);
            var nonogramMatrix = new int[rowList.Count,columnList.Count ];
            FillFixedFields(nonogramMatrix,rowList,columnList);
            PrintMaxtrix(nonogramMatrix); 
        }
    }
}
