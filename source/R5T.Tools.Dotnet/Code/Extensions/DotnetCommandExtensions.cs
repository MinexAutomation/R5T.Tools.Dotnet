﻿using System;

using R5T.Code.VisualStudio.IO;
using R5T.NetStandard.IO.Paths;
using R5T.Tools.NuGet;
using R5T.Tools.NuGet.IO;


namespace R5T.Tools.Dotnet
{
    public static class DotnetCommandExtensions
    {
        public static NupkgFilePath Pack(this DotnetCommand command, ProjectFilePath projectFilePath, DirectoryPath outputDirectoryPath)
        {
            var packageFilePath = DotnetCommandServicesProvider.Pack(command.DotnetExecutableFilePath, projectFilePath, outputDirectoryPath, command.Logger);
            return packageFilePath;
        }

        public static NupkgFilePath Pack(this DotnetCommand command, ProjectFilePath projectFilePath, DirectoryPath outputDirectoryPath, PackageID packageID)
        {
            var packageFilePath = DotnetCommandServicesProvider.Pack(command.DotnetExecutableFilePath, projectFilePath, outputDirectoryPath, packageID, command.Logger);
            return packageFilePath;
        }
    }
}
