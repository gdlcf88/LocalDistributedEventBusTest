using System;
using Volo.Abp.Domain.Entities;

namespace LocalDistributedEventBusTest.Books;

public class Book : AggregateRoot<Guid>
{
    public string Name { get; set; }
    
    public string BlobName { get; set; }

    private Book()
    {

    }

    public Book(Guid id, string name, string blobName) : base(id)
    {
        Name = name;
        BlobName = blobName;
    }
}
