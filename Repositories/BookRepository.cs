using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository
    {
        private string connectionString =
            @"Server=(local)\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public void AddBook(Book book)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = @"INSERT INTO Books
                (Title, Author, Category, TotalCopies, AvailableCopies)
                VALUES (@Title, @Author, @Category, @TotalCopies, @AvailableCopies)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Category", book.Category);
            cmd.Parameters.AddWithValue("@TotalCopies", book.TotalCopies);
            cmd.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Books";

            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Book book = new Book(
                    Convert.ToInt32(reader["BookId"]),
                    reader["Title"].ToString()!,
                    reader["Author"].ToString()!,
                    reader["Category"].ToString()!,
                    Convert.ToInt32(reader["TotalCopies"]),
                    Convert.ToInt32(reader["AvailableCopies"])
                );

                books.Add(book);
            }

            return books;
        }

        public Book? GetBookById(int id)
        {
            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Books WHERE BookId = @BookId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@BookId", id);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Book(
                    Convert.ToInt32(reader["BookId"]),
                    reader["Title"].ToString()!,
                    reader["Author"].ToString()!,
                    reader["Category"].ToString()!,
                    Convert.ToInt32(reader["TotalCopies"]),
                    Convert.ToInt32(reader["AvailableCopies"])
                );
            }

            return null;
        }
    }
}
