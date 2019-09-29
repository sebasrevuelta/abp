﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Linq;

namespace Acme.BookStore.BookManagement.Books
{
    public class BookManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IAsyncQueryableExecuter _queryableExecuter;
        private readonly IGuidGenerator _guidGenerator;

        public BookManagementDataSeedContributor(
            IRepository<Book, Guid> bookRepository,
            IAsyncQueryableExecuter queryableExecuter,
            IGuidGenerator guidGenerator)
        {
            _bookRepository = bookRepository;
            _queryableExecuter = queryableExecuter;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _queryableExecuter.CountAsync(_bookRepository) > 0)
            {
                return;
            }

            await _bookRepository.InsertAsync(
                new Book
                {
                    Id = _guidGenerator.Create(),
                    Name = "Pet Sematary",
                    Price = 42,
                    PublishDate = new DateTime(1995,11,15),
                    Type = BookType.Horror
                }
            );
        }
    }
}
