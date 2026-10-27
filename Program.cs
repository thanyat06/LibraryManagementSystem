using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

BookService bookService = new BookService();
BorrowingService borrowingService = new BorrowingService();
PaymentService paymentService = new PaymentService();

while (true)
{
    Console.WriteLine("\n===== LIBRARY MANAGEMENT SYSTEM =====");
    Console.WriteLine("1. Add Book");
    Console.WriteLine("2. View All Books");
    Console.WriteLine("3. Search Book");
    Console.WriteLine("4. Borrow Book");
    Console.WriteLine("5. View Borrowings");
    Console.WriteLine("6. Return Book");
    Console.WriteLine("7. Calculate Fine");
    Console.WriteLine("8. Make Payment");
    Console.WriteLine("9. View Payments");
    Console.WriteLine("10. Exit");
    Console.Write("Enter your choice: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Enter a valid number.");
        continue;
    }

    try
    {
        switch (choice)
        {
            case 1:
                Console.Write("Enter title: ");
                string title = Console.ReadLine() ?? "";

                Console.Write("Enter author: ");
                string author = Console.ReadLine() ?? "";

                Console.Write("Enter category: ");
                string category = Console.ReadLine() ?? "";

                Console.Write("Enter total copies: ");
                if (!int.TryParse(Console.ReadLine(), out int totalCopies) ||
                    totalCopies <= 0)
                {
                    Console.WriteLine("Enter a valid positive number.");
                    break;
                }

                Book book = new Book(
                    0, title, author, category,
                    totalCopies, totalCopies);

                bookService.AddBook(book);
                Console.WriteLine("Book added successfully.");
                break;

            case 2:
                foreach (Book b in bookService.GetAllBooks())
                {
                    Console.WriteLine(
                        $"{b.BookId} | {b.Title} | {b.Author} | " +
                        $"{b.Category} | Available: {b.AvailableCopies}/{b.TotalCopies}");
                }
                break;

            case 3:
                Console.Write("Enter Book ID: ");
                if (!int.TryParse(Console.ReadLine(), out int bookId))
                {
                    Console.WriteLine("Invalid Book ID.");
                    break;
                }

                Book? foundBook = bookService.GetBookById(bookId);

                if (foundBook == null)
                {
                    Console.WriteLine("Book not found.");
                }
                else
                {
                    Console.WriteLine(
                        $"{foundBook.BookId} | {foundBook.Title} | " +
                        $"{foundBook.Author} | {foundBook.Category} | " +
                        $"Available: {foundBook.AvailableCopies}");
                }
                break;

            case 4:
                Console.Write("Enter Book ID: ");
                if (!int.TryParse(Console.ReadLine(), out int borrowBookId))
                {
                    Console.WriteLine("Invalid Book ID.");
                    break;
                }

                Console.Write("Enter borrower name: ");
                string borrowerName = Console.ReadLine() ?? "";

                Console.Write("Enter phone number: ");
                string phone = Console.ReadLine() ?? "";

                Borrowing borrowing = new Borrowing(
                    0, borrowBookId, borrowerName, phone,
                    DateTime.Today, DateTime.Today.AddDays(14),
                    null, "Borrowed");

                borrowingService.BorrowBook(borrowing);
                Console.WriteLine("Book borrowed successfully.");
                break;

            case 5:
                foreach (Borrowing br in borrowingService.GetAllBorrowings())
                {
                    Console.WriteLine(
                        $"{br.BorrowingId} | Book ID: {br.BookId} | " +
                        $"{br.BorrowerName} | Due: {br.DueDate:dd-MM-yyyy} | " +
                        $"Status: {br.Status}");
                }
                break;

            case 6:
                Console.Write("Enter Borrowing ID: ");
                if (!int.TryParse(Console.ReadLine(), out int returnId))
                {
                    Console.WriteLine("Invalid Borrowing ID.");
                    break;
                }

                borrowingService.ReturnBook(returnId);
                Console.WriteLine("Book returned successfully.");
                break;

            case 7:
                Console.Write("Enter Borrowing ID: ");
                if (!int.TryParse(Console.ReadLine(), out int fineId))
                {
                    Console.WriteLine("Invalid Borrowing ID.");
                    break;
                }

                decimal fine = paymentService.CalculateFine(fineId);
                Console.WriteLine($"Fine amount: Rs. {fine:F2}");
                break;

            case 8:
                Console.Write("Enter Borrowing ID: ");
                if (!int.TryParse(Console.ReadLine(), out int paymentBorrowingId))
                {
                    Console.WriteLine("Invalid Borrowing ID.");
                    break;
                }

                decimal amount = paymentService.CalculateFine(paymentBorrowingId);

                if (amount <= 0)
                {
                    Console.WriteLine("No fine is due for this borrowing.");
                    break;
                }

                Console.WriteLine($"Fine amount: Rs. {amount:F2}");
                Console.Write("Enter payment method (Cash/UPI/Card): ");
                string method = Console.ReadLine() ?? "";

                Payment payment = new Payment(
                    0, paymentBorrowingId, amount,
                    DateTime.Today, method, "Paid");

                paymentService.MakePayment(payment);
                Console.WriteLine("Payment recorded successfully.");
                break;

            case 9:
                foreach (Payment p in paymentService.GetAllPayments())
                {
                    Console.WriteLine(
                        $"{p.PaymentId} | Borrowing ID: {p.BorrowingId} | " +
                        $"Amount: Rs. {p.Amount:F2} | " +
                        $"{p.PaymentMethod} | {p.PaymentStatus}");
                }
                break;

            case 10:
                return;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
