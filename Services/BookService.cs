using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private BookRepository repository = new BookRepository();

        public void AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) ||
                string.IsNullOrWhiteSpace(book.Author) ||
                string.IsNullOrWhiteSpace(book.Category))
            {
                throw new Exception("Book details cannot be empty.");
            }

            if (book.TotalCopies <= 0)
            {
                throw new Exception("Total copies must be greater than zero.");
            }

            if (book.AvailableCopies < 0 ||
                book.AvailableCopies > book.TotalCopies)
            {
                throw new Exception("Invalid available copies.");
            }

            repository.AddBook(book);
        }

        public List<Book> GetAllBooks()
        {
            return repository.GetAllBooks();
        }

        public Book? GetBookById(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Book ID must be greater than zero.");
            }

            return repository.GetBookById(id);
        }
    }
}
