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

    }
}
