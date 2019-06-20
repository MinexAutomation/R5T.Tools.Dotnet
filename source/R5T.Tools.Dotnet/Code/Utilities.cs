using System;

using R5T.Code.VisualStudio;
using R5T.Code.VisualStudio.IO;
using R5T.NetStandard;

using VsUtilities = R5T.Code.VisualStudio.Utilities;
using VsIoUtilities = R5T.Code.VisualStudio.IO.Utilities;


namespace R5T.Tools.Dotnet
{
    public static class Utilities
    {
        public static ProjectFileName GetCSharpProjectFileName(ProjectName projectName, DotnetNewConventions conventions)
        {
            var projectFileNameWithoutExtension = conventions.ProjectFileNameWithoutExtensionFromProjectName(projectName);

            var projectFileName = VsIoUtilities.GetCSharpProjectFileName(projectFileNameWithoutExtension);
            return projectFileName;
        }

        /// <summary>
        /// Uses the <see cref="DotnetNewConventions.Instance"/>.
        /// </summary>
        public static ProjectFileName GetCSharpProjectFileName(ProjectName projectName)
        {
            var projectFileName = Utilities.GetCSharpProjectFileName(projectName, DotnetNewConventions.Instance);
            return projectFileName;
        }

        public static ProjectFilePath GetCSharpProjectFilePath(ProjectDirectoryPath projectDirectoryPath, ProjectName projectName, DotnetNewConventions conventions)
        {
            var projectFileName = Utilities.GetCSharpProjectFileName(projectName, conventions);

            var projectFilePath = VsIoUtilities.GetProjectFilePath(projectDirectoryPath, projectFileName);
            return projectFilePath;
        }

        /// <summary>
        /// Uses the <see cref="DotnetNewConventions.Instance"/>.
        /// </summary>
        public static ProjectFilePath GetCSharpProjectFilePath(ProjectDirectoryPath projectDirectoryPath, ProjectName projectName)
        {
            var projectFilePath = Utilities.GetCSharpProjectFilePath(projectDirectoryPath, projectName, DotnetNewConventions.Instance);
            return projectFilePath;
        }

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

        public static SolutionFileName GetSolutionFileName(SolutionName solutionName, DotnetNewConventions conventions)
        {
            var solutionFileNameWithoutExtension = conventions.SolutionFileNameWithoutExtensionFromSolutionName(solutionName);

            var solutionFileName = VsIoUtilities.GetSolutionFileName(solutionFileNameWithoutExtension);
            return solutionFileName;
        }

        /// <summary>
        /// Uses the <see cref="DotnetNewConventions.Instance"/>.
        /// </summary>
        public static SolutionFileName GetSolutionFileName(SolutionName solutionName)
        {
            var solutionFileName = Utilities.GetSolutionFileName(solutionName, DotnetNewConventions.Instance);
            return solutionFileName;
        }

        public static SolutionFilePath GetSolutionFilePath(SolutionDirectoryPath solutionDirectoryPath, SolutionName solutionName, DotnetNewConventions conventions)
        {
            var solutionFileName = Utilities.GetSolutionFileName(solutionName, conventions);

            var solutionFilePath = VsIoUtilities.GetSolutionFilePath(solutionDirectoryPath, solutionFileName);
            return solutionFilePath;
        }

        /// <summary>
        /// Uses the <see cref="DotnetNewConventions.Instance"/>.
        /// </summary>
        public static SolutionFilePath GetSolutionFilePath(SolutionDirectoryPath solutionDirectoryPath, SolutionName solutionName)
        {
            var solutionFilePath = Utilities.GetSolutionFilePath(solutionDirectoryPath, solutionName, DotnetNewConventions.Instance);
            return solutionFilePath;
        }
    }
}
