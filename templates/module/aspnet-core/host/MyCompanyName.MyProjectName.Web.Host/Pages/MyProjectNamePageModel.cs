﻿using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Pages;

public abstract class MyProjectNamePageModel : AbpPageModel
{
    protected MyProjectNamePageModel()
    {
        LocalizationResourceType = typeof(MyProjectNameResource);
    }
}
