﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.Extensions;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Http.Client;
using Volo.Abp.TestApp.Application;
using Volo.Abp.TestApp.Application.Dto;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class PersonAppServiceClientProxy_Tests : AbpHttpClientTestBase
    {
        private readonly IPeopleAppService _peopleAppService;
        private readonly IRepository<Person, Guid> _personRepository;

        public PersonAppServiceClientProxy_Tests()
        {
            _peopleAppService = ServiceProvider.GetRequiredService<IPeopleAppService>();
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
        }

        [Fact]
        public async Task Get()
        {
            var firstPerson = (await _personRepository.GetListAsync()).First();

            var person = await _peopleAppService.GetAsync(firstPerson.Id);
            person.ShouldNotBeNull();
            person.Id.ShouldBe(firstPerson.Id);
            person.Name.ShouldBe(firstPerson.Name);
        }

        [Fact]
        public async Task GetList()
        {
            var people = await _peopleAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            people.TotalCount.ShouldBeGreaterThan(0);
            people.Items.Count.ShouldBe((int) people.TotalCount);
        }

        [Fact]
        public async Task GetEnumerableParams()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var @params = await _peopleAppService.GetEnumerableParams(new List<Guid>
            {
                id1,
                id2
            }, new[] {"name1", "name2"});

            @params.ShouldContain(id1.ToString("N"));
            @params.ShouldContain(id2.ToString("N"));
            @params.ShouldContain("name1");
            @params.ShouldContain("name2");
        }


        [Fact]
        public async Task GetDictionaryParams()
        {
            var id = Guid.NewGuid();

            var @params = await _peopleAppService.GetDictionaryParams(new Dictionary<string, string>
            {
                {"Id", id.ToString()},
                {"Name", "John"},
                {"Age", 36.ToString()},
            });

            @params.ShouldContain($"Id:{id}");
            @params.ShouldContain("Name:John");
            @params.ShouldContain("Age:36");
        }

        [Fact]
        public async Task Delete()
        {
            var firstPerson = (await _personRepository.GetListAsync()).First();

            await _peopleAppService.DeleteAsync(firstPerson.Id);

            firstPerson = (await _personRepository.GetListAsync()).FirstOrDefault(p => p.Id == firstPerson.Id);
            firstPerson.ShouldBeNull();
        }

        [Fact]
        public async Task Create()
        {
            var uniquePersonName = Guid.NewGuid().ToString();

            var person = await _peopleAppService.CreateAsync(new PersonDto
                {
                    Name = uniquePersonName,
                    Age = 42
                }
            );

            person.ShouldNotBeNull();
            person.Id.ShouldNotBe(Guid.Empty);
            person.Name.ShouldBe(uniquePersonName);

            var personInDb = (await _personRepository.GetListAsync()).FirstOrDefault(p => p.Name == uniquePersonName);
            personInDb.ShouldNotBeNull();
            personInDb.Id.ShouldBe(person.Id);
        }

        [Fact]
        public async Task Create_Validate_Exception()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var person = await _peopleAppService.CreateAsync(new PersonDto
                    {
                        Age = 42
                    }
                );
            });
        }

        [Fact]
        public async Task Update()
        {
            var firstPerson = (await _personRepository.GetListAsync()).First();
            var uniquePersonName = Guid.NewGuid().ToString();

            var person = await _peopleAppService.UpdateAsync(
                firstPerson.Id,
                new PersonDto
                {
                    Id = firstPerson.Id,
                    Name = uniquePersonName,
                    Age = firstPerson.Age
                }
            );

            person.ShouldNotBeNull();
            person.Id.ShouldBe(firstPerson.Id);
            person.Name.ShouldBe(uniquePersonName);
            person.Age.ShouldBe(firstPerson.Age);

            var personInDb = (await _personRepository.GetListAsync()).FirstOrDefault(p => p.Id == firstPerson.Id);
            personInDb.ShouldNotBeNull();
            personInDb.Id.ShouldBe(person.Id);
            personInDb.Name.ShouldBe(person.Name);
            personInDb.Age.ShouldBe(person.Age);
        }

        [Fact]
        public async Task GetWithAuthorized()
        {
            await Assert.ThrowsAnyAsync<Exception>(async () => { await _peopleAppService.GetWithAuthorized(); });
        }

        [Fact]
        public async Task GetWithComplexType()
        {
            var result = await _peopleAppService.GetWithComplexType(
                new GetWithComplexTypeInput
                {
                    Value1 = "value one",
                    Inner1 = new GetWithComplexTypeInput.GetWithComplexTypeInner
                    {
                        Value2 = "value two",
                        Inner2 = new GetWithComplexTypeInput.GetWithComplexTypeInnerInner
                        {
                            Value3 = "value three",
                            Value4 = new Dictionary<string, string>
                            {
                                {"name", "john"},
                                {"age", "36"},
                            }
                        },
                    },
                    ListInner = new List<GetWithComplexTypeInput.GetWithComplexTypeInner>
                    {
                        new GetWithComplexTypeInput.GetWithComplexTypeInner
                        {
                            Value2 = "list0 value two",
                            Inner2 = new GetWithComplexTypeInput.GetWithComplexTypeInnerInner
                            {
                                Value3 = "list0 value three",
                                Value4 = new Dictionary<string, string>
                                {
                                    {"name", "list0 john"},
                                    {"age", "list0 36"},
                                }
                            },
                        },
                        new GetWithComplexTypeInput.GetWithComplexTypeInner
                        {
                            Value2 = "list1 value two",
                            Inner2 = new GetWithComplexTypeInput.GetWithComplexTypeInnerInner
                            {
                                Value3 = "list1 value three",
                                Value4 = new Dictionary<string, string>
                                {
                                    {"name", "list1 bob"},
                                    {"age", "list1 42"},
                                }
                            },
                        },
                    }
                }
            );

            result.Value1.ShouldBe("value one");
            result.Inner1.Value2.ShouldBe("value two");
            result.Inner1.Inner2.Value3.ShouldBe("value three");
            result.Inner1.Inner2.Value4.ShouldContain(kv => kv.Key == "name" && kv.Value == "john");
            result.Inner1.Inner2.Value4.ShouldContain(kv => kv.Key == "age" && kv.Value == "36");
            result.ListInner.Count.ShouldBe(2);
            result.ListInner[0].Value2.ShouldBe("list0 value two");
            result.ListInner[0].Inner2.Value3.ShouldBe("list0 value three");
            result.ListInner[0].Inner2.Value4.ShouldContain(kv => kv.Key == "name" && kv.Value == "list0 john");
            result.ListInner[0].Inner2.Value4.ShouldContain(kv => kv.Key == "age" && kv.Value == "list0 36");
            result.ListInner[1].Value2.ShouldBe("list1 value two");
            result.ListInner[1].Inner2.Value3.ShouldBe("list1 value three");
            result.ListInner[1].Inner2.Value4.ShouldContain(kv => kv.Key == "name" && kv.Value == "list1 bob");
            result.ListInner[1].Inner2.Value4.ShouldContain(kv => kv.Key == "age" && kv.Value == "list1 42");
        }
    }
}