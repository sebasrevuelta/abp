// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Blogging.Posts;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IPostAppService), typeof(PostsClientProxy))]
    public partial class PostsClientProxy : ClientProxyBase<IPostAppService>, IPostAppService
    {
        public virtual async Task<ListResultDto<PostWithDetailsDto>> GetListByBlogIdAndTagNameAsync(Guid blogId, string tagName)
        {
            return await RequestAsync<ListResultDto<PostWithDetailsDto>>(nameof(GetListByBlogIdAndTagNameAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), blogId },
                { typeof(string), tagName }
            });
        }

        public virtual async Task<ListResultDto<PostWithDetailsDto>> GetTimeOrderedListAsync(Guid blogId)
        {
            return await RequestAsync<ListResultDto<PostWithDetailsDto>>(nameof(GetTimeOrderedListAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), blogId }
            });
        }

        public virtual async Task<PostWithDetailsDto> GetForReadingAsync(GetPostInput input)
        {
            return await RequestAsync<PostWithDetailsDto>(nameof(GetForReadingAsync), new ClientProxyRequestTypeValue
            {
                { typeof(GetPostInput), input }
            });
        }

        public virtual async Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            return await RequestAsync<PostWithDetailsDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), id }
            });
        }

        public virtual async Task<PostWithDetailsDto> CreateAsync(CreatePostDto input)
        {
            return await RequestAsync<PostWithDetailsDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
            {
                { typeof(CreatePostDto), input }
            });
        }

        public virtual async Task<PostWithDetailsDto> UpdateAsync(Guid id, UpdatePostDto input)
        {
            return await RequestAsync<PostWithDetailsDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), id },
                { typeof(UpdatePostDto), input }
            });
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
            {
                { typeof(Guid), id }
            });
        }
    }
}
