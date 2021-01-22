using System;
using System.IO;
using System.Linq;

namespace MIS221_Starter_Code
{
    internal class ManagerMenu
    {
        private static readonly String[] GENRES = { "Action", "Family", "Horror", "Sci-Fi", "Comedy", "Other" };

        //Main for ManagerMenu
        public static void Run()
        {
            Console.Clear();
            int choice = Menu();
            switch (choice)
            {
                case 1: AddMovie(); break;
                case 2: RemoveMovie(); break;
                case 3: EditMovie(); break;
                case 4: BatchUpdate.OpenBatch(); break;
                case 5: Reports.OpenReports(); break;
                case 6: Program.Main(); break;
            }
            ReturnToMenu();
        }

        //Displays menu and options
        public static int Menu()
        {
            Console.WriteLine("\nMANAGER MENU\n");

            int chc = 0;
            while (chc < 1 || chc > 6)
            {
                Console.WriteLine("\t(1) Add Movie");
                Console.WriteLine("\t(2) Remove Movie");
                Console.WriteLine("\t(3) Edit Movie");
                Console.WriteLine("\t(4) Process batch transaction file");
                Console.WriteLine("\t(5) Access report menu");
                Console.WriteLine("\t(6) Return to main menu");
                Console.WriteLine("\nEnter number to select option: ");
                String input = Console.ReadLine();

                if (input.All(char.IsNumber))
                {
                    chc = int.Parse(input);

                    if (chc < 1 || chc > 6)
                        Console.WriteLine("\tError: Enter value between 1 and 6");
                }
                else
                    Console.WriteLine("\tError: Enter only numeric values");
            }
            return chc;
        }

        //Prompts user for info of movie to add
        public static void AddMovie()
        {
            Console.Clear();
            Console.WriteLine("\nADD MOVIE\n");

            //Read moviefile and instantiate inventory.
            MovieFile mf = new MovieFile();
            Movie[] movieInventory = mf.ReadFile();
            mf.SetNextID();
            int id = mf.GetID();

            String title, genre, input;
            int genreChc = 0;
            int year = -1;
            bool stock = true;

            //Set title
            Console.WriteLine("\nEnter movie title: ");
            title = Console.ReadLine();

            //Set genre
            while (genreChc < 1 || genreChc > 6)
            {
                Console.WriteLine("\nEnter genre choice: ");
                Console.WriteLine("\t(1) Action");
                Console.WriteLine("\t(2) Family");
                Console.WriteLine("\t(3) Horror");
                Console.WriteLine("\t(4) Sci-Fi");
                Console.WriteLine("\t(5) Comedy");
                Console.WriteLine("\t(6) Other");

                input = Console.ReadLine();

                if (input.All(char.IsNumber))
                {
                    genreChc = int.Parse(input);

                    if (genreChc < 1 || genreChc > 6)
                        Console.WriteLine("\tError: Enter value between 1 and 6");
                }
                else
                    Console.WriteLine("\tError: Enter only numeric values");
            }
            genre = GENRES[genreChc - 1];

            //Set year
            while (year == -1)
            {
                Console.WriteLine("\nEnter movie release year: ");
                input = Console.ReadLine();

                if (input.All(char.IsNumber))
                    year = int.Parse(input);
                else
                    Console.WriteLine("\tError: Enter only numeric values");
            }

            //Creates instance of Movie with id, title, genre, year, and stock assigned.
            //Updates moviefile count +1 and creates new Movie array updatedInventory that is of size count.
            Movie newMovie = new Movie(id, title, genre, year, stock);
            String str = newMovie.ToString();
            Add(str);
        }

        //Adds movie to inventory
        public static void Add(String line)
        {
            String[] d = line.Split('#');

            bool stock;
            if (d[4] == "yes")
                stock = true;
            else
                stock = false;

            Movie movieToAdd = new Movie(int.Parse(d[0]), d[1], d[2], int.Parse(d[3]), stock);
            MovieFile mf = new MovieFile();
            Movie[] oldInv = mf.ReadFile();
            mf.IncrementCount();
            int count = mf.GetCount();
            Movie[] updatedInventory = new Movie[count];

            //Assigns updatedInventory movies to oldInv movies (thru count - 1).
            //Assigns last value of updatedInventory to movieToAdd and writes moviefile with updatedInventory.
            for (int i = 0; i < count - 1; i++)
            {
                updatedInventory[i] = oldInv[i];
            }
            updatedInventory[count - 1] = movieToAdd;
            Console.WriteLine($"\nAdded movie: {movieToAdd.ReadString()}");
            mf.WriteFile(updatedInventory);
        }

        //Prompts user for info of movie to remove
        public static void RemoveMovie()
        {
            Console.Clear();
            Console.WriteLine("\nREMOVE MOVIE\n");

            MovieFile mf = new MovieFile();
            Movie[] movieInventory = mf.ReadFile();
            mf.DecrementCount();
            int index = -1;
            while (index == -1)
            {
                index = PromptForMovie(movieInventory, "remove");
                if (!movieInventory[index].GetStock())
                {
                    Console.WriteLine("\tError: Selected movie is unavailable. Please select a different movie.");
                    index = -1;
                }
            }
            String str = movieInventory[index].ToString();
            Delete(str);
        }

