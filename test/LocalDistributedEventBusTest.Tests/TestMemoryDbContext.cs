using System;
using System.Collections.Generic;
using LocalDistributedEventBusTest.Books;
using Volo.Abp.MemoryDb;

namespace LocalDistributedEventBusTest;

public class TestMemoryDbContext : MemoryDbContext
{
    private static readonly Type[] EntityTypeList = {
            typeof(Book),
        };

    public override IReadOnlyList<Type> GetEntityTypes()
    {
        return EntityTypeList;
    }
}
