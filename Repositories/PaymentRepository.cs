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
        public List<Payment> GetAllPayments()
        {
            List<Payment> payments = new List<Payment>();

            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Payments";

            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Payment payment = new Payment(
                    Convert.ToInt32(reader["PaymentId"]),
                    Convert.ToInt32(reader["BorrowingId"]),
                    Convert.ToDecimal(reader["Amount"]),
                    Convert.ToDateTime(reader["PaymentDate"]),
                    reader["PaymentMethod"].ToString()!,
                    reader["PaymentStatus"].ToString()!
                );

                payments.Add(payment);
            }

            return payments;
        }
        public Payment? GetPaymentById(int id)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Payments WHERE PaymentId = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Payment(
                    Convert.ToInt32(reader["PaymentId"]),
                    Convert.ToInt32(reader["BorrowingId"]),
                    Convert.ToDecimal(reader["Amount"]),
                    Convert.ToDateTime(reader["PaymentDate"]),
                    reader["PaymentMethod"].ToString()!,
                    reader["PaymentStatus"].ToString()!
                );
            }

            return null;
        }
        public void UpdatePaymentStatus(int paymentId, string status)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = @"UPDATE Payments
                             SET PaymentStatus = @Status
                             WHERE PaymentId = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Id", paymentId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
