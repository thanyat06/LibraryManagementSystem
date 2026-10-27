using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class PaymentRepository
    {
        private string connectionString =
            @"Server=(local)\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public void AddPayment(Payment payment)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = @"INSERT INTO Payments
                (BorrowingId, Amount, PaymentDate,
                 PaymentMethod, PaymentStatus)
                VALUES
                (@BorrowingId, @Amount, @PaymentDate,
                 @PaymentMethod, @PaymentStatus)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@BorrowingId", payment.BorrowingId);
            cmd.Parameters.AddWithValue("@Amount", payment.Amount);
            cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate.Date);
            cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
            cmd.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
