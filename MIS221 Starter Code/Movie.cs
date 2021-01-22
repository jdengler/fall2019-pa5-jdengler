using System;
namespace MIS221_Starter_Code
{
    public class Movie
    {
        private int movieID;
        private string movieTitle;
        private string movieGenre;
        private int releaseYear;
        private bool inStock;

        //Default constructor
	    public Movie()
	    {

	    }

        //Constructor
        public Movie(int id, string title, string genre, int year, bool stock)
        {
            movieID = id;
            movieTitle = title;
            movieGenre = genre;
            releaseYear = year;
            inStock = stock;
        }
    
        //Returns movie id
        public int GetMovieID()
        {
            return movieID;
        }

        //Sets movie id
        public void SetMovieID(int id)
        {
            movieID = id;
        }

        //returns title
        public string GetTitle()
        {
            return movieTitle;
        }

        //sets title
        public void SetTitle(string title)
        {
            movieTitle = title;
        }

        //returns genre
        public string GetGenre()
        {
            return movieGenre;
        }

        //sets genre
        public void SetGenre(string genre)
        {
            movieGenre = genre;
        }

        //returns release year
        public int GetReleaseYear()
        {
            return releaseYear;
        }

        //sets release year
        public void SetReleaseYear(int year)
        {
            releaseYear = year;
        }

        //returns stock
        public bool GetStock()
        {
            return inStock;
        }

        //sets stock
        public void SetStock()
        {
            inStock = !inStock;
        }

        //sets stock (with string)
        public void SetStock(String str)
        {
            if (str == "yes")
                inStock = true;
            else
                inStock = false;
        }

        //returns # delineated string
        public override string ToString()
        {
            string stock;
            if (inStock)
                stock = "yes";
            else
                stock = "no";

            return movieID + "#" + movieTitle + "#" + movieGenre + "#" + releaseYear + "#" + stock;
        }

        //returns readable string
        public string ReadString()
        {
            return "ID: " + movieID + "\t|\t" + movieGenre + "\t|\t" + releaseYear + "\t|\t" + movieTitle;
        }

    }
}
