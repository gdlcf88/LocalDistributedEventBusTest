using System;
using System.Threading.Tasks;
using LocalDistributedEventBusTest.Books;
using Shouldly;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace LocalDistributedEventBusTest;

public class BusTests : AbpIntegratedTest<LocalDistributedEventBusTestTestsModule> 
{
    protected IDistributedEventBus DistributedEventBus;
    protected IUnitOfWorkManager UnitOfWorkManager;
    protected IBookRepository BookRepository;
    protected IBlobContainer BlobContainer;

    public BusTests()
    {
        DistributedEventBus = GetRequiredService<LocalDistributedEventBus>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        BookRepository = GetRequiredService<IBookRepository>();
        BlobContainer = GetRequiredService<IBlobContainer>();
    }
    
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
    
    [Fact]
    public async Task Should_Not_Delete_Blob_If_Rollback()
    {
        const string blobName = "my-book-blob";
        
        await BlobContainer.DeleteAsync(blobName);
        await BlobContainer.SaveAsync(blobName, "1234".GetBytes());

        DistributedEventBus.Subscribe<EntityDeletedEto<Book>>(async data =>
        {
            // The EntityChangeEventHelper set `useOutbox = true`.
            // This handler is expected to execute after UOW is completed.
            await BlobContainer.DeleteAsync(data.Entity.BlobName);
        });

        using (var uow = UnitOfWorkManager.Begin(new AbpUnitOfWorkOptions()))
        {
            uow.OnCompleted(async () =>
            {
                // If the `useOutbox = true` works, The blob should not be deleted.
                // Since the event message is in still the outbox.
                (await BlobContainer.GetAllBytesOrNullAsync(blobName)).ShouldNotBeNull();
            });
            
            var book = await BookRepository.InsertAsync(new Book(Guid.Empty, "My book", blobName), true);

            await BookRepository.DeleteAsync(book, true);

            await uow.CompleteAsync();
        }
    }
}