using System;

using Microsoft.Extensions.Logging;

using R5T.NetStandard.IO.Paths;


namespace R5T.Tools.Dotnet
{
    public class DotnetCommand
    {
        public const string Value = @"dotnet";


        public FilePath DotnetExecutableFilePath { get; }
        public ILogger Logger { get; }


        public DotnetCommand(FilePath dotnetExecutableFilePath, ILogger<DotnetCommand> logger)
        {
            this.DotnetExecutableFilePath = dotnetExecutableFilePath;
            this.Logger = logger;
        }
    }
}
