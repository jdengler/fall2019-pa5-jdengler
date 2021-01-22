using System;
using System.Linq;

namespace MIS221_Starter_Code
{
    internal class Reports
    {
        //Reports main
        public static void OpenReports()
        {
            while (true)
            {
                Console.Clear();
                int chc = ReportMenu();
                switch (chc)
                {
                    case 1: StockedMovies(); break;
                    case 2: RentedMovies(); break;
                    case 3: Top5Rented(); break;
                    case 4: RentedGenresCount(); break;
                    case 5: Program.Main();break;
                }
                ReturnToMenu();
            }
        }

        //Displays menu options
        public static int ReportMenu()
        {
            Console.WriteLine("\nREPORT MENU\n");

            int choice = 0;
            while(choice < 1 || choice > 5)
            {
                Console.WriteLine("\t(1) View In Stock Movies");
                Console.WriteLine("\t(2) View Movies Currently Rented");
                Console.WriteLine("\t(3) View Most Rented Movies");
                Console.WriteLine("\t(4) View Amount Rented By Genre");
                Console.WriteLine("\t(5) Return to Main Menu");
                choice = Program.ValidInput(5);
            }
            return choice;
        }

        //Prints movies in stock
        public static void StockedMovies()
        {
            Console.Clear();
            Console.WriteLine("\nIN STOCK MOVIES\n");

            MovieFile mf = new MovieFile();
            Movie[] inv = mf.ReadFile();

            for(int i = 0; i < mf.GetCount(); i++)
            {
                if (inv[i].GetStock())
                    Console.WriteLine(inv[i].ReadString());
            }
        }

        //Prints rented movies
        public static void RentedMovies()
        {
            Console.Clear();
            Console.WriteLine("\nRENTED MOVIES\n");

            TransactionFile tf = new TransactionFile();
            Transaction[] trans = tf.ReadFile();

            for (int i = 0; i < tf.GetCount(); i++)
            {
                if (trans[i].GetReturnDate() == "Not Returned")
                    Console.WriteLine(trans[i].ReadString());
            }
        }

        //Process transaction file and counts number of rentals for each movie and stores that count in a 2d array along with respective movie id
        //Then bubble sorts the 2d array and prints top 5
        public static void Top5Rented()
        {
            Console.Clear();
            Console.WriteLine("\nTOP 5 RENTED MOVIES\n");

            TransactionFile tf = new TransactionFile();
            Transaction[] trans = tf.ReadFile();

            int[,] counts = new int[tf.GetCount(), 2];
            for (int i = 0; i < tf.GetCount(); i++)
            {
                counts[trans[i].GetMovieID(), 0]++;
                counts[trans[i].GetMovieID(), 1] = trans[i].GetMovieID();
            }

            //Bubble sorts
            for (int i = 0; i < tf.GetCount(); i++)
            {
                for (int j = 0; j < tf.GetCount() - 1; j++)
                {
                    if (counts[j, 0] < counts[j + 1, 0])
                    {
                        int[,] temp = { { counts[j, 0], counts[j, 1] } };
                        counts[j, 0] = counts[j + 1, 0];
                        counts[j, 1] = counts[j + 1, 1];

                        counts[j + 1, 0] = temp[0, 0];
                        counts[j + 1, 1] = temp[0, 1];
                    }
                }
            }

            //prints top 5
            for (int i = 0; i < 5; i++)
            {
                if(counts[i, 0] != 0)
                    Console.WriteLine($"#{i+1}\t\tID: {counts[i, 1]}\t|\tCount: {counts[i,0]}\t|\t{Program.GetMovie(counts[i, 1]).GetTitle()}");
            }
        }

        //Sequential sorts the transactions by genre then processes transactions again keeping count and printing count by genre
        public static void RentedGenresCount()
        {
            String[] GENRES = { "Action", "Family", "Horror", "Sci-Fi", "Comedy", "Other" };

            Console.Clear();
            Console.WriteLine("\nRENT COUNT BY GENRE\n");

            TransactionFile tf = new TransactionFile();
            Transaction[] trans = tf.ReadFile();
            Transaction tempTran;

            Movie tempMovie = Program.GetMovie(trans[0].GetMovieID());
            Movie tempMovie2 = Program.GetMovie(trans[1].GetMovieID());
            String holdGenre = tempMovie.GetGenre();
            String currGenre = tempMovie2.GetGenre();

            //Bubble sorts transactions
            for (int i = 0; i < tf.GetCount(); i++)
            {
                tempMovie = Program.GetMovie(trans[i].GetMovieID());
                holdGenre = tempMovie.GetGenre();

                for (int j = i + 1; j < tf.GetCount(); j++)
                {
                    tempMovie2 = Program.GetMovie(trans[j].GetMovieID());
                    currGenre = tempMovie2.GetGenre();

                    if (holdGenre.CompareTo(currGenre) > 0)
                    {
                        tempTran = trans[j];
                        trans[j] = trans[i];
                        trans[i] = tempTran;

                        tempMovie = Program.GetMovie(trans[i].GetMovieID());
                        holdGenre = tempMovie.GetGenre();
                    }
                }
            }

            //Prints genre count
            tempMovie = Program.GetMovie(trans[0].GetMovieID());
            currGenre = tempMovie.GetGenre();
            int count = 0;
            Console.WriteLine("Genre \t| Count");
            for (int i = 0; i < tf.GetCount(); i++)
            {
                tempMovie = Program.GetMovie(trans[i].GetMovieID());

                if(currGenre == tempMovie.GetGenre())
                {
                    count++;
                }
                else
                {
                    Console.WriteLine($"{currGenre} \t| {count}");

                    tempMovie = Program.GetMovie(trans[i].GetMovieID());
                    currGenre = tempMovie.GetGenre();
                    count = 0;
                }
            }
            Console.WriteLine($"{currGenre} \t| {count}");
        }

        //Displays new menu options and opens selected movie;
        public static void ReturnToMenu()
        {
            int choice = 0;
            while (choice < 1 || choice > 3)
            {
                Console.WriteLine("\n\t(1) Return to Report Menu");
                Console.WriteLine("\t(2) Return to Main Menu");
                Console.WriteLine("\t(3) Exit Program");
                choice = Program.ValidInput(3);
            }

            switch(choice)
            {
                case 1: OpenReports(); break;
                case 2: Program.Main(); break;
                case 3: System.Environment.Exit(1); break;

            }
        }
    }
}