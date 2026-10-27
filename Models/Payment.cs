using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BorrowingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public Payment(int paymentId, int borrowingId,
                       decimal amount, DateTime paymentDate,
                       string paymentMethod, string paymentStatus)
        {
            PaymentId = paymentId;
            BorrowingId = borrowingId;
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
        }
    }
}
