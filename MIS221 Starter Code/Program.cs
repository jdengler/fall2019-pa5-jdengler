using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS221_Starter_Code
{
    class Program
    {
        //program main
        public static void Main()
        {
            Console.Clear();
            int choice = Menu();
            switch (choice)
            {
                case 1: ManagerMenu.Run(); break;
                case 2: CustomerMenu.Run(); break;
                case 3: Reports.OpenReports(); break;
                case 4: System.Environment.Exit(1); break;
            }
        }

        //Displays menu and options
        public static int Menu()
        {
            Console.WriteLine("\nMAIN MENU\n");

            int chc = 0;
            while (chc < 1 || chc > 4)
            {
                Console.WriteLine("\t(1) Enter Manager Menu");
                Console.WriteLine("\t(2) Enter Customer Menu");
                Console.WriteLine("\t(3) View Reports");
                Console.WriteLine("\t(4) Exit Program");
                Console.WriteLine("\nEnter number to select option: ");
                String input = Console.ReadLine();

                if (input.All(char.IsNumber))
                {
                    chc = int.Parse(input);

                    if (chc < 1 || chc > 4)
                    {
                        Console.WriteLine("Error: Enter value between 1 and 4");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Enter only numeric values");
                }
            }
            return chc;
        }

        //Reads through movie inventory and removed inventory and returns the next movie id
        public static int GetNextID()
        {
            int max = 0;

            MovieFile mf = new MovieFile();
            Movie[] inv = mf.ReadFile();
            for(int i = 0; i < inv.Length; i++)
            {
                if (inv[i].GetMovieID() > max)
                    max = inv[i].GetMovieID();
            }

            RemovedFile rf = new RemovedFile();
            Movie[] rem = rf.ReadFile();
            for (int i = 0; i < rem.Length; i++)
            {
                if (rem[i].GetMovieID() > max)
                    max = rem[i].GetMovieID();
            }

            return max + 1;
        }

        //Reads through movie inventory and removed inventory and returns movie that matches id parameter
        public static Movie GetMovie(int id)
        {
            MovieFile mf = new MovieFile();
            Movie[] inv = mf.ReadFile();
            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i].GetMovieID() == id)
                    return inv[i];
            }

            RemovedFile rf = new RemovedFile();
            Movie[] rem = rf.ReadFile();
            for (int i = 0; i < rem.Length; i++)
            {
                if (rem[i].GetMovieID() == id)
                    return rem[i];
            }

            Console.WriteLine("\tError: Movie with ID = " + id + " Not Found"); 
            Movie blankMovie = new Movie();
            return blankMovie;
        }

        //Returns index of movie with matching id parameter
        public static int GetMovieIndex(Movie[] movieInventory, int id)
        {
            for (int i = 0; i < movieInventory.Length; i++)
            {
                if (movieInventory[i].GetMovieID() == id)
                {
                    return i;
                }
            }
            return -1;
        }

        //Returns index of movie with matching title parameter
        public static int GetMovieIndex(Movie[] movieInventory, String title)
        {
            for (int i = 0; i < movieInventory.Length; i++)
            {
                if (movieInventory[i].GetTitle().CompareTo(title) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        //Removes movie from inventory and returns updated inventory
        public static Movie[] RemoveMovieFromArray(Movie[] movieInventory, Movie toBeRemoved)
        {
            int length = movieInventory.Length - 1;
            Movie[] newInventory = new Movie[length];
            int count = 0;

            for (int i = 0; i <= length; i++)
            {
                if (movieInventory[i].GetMovieID() != toBeRemoved.GetMovieID())
                {
                    newInventory[count++] = movieInventory[i];
                }
            }
            return newInventory;
        }

        //General menu option validity checker
        public static int ValidInput(int options)
        {
            Console.WriteLine("\nEnter number to select option: ");
            String input = Console.ReadLine();
            int choice = -1;

            if (input.All(char.IsNumber))
            {
                choice = int.Parse(input);

                if (choice < 1 || choice > options)
                {
                    Console.WriteLine("\tError: Enter value between 1 and " + options);
                }
            }
            else
            {
                Console.WriteLine("\tError: Enter only numeric values");
            }
            return choice;
        }
    }
}
