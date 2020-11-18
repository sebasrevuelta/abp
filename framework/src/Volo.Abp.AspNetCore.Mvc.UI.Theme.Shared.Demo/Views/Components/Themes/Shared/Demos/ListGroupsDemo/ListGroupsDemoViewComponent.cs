using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.ListGroupsDemo
{
    [Widget(
        StyleTypes = new []{ typeof(PrismjsStyleBundleContributor) },
        ScriptTypes = new[]{ typeof(PrismjsScriptBundleContributor) }
    )]
    public class ListGroupsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/ListGroupsDemo/Default.cshtml";

        public IViewComponentResult Invoke()
        {
            return View(ViewPath);
        }
    }
}