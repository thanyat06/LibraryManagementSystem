using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public Book(int id, string title, string author,
                    string category, int totalCopies,
                    int availableCopies)
        {
            BookId = id;
            Title = title;
            Author = author;
            Category = category;
            TotalCopies = totalCopies;
            AvailableCopies = availableCopies;
        }
    }
}
