using System;
using System.IO;
using System.Linq;

namespace MIS221_Starter_Code
{
    internal class CustomerMenu
    {
        //CustomerMenu main
        public static void Run()
        {
            Console.Clear();
            int choice = Menu();
            switch (choice)
            {
                case 1: ViewAvailableMovies(); break;
                case 2: RentMovie(); break;
                case 3: ViewRentedMovies(); break;
                case 4: ReturnMovie(); break;
                case 5: Program.Main(); break;
            }
            ReturnToMenu();
        }

        //Displays menu and options
        public static int Menu()
        {
            Console.WriteLine("\nCUSTOMER MENU\n");

            int choice = 0;
            while (choice < 1 || choice > 5)
            {
                Console.WriteLine("\t(1) View Available Movies");
                Console.WriteLine("\t(2) Rent a Movie");
                Console.WriteLine("\t(3) View Rented Movies");
                Console.WriteLine("\t(4) Return Rented Movie");
                Console.WriteLine("\t(5) Return to main menu");
                choice = Program.ValidInput(5);
            }
            return choice;
        }

        //Prints available movies
        public static void ViewAvailableMovies()
        {
            Console.WriteLine("\nAVAILABLE MOVIES\n");
            Console.WriteLine("\tMovie ID\t|\tTitle\t\t\t|\tGenre\t\t|\tRelease Year");
            Console.WriteLine("\t--------------------------------------------------------------------------------------------");
            MovieFile mf = new MovieFile();
            Movie[] movieInventory = mf.ReadFile();
            for(int i = 0; i < movieInventory.Length; i++)
            {
                if(movieInventory[i].GetStock())
                {
                    if(movieInventory[i].GetTitle().Length < 9)
                        Console.WriteLine($"\t{movieInventory[i].GetMovieID()}\t\t|\t{movieInventory[i].GetTitle()}\t\t\t|\t{movieInventory[i].GetGenre()}\t\t|\t{movieInventory[i].GetReleaseYear()}");
                    else
                        Console.WriteLine($"\t{movieInventory[i].GetMovieID()}\t\t|\t{movieInventory[i].GetTitle()}\t\t|\t{movieInventory[i].GetGenre()}\t\t|\t{movieInventory[i].GetReleaseYear()}");
                }
            }
        }

        //Prompts user for id and rents movie. Updates inventory and adds to transactions
        public static void RentMovie()
        {
            Console.Clear();
            Console.WriteLine("\nRENT MOVIE\n");

            String input;
            int id;
            int index = -1;
            TransactionFile tf = new TransactionFile();
            MovieFile mf = new MovieFile();
            Movie[] movieInventory = mf.ReadFile();
            ViewAvailableMovies();

            Console.WriteLine("\nEnter email address: ");
            String email = Console.ReadLine();

            while (index == -1)
            {
                Console.WriteLine("\nEnter movie ID to rent:\t(Enter 0 to input movie title instead)");
                input = Console.ReadLine();

                if (input.All(char.IsNumber))
                {
                    id = int.Parse(input);

                    if (id == 0)
                    {
                        Console.WriteLine("\nEnter movie title to rent: ");
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

                if(index != -1)
                {
                    if(movieInventory[index].GetStock() == false)
                    {
                        Console.WriteLine("\tError: Selected movie is unavailable. Please select a different movie.");
                        index = -1;
                    }
                }
            }
            movieInventory[index].SetStock();
            mf.WriteFile(movieInventory);

            String[] dateDelin = DateTime.Now.ToString().Split(' ');
            String rentDate = dateDelin[0];
            Transaction[] transactions = tf.ReadFile();
            tf.IncrementCount();

            Transaction newTransaction = new Transaction(tf.GetID(), email, movieInventory[index].GetMovieID(), rentDate);
            int count = tf.GetCount();
            Transaction[] updatedTransactions = new Transaction[count];

            for (int i = 0; i < count - 1; i++)
            {
                updatedTransactions[i] = transactions[i];
            }
            updatedTransactions[updatedTransactions.Length - 1] = newTransaction;

            Console.WriteLine($"\nRented movie: {movieInventory[index].ReadString()}");

            tf.WriteFile(updatedTransactions);
        }

        //Prompts user for email and prints each movie rented by that email
        public static string ViewRentedMovies()
        { 
            
            Console.WriteLine("\nEnter email address: ");
            String email = Console.ReadLine();

            Console.WriteLine("\nRENTED MOVIES\n");
            Console.WriteLine("\n\tTransaction ID\t|\tMovie ID\t|\tDate Rented|\tTitle\t");
            Console.WriteLine("\t-------------------------------------------------------------------------------------");

            TransactionFile tf = new TransactionFile();
            Transaction[] transList = tf.ReadFile();
            MovieFile mf = new MovieFile();
            Movie[] movieInv = mf.ReadFile();

            for (int i = 0; i < transList.Length; i++)
            {
                if(transList[i].GetEmail() == email)
                {
                    if(transList[i].GetReturnDate() == "Not Returned")
                    {
                        string rented = transList[i].GetRentDate();
                        int transID = transList[i].GetTransID();

                        int movieID = transList[i].GetMovieID();
                        int index = Program.GetMovieIndex(movieInv, movieID);
                        string title = movieInv[index].GetTitle();

                        string space = "\t\t|\t";
                        Console.WriteLine($"\t{transID}{space}{movieID}{space}{rented}{space}{title}");
                    }
                }
            }
            return email;
        }

        //Prompts suer for id and returns movie. Updates inventory and adds return date to transactions
        public static void ReturnMovie()
        {
            Console.Clear();
            Console.WriteLine("\nRETURN MOVIE\n");

            TransactionFile tf = new TransactionFile();
            Transaction[] transList = tf.ReadFile();
            string email = ViewRentedMovies();

            int index = -1;
            int id = -1;
            while(index == -1)
            {
                Console.WriteLine("\nEnter movie ID of movie to return: ");
                string input = Console.ReadLine();

                if (input.All(char.IsNumber))
                {
                    id = int.Parse(input);
                    index = GetTransactionIndex(transList, id);

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
            String[] dateDelin = DateTime.Now.ToString().Split(' ');
            String returnDate = dateDelin[0];
            transList[index].SetReturnDate(returnDate);
            tf.WriteFile(transList);

            MovieFile mf = new MovieFile();
            Movie[] inv = mf.ReadFile();
            index = Program.GetMovieIndex(inv, id);
            inv[index].SetStock();
            Console.WriteLine($"\nReturned movie: {inv[index].ReadString()}");
            mf.WriteFile(inv);
        }

        //Returns index of movieID in transactions
        public static int GetTransactionIndex(Transaction[] transactions, int id)
        {
            for (int i = 0; i < transactions.Length; i++)
            {
                if (transactions[i].GetMovieID() == id)
                {
                    return i;
                }
            }
            return -1;
        }

        //Displays new menu options and opens selected movie;
        public static void ReturnToMenu()
        {
            int choice = 0;
            while (choice < 1 || choice > 3)
            {
                Console.WriteLine("\n\t(1) Return to Customer Menu");
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
 