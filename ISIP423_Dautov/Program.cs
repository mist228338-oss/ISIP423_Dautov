using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp
{
    public enum Genre
    {
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Horror
    }
    public class Book
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }