namespace MIS221_Starter_Code
{
    internal class Transaction
    {
        private int transactionID;
        private string customerEmail;
        private int movieID;
        private string rentDate;
        private string returnDate;

        //default constructor
        public Transaction()
        {

        }

        //constructor
        public Transaction(int transactionID, string customerEmail, int movieID, string rentDate)
        {
            this.transactionID = transactionID;
            this.customerEmail = customerEmail;
            this.movieID = movieID;
            this.rentDate = rentDate;
            returnDate = "Not Returned";
        }

        //constructor
        public Transaction(int transactionID, string customerEmail, int movieID, string rentDate, string returnDate)
        {
            this.transactionID = transactionID;
            this.customerEmail = customerEmail;
            this.movieID = movieID;
            this.rentDate = rentDate;
            this.returnDate = returnDate;
        }

        //returns transaction id
        public int GetTransID()
        {
            return transactionID;
        }

        //returns email
        public string GetEmail()
        {
            return customerEmail;
        }

        //Returns movie id
        public int GetMovieID()
        {
            return movieID;
        }

        //returns rent date
        public string GetRentDate()
        {
            return rentDate;
        }

        //returns return date
        public string GetReturnDate()
        {
            return returnDate;
        }

        //sets return date
        public void SetReturnDate(string date)
        {
            returnDate = date;
        }

        //returns # delineated string
        public override string ToString()
        {
            return transactionID + "#" + customerEmail + "#" + movieID + "#" + rentDate + "#" + returnDate;
        }

        //returns readable string
        public string ReadString()
        {
            return "Transaction ID: " + transactionID + "\t| Email: " + customerEmail + "\t\t| Rented: " + rentDate + " | Movie ID: " + movieID + "\t| Title: " + Program.GetMovie(movieID).GetTitle();
            ;
        }
    }
}