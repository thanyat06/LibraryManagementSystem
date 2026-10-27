using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class BorrowingService
    {
        private BorrowingRepository borrowingRepository =
            new BorrowingRepository();

        private BookRepository bookRepository =
            new BookRepository();

        private string connectionString =
            @"Server=(local)\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public void BorrowBook(Borrowing borrowing)
        {
            Book? book = bookRepository.GetBookById(borrowing.BookId);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            if (book.AvailableCopies <= 0)
            {
                throw new Exception("No copies are available.");
            }

            if (string.IsNullOrWhiteSpace(borrowing.BorrowerName) ||
                string.IsNullOrWhiteSpace(borrowing.BorrowerPhone))
            {
                throw new Exception("Borrower details cannot be empty.");
            }

            if (borrowing.DueDate.Date < borrowing.BorrowDate.Date)
            {
                throw new Exception("Due date cannot be before borrow date.");
            }

            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            using SqlTransaction transaction = con.BeginTransaction();

            try
            {
                string insertQuery = @"INSERT INTO Borrowings
                    (BookId, BorrowerName, BorrowerPhone,
                     BorrowDate, DueDate, ReturnDate, Status)
                    VALUES
                    (@BookId, @Name, @Phone,
                     @BorrowDate, @DueDate, NULL, 'Borrowed')";

                using SqlCommand insertCmd =
                    new SqlCommand(insertQuery, con, transaction);

                insertCmd.Parameters.AddWithValue("@BookId", borrowing.BookId);
                insertCmd.Parameters.AddWithValue("@Name", borrowing.BorrowerName);
                insertCmd.Parameters.AddWithValue("@Phone", borrowing.BorrowerPhone);
                insertCmd.Parameters.AddWithValue("@BorrowDate", borrowing.BorrowDate.Date);
                insertCmd.Parameters.AddWithValue("@DueDate", borrowing.DueDate.Date);

                insertCmd.ExecuteNonQuery();

                string updateQuery = @"UPDATE Books
                    SET AvailableCopies = AvailableCopies - 1
                    WHERE BookId = @BookId AND AvailableCopies > 0";

                using SqlCommand updateCmd =
                    new SqlCommand(updateQuery, con, transaction);

                updateCmd.Parameters.AddWithValue("@BookId", borrowing.BookId);

                int rows = updateCmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new Exception("No copies are available.");
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<Borrowing> GetAllBorrowings()
        {
            return borrowingRepository.GetAllBorrowings();
        }

        public void ReturnBook(int borrowingId)
        {
            Borrowing? borrowing =
                borrowingRepository.GetBorrowingById(borrowingId);

            if (borrowing == null)
            {
                throw new Exception("Borrowing record not found.");
            }

            if (borrowing.Status == "Returned")
            {
                throw new Exception("This book has already been returned.");
            }

            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            using SqlTransaction transaction = con.BeginTransaction();

            try
            {
                string returnQuery = @"UPDATE Borrowings
                    SET ReturnDate = @ReturnDate, Status = 'Returned'
                    WHERE BorrowingId = @Id AND Status = 'Borrowed'";

                using SqlCommand returnCmd =
                    new SqlCommand(returnQuery, con, transaction);

                returnCmd.Parameters.AddWithValue("@ReturnDate", DateTime.Today);
                returnCmd.Parameters.AddWithValue("@Id", borrowingId);

                int rows = returnCmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new Exception("Unable to return this book.");
                }

                string bookQuery = @"UPDATE Books
                    SET AvailableCopies = AvailableCopies + 1
                    WHERE BookId = @BookId";

                using SqlCommand bookCmd =
                    new SqlCommand(bookQuery, con, transaction);

                bookCmd.Parameters.AddWithValue("@BookId", borrowing.BookId);
                bookCmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