        //Removes movie from inventory
        public static void Delete(String line)
        {
            MovieFile mf = new MovieFile();
            Movie[] inv = mf.ReadFile();

            int id = int.Parse(line.Split('#')[0]);
            int index = Program.GetMovieIndex(inv, id);
            inv[index].SetStock();
            Movie removie = inv[index];
            inv = Program.RemoveMovieFromArray(inv, inv[index]);
            mf.DecrementCount();
            mf.WriteFile(inv);

            RemovedFile rf = new RemovedFile();
            Movie[] oldRemoved = rf.ReadFile();
            rf.IncrementCount();
            int count = rf.GetCount();
            Movie[] updatedRemoved = new Movie[count];

            for (int i = 0; i < count - 1; i++)
            {
                updatedRemoved[i] = oldRemoved[i];
            }
            updatedRemoved[count - 1] = removie;
            Console.WriteLine($"\nRemoved movie: {removie.ReadString()}");
            rf.WriteFile(updatedRemoved);
        }

        //Prompts user for info to change on movie and edits it
        public static void EditMovie()
        {
            Console.Clear();
            Console.WriteLine("\nEDIT MOVIE\n");

            MovieFile mf = new MovieFile();
            Movie[] movieInventory = mf.ReadFile();
            int index = PromptForMovie(movieInventory, "edit");
            Movie movieToEdit = movieInventory[index];
            String input = "";

            Console.WriteLine("\t(1) Title:\t\t" + movieToEdit.GetTitle());
            Console.WriteLine("\t(2) Genre:\t\t" + movieToEdit.GetGenre());
            Console.WriteLine("\t(3) Release Year:\t" + movieToEdit.GetReleaseYear());
            Console.WriteLine("\t(4) Stock:\t\t" + movieToEdit.GetStock());

            int choice = 0;
            while (choice < 1 || choice > 4)
            {
                choice = Program.ValidInput(4);
            }

            switch(choice)
            {
                case 1:
                    Console.WriteLine("\nEnter new movie title: ");
                    input = Console.ReadLine();
                    movieToEdit.SetTitle(input);
                    break;

                case 2:
                    int genreChc = 0;
                    while (genreChc < 1 || genreChc > 6)
                    {
                        Console.WriteLine("\nEnter genre choice: \n\t(1) Action\n\t(2) Family\n\t(3) Horror\n\t(4) Sci-Fi\n\t(5) Comedy\n\t(6) Other");
                        input = Console.ReadLine();
                        genreChc = Program.ValidInput(6);
                    }
                    string genre = GENRES[genreChc - 1];
                    movieToEdit.SetGenre(genre);
                    break;

                case 3:
                    int year = -1;
                    while (year == -1)
                    {
                        Console.WriteLine("\nEnter new release year: ");
                        input = Console.ReadLine();

                        if (input.All(char.IsNumber))
                        {
                            year = int.Parse(input);
                        }
                        else
                        {
                            Console.WriteLine("\tError: Enter only numeric values");
                        }
                    }
                    movieToEdit.SetReleaseYear(year);
                    break;

                case 4:
                    movieToEdit.SetStock();
                    break;
            }

            Console.WriteLine($"\nUpdated movie: {movieToEdit.ReadString()}");
            movieInventory[index] = movieToEdit;
            mf.WriteFile(movieInventory);
        }

        //Prompts user for id or title of movie to edit or delete and returns index
        public static int PromptForMovie(Movie[] movieInventory, String removeOrEdit)
        {
            String input;
            int id;
            int index = -1;

            while (index == -1)
            {
                Console.WriteLine($"\nEnter movie ID to {removeOrEdit}:\t(Enter 0 to input movie title instead)");
                input = Console.ReadLine();

                if (input.All(char.IsNumber))
                {
                    id = int.Parse(input);

                    if (id == 0)
                    {
                        Console.WriteLine($"\nEnter movie title to {removeOrEdit}: ");
                        input = Console.ReadLine();
                        index = Program.GetMovieIndex(movieInventory, input);
                    }
                    else
                    {
                        index = Program.GetMovieIndex(movieInventory, id);
                    }

                    if (index == -1)
                    {
                        Console.WriteLine("\tError: Movie not found.");
                    }
                }
                else
                {
                    Console.WriteLine("\tError: Enter only numeric values.");
                }
            }
            return index;
        }

        //Displays new menu options and opens selected movie;
        public static void ReturnToMenu()
        {
            int choice = 0;
            while (choice < 1 || choice > 3)
            {
                Console.WriteLine("\n\t(1) Return to Manager Menu");
                Console.WriteLine("\t(2) Return to Main Menu");
                Console.WriteLine("\t(3) Exit Program");
                choice = Program.ValidInput(3);
            }

            switch (choice)
            {
                case 1: Run(); break;
                case 2: Program.Main(); break;
                case 3: System.Environment.Exit(1); break;
            }
        }
    }
}
