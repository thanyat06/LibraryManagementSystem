using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class BorrowingRepository
    {
        private string connectionString =
            @"Server=(local)\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public void AddBorrowing(Borrowing borrowing)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = @"INSERT INTO Borrowings
                (BookId, BorrowerName, BorrowerPhone,
                 BorrowDate, DueDate, ReturnDate, Status)
                VALUES
                (@BookId, @BorrowerName, @BorrowerPhone,
                 @BorrowDate, @DueDate, @ReturnDate, @Status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@BookId", borrowing.BookId);
            cmd.Parameters.AddWithValue("@BorrowerName", borrowing.BorrowerName);
            cmd.Parameters.AddWithValue("@BorrowerPhone", borrowing.BorrowerPhone);
            cmd.Parameters.AddWithValue("@BorrowDate", borrowing.BorrowDate.Date);
            cmd.Parameters.AddWithValue("@DueDate", borrowing.DueDate.Date);
            cmd.Parameters.AddWithValue("@ReturnDate",
                (object?)borrowing.ReturnDate?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", borrowing.Status);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Borrowing> GetAllBorrowings()
        {
            List<Borrowing> borrowings = new List<Borrowing>();

            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Borrowings";

            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Borrowing borrowing = new Borrowing(
                    Convert.ToInt32(reader["BorrowingId"]),
                    Convert.ToInt32(reader["BookId"]),
                    reader["BorrowerName"].ToString()!,
                    reader["BorrowerPhone"].ToString()!,
                    Convert.ToDateTime(reader["BorrowDate"]),
                    Convert.ToDateTime(reader["DueDate"]),
                    reader["ReturnDate"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["ReturnDate"]),
                    reader["Status"].ToString()!
                );

                borrowings.Add(borrowing);
            }

            return borrowings;
        }

        public Borrowing? GetBorrowingById(int id)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Borrowings WHERE BorrowingId = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Borrowing(
                    Convert.ToInt32(reader["BorrowingId"]),
                    Convert.ToInt32(reader["BookId"]),
                    reader["BorrowerName"].ToString()!,
                    reader["BorrowerPhone"].ToString()!,
                    Convert.ToDateTime(reader["BorrowDate"]),
                    Convert.ToDateTime(reader["DueDate"]),
                    reader["ReturnDate"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["ReturnDate"]),
                    reader["Status"].ToString()!
                );
            }

            return null;
        }

        public void ReturnBook(int borrowingId, DateTime returnDate)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = @"UPDATE Borrowings
                             SET ReturnDate = @ReturnDate,
                                 Status = 'Returned'
                             WHERE BorrowingId = @Id
                             AND Status = 'Borrowed'";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ReturnDate", returnDate.Date);
            cmd.Parameters.AddWithValue("@Id", borrowingId);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
