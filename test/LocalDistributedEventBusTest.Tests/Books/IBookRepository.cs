using System;
using Volo.Abp.Domain.Repositories;

namespace LocalDistributedEventBusTest.Books;

public interface IBookRepository : IBasicRepository<Book, Guid>
{
}
