using System;

using R5T.Code.VisualStudio;
using R5T.NetStandard;


namespace R5T.Tools.Dotnet
{
    public static class Utilities
    {
        public static DotnetNewProjectType GetDotnetNewProjectType(ProjectType projectType)
        {
            switch(projectType)
            {
                case ConsoleProjectType console:
                    return DotnetNewProjectType.Console;

                case LibraryProjectType library:
                    return DotnetNewProjectType.Library;

                case TestingProjectType testing:
                    return DotnetNewProjectType.MSTest;

                case UnspecifiedProjectType unspecified:
                default:
                    throw new ArgumentException(ExceptionHelper.UnexpectedTypeExceptionMessage(projectType));
            }
        }
    }
}
