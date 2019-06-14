using System;

using R5T.Code.VisualStudio;


namespace R5T.Tools.Dotnet
{
    public static class ProjectTypeExtensions
    {
        public static DotnetNewProjectType DotnetNewProjectType(this ProjectType projectType)
        {
            var dotnetNewProjectType = Utilities.GetDotnetNewProjectType(projectType);
            return dotnetNewProjectType;
        }
    }
}
