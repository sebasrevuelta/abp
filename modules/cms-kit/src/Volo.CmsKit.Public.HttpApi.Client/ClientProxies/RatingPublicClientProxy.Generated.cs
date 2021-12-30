// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Ratings;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Ratings.ClientProxies;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IRatingPublicAppService), typeof(RatingPublicClientProxy))]
public partial class RatingPublicClientProxy : ClientProxyBase<IRatingPublicAppService>, IRatingPublicAppService
{
    public virtual async Task<RatingDto> CreateAsync(string entityType, string entityId, CreateUpdateRatingInput input)
    {
        return await RequestAsync<RatingDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), entityType },
            { typeof(string), entityId },
            { typeof(CreateUpdateRatingInput), input }
        });
    }

    public virtual async Task DeleteAsync(string entityType, string entityId)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), entityType },
            { typeof(string), entityId }
        });
    }

    public virtual async Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType, string entityId)
    {
        return await RequestAsync<List<RatingWithStarCountDto>>(nameof(GetGroupedStarCountsAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), entityType },
            { typeof(string), entityId }
        });
    }
}
