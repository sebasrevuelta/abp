using System;

namespace Volo.Abp.Authorization.Fluent;

public class AbpAndNodeFluentAuthorizationBuilder : AbpFluentAuthorizationBuilderBase,
    IAbpAndNodeFluentAuthorizationBuilder<AbpAndNodeFluentAuthorizationBuilder>
{
    public AbpAndNodeFluentAuthorizationBuilder(AbpFluentAuthorizationNodeModel model) : base(model)
    {
    }

    public AbpAndNodeFluentAuthorizationBuilder Meet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateAndBranch(config, false, exceptionForFailure);

        return new AbpAndNodeFluentAuthorizationBuilder(Model);
    }

    public AbpAndNodeFluentAuthorizationBuilder NotMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateAndBranch(config, true, exceptionForFailure);

        return new AbpAndNodeFluentAuthorizationBuilder(Model);
    }
}