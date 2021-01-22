using System;
using System.IO;
using System.Linq;

namespace MIS221_Starter_Code
{
    internal class RemovedFile
    {
        private readonly String fileName = "removed.txt";
        private int removedCount;

        //Default constructor
        public RemovedFile()
        {
            removedCount = 0;
        }

        //Returns count
        public int GetCount()
        {
            return removedCount;
        }

        //increments count
        public void IncrementCount()
        {
            removedCount++;
        }
        
        //Reads removed file and returns movie array
        public Movie[] ReadFile()
        {
            removedCount = 0;

            if (!File.Exists(fileName))
                File.Create(fileName);

            int count = 0;
            int lineCount = File.ReadLines(@fileName).Count();

            String[] lines = new String[lineCount];
            Movie[] removed = new Movie[lineCount];

            //opens the text file using a stream reader
            using (StreamReader sr = new StreamReader(fileName))
            {
                //reads the stream to a string and adds string to array
                do
                {
                    lines[count++] = sr.ReadLine();
                }
                while (!sr.EndOfStream);
            }
            count = 0;
            foreach (String line in lines)
            {
                String[] delin = line.Split('#');

                bool stock = true;
                if (delin[4] == "yes")
                    stock = true;
                if (delin[4] == "no")
                    stock = false;

                Movie currMovie = new Movie(int.Parse(delin[0]), delin[1], delin[2], int.Parse(delin[3]), stock);
                removed[count++] = currMovie;
                IncrementCount();
            }
            return removed;
        }

        //Writes removed file from movie array parameter
        public void WriteFile(Movie[] movies)
        {
            StreamWriter sw = new StreamWriter(File.Create(fileName));

            for (int i = 0; i < removedCount; i++)
            {
                String currLine = movies[i].ToString();
                sw.WriteLine(currLine);
            }
            sw.Close();
            return;
        }
    }
}