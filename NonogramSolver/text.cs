using System.Collections.Generic;
using static NonogramSolver.utilities;

namespace NonogramSolver
{
    public static class Text
    {
        public static void ReadFile(string path,List<List<int>> rowList,List<List<int>> columnList)
        {
            string line;
            var orientation = "FILAS";
            
            // Read the file and display it line by line.  
            System.IO.StreamReader file =   
                new System.IO.StreamReader(@path);  
            while((line = file.ReadLine()) != null)
            {
                if ((line == "FILAS") || (line == "COLUMNAS"))
                    orientation = line;
                else 
                {
                    var aux = ConvertToList(line);
                    if(orientation=="FILAS")
                        rowList.Add(aux);
                    else
                        columnList.Add(aux);
                }
            }
            file.Close();
        }
    }
}