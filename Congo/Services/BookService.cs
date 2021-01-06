﻿using System.Collections.Generic;
using Congo.Models;
using MongoDB.Driver;

namespace Congo.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        // an IBookstoreDatabaseSettings instance is retrieved from DI via constructor injection.
        public BookService(IBookstoreDatabaseSettings settings)
        {
            // MongoClient reads the server instance for performing database operations.
            MongoClient client = new MongoClient(settings.ConnectionString);
            // IMongoDatabase represents the Mongo database for performing operations.
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> Get() => _books.Find(book => true).ToList();

        public Book Get(string id) => _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }
        
        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) => 
            _books.DeleteOne(book => book.Id == id);
    }
}