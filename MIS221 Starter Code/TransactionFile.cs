using System;
using System.IO;
using System.Linq;

namespace MIS221_Starter_Code
{
    internal class TransactionFile
    {
        private readonly String fileName = "transactions.txt";
        private int transactionID;
        private int transactionCount;

        //default constructor
        public TransactionFile()
        {
            transactionID = 0;
            transactionCount = 0;
        }

        //returns transaction id
        public int GetID()
        {
            return transactionID;
        }

        //increments transaction id
        public void IncrementID()
        {
            transactionID++;
        }

        //returns count
        public int GetCount()
        {
            return transactionCount;
        }

        //increments count
        public void IncrementCount()
        {
            transactionCount++;
        }

        //reads file and returns transaction array
        public Transaction[] ReadFile()
        {
            transactionCount = 0;
            transactionID = 0;

            if (!File.Exists(fileName))
                File.Create(fileName);

            int count = 0;
            int lineCount = File.ReadLines(@fileName).Count();


            String[] lines = new String[lineCount];
            Transaction[] transactions = new Transaction[lineCount];

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
                Transaction currTrans = new Transaction(int.Parse(delin[0]), delin[1], int.Parse(delin[2]), delin[3], delin[4]);
                transactions[count++] = currTrans;
                IncrementCount();
                IncrementID();
            }
            return transactions;
        }

        //writes file from transaction array parameter
        public void WriteFile(Transaction[] transactions)
        {
            StreamWriter sw = new StreamWriter(File.Create(fileName));

            //splits data to code from file into each line from arraylist
            for (int i = 0; i < transactionCount; i++)
            {
                String currLine = transactions[i].ToString();
                sw.WriteLine(currLine);
            }
            sw.Close();
            return;
        }
    }
}