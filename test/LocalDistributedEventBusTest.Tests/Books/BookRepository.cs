using System;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.MemoryDb;

namespace LocalDistributedEventBusTest.Books;

public class BookRepository : MemoryDbRepository<TestMemoryDbContext, Book, Guid>, IBookRepository
{
    public BookRepository(IMemoryDatabaseProvider<TestMemoryDbContext> databaseProvider)
        : base(databaseProvider)
    {
    }
}
