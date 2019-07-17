﻿using System.Threading.Tasks;
using Volo.Abp.Cli.Args;

namespace Volo.Abp.Cli.Commands
{
    public interface IConsoleCommand
    {
        Task ExecuteAsync(CommandLineArgs commandLineArgs);

        Task<string> GetUsageInfo();

        Task<string> GetShortDescriptionAsync();
    }
}