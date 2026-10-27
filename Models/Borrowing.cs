using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementSystem.Models
{
    public class Borrowing
    {
        public int BorrowingId { get; set; }
        public int BookId { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerPhone { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }

        public Borrowing(int borrowingId, int bookId,
                         string borrowerName, string borrowerPhone,
                         DateTime borrowDate, DateTime dueDate,
                         DateTime? returnDate, string status)
        {
            BorrowingId = borrowingId;
            BookId = bookId;
            BorrowerName = borrowerName;
            BorrowerPhone = borrowerPhone;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            ReturnDate = returnDate;
            Status = status;
        }
    }
}
