using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class PaymentService
    {
        private PaymentRepository paymentRepository =
            new PaymentRepository();

        private BorrowingRepository borrowingRepository =
            new BorrowingRepository();

        public decimal CalculateFine(int borrowingId)
        {
            Borrowing? borrowing =
                borrowingRepository.GetBorrowingById(borrowingId);

            if (borrowing == null)
            {
                throw new Exception("Borrowing record not found.");
            }

            DateTime endDate = borrowing.ReturnDate ?? DateTime.Today;

            int lateDays = (endDate.Date - borrowing.DueDate.Date).Days;

            if (lateDays <= 0)
            {
                return 0;
            }

            return lateDays * 10;
        }

        public void MakePayment(Payment payment)
        {
            if (payment.BorrowingId <= 0)
            {
                throw new Exception("Invalid borrowing ID.");
            }

            if (payment.Amount <= 0)
            {
                throw new Exception("Payment amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(payment.PaymentMethod))
            {
                throw new Exception("Payment method cannot be empty.");
            }

            Borrowing? borrowing =
                borrowingRepository.GetBorrowingById(payment.BorrowingId);

            if (borrowing == null)
            {
                throw new Exception("Borrowing record not found.");
            }

            decimal fine = CalculateFine(payment.BorrowingId);

            if (payment.Amount < fine)
            {
                throw new Exception("Payment amount is less than the fine.");
            }

            payment.PaymentDate = DateTime.Today;
            payment.PaymentStatus = "Paid";

            paymentRepository.AddPayment(payment);
        }

        public List<Payment> GetAllPayments()
        {
            return paymentRepository.GetAllPayments();
        }

        public Payment? GetPaymentById(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Payment ID must be greater than zero.");
            }

            return paymentRepository.GetPaymentById(id);
        }
    }
}
