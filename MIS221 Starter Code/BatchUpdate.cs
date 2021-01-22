using System;
using System.IO;
using System.Linq;

namespace MIS221_Starter_Code
{
    internal class BatchUpdate
    {
        //Prompts user for batchfile name
        public static void OpenBatch()
        {
            Console.Clear();
            Console.WriteLine("PROCESS BATCH");

            bool valid = false;
            String fileName = "";

            while (!valid)
            {
                Console.WriteLine("\nEnter name of batch file to use: ");
                fileName = Console.ReadLine();
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("\tError: file not found.");
                }
                else
                    valid = true;
            }
            ReadBatch(fileName);
        }

        //Reads and runs batch and parses Add Change or Delete
        public static void ReadBatch(String fileName)
        {
            int count = 0;
            int lineCount = File.ReadLines(@fileName).Count();

            String[] lines = new String[lineCount];

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

            //Checks if batch is less than a month old
            String header = lines[0];
            String[] delin = header.Split('#');
            String[] date = delin[2].Split('/');
            DateTime now = DateTime.Now;
            DateTime tempDate = new DateTime(int.Parse(date[2]), int.Parse(date[0]), int.Parse(date[1]));
            int days = ( now - tempDate ).Days;

            if(days > 31)
            {
                Console.WriteLine("Batch is over 1 month old. Deleting batch.");
                return;
            }

            //Check if record# in trailer is accurate
            String trailer = lines[lineCount - 1];
            delin = trailer.Split('#');
            int numRecords = int.Parse(delin[3]);
            if(numRecords != lineCount)
            {
                Console.WriteLine($"Number of records ({numRecords}) != number of lines({lineCount})");
                return;
            }

            //Goes through each detail record and does appropriate action
            for (int i = 1; i < numRecords - 1; i++)
            {
                delin = lines[i].Split('#');
                String action = delin[1];
                String line = lines[i].Substring(4);

                switch(action)
                {
                    case "A": ManagerMenu.Add(line); break;
                    case "C": Change(line); break;
                    case "D": ManagerMenu.Delete(line); break;
                }
            }
        }

        //Changes line in inventory
        public static void Change(String line)
        {
            MovieFile mf = new MovieFile();
            Movie[] inv = mf.ReadFile();

            String[] delin = line.Split('#');
            int id = int.Parse(delin[0]);
            int index = Program.GetMovieIndex(inv, id);

            if (delin[1].Length > 0)
                inv[index].SetTitle(delin[1]);

            if (delin[2].Length > 0)
                inv[index].SetGenre(delin[2]);

            if (delin[3].Length > 0)
                inv[index].SetReleaseYear(int.Parse(delin[3]));

            if (delin[4].Length > 0)
                inv[index].SetStock(delin[4]);

            Console.WriteLine($"\nUpdated movie: {inv[index].ReadString()}");
            mf.WriteFile(inv);
        }
    }
}