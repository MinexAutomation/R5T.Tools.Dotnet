using System;

using R5T.Code.VisualStudio;
using R5T.NetStandard.IO.Paths;


namespace R5T.Tools.Dotnet
{
    public static class DotnetNewDefaultConventions
    {
        /// <summary>
        /// The default project-file file-name-without-extension is the same as the project-name.
        /// </summary>
        public static FileNameWithoutExtension ProjectFileNameWithoutExtensionFromProjectName(ProjectName projectName)
        {
            var projectFileNameWithoutExtension = new FileNameWithoutExtension(projectName.Value);
            return projectFileNameWithoutExtension;
        }

        /// <summary>
        /// The default project-name is the same as the project-file file-name-without-extension.
        /// </summary>
        public static ProjectName ProjectNameFromProjectFileNameWithoutExtension(FileNameWithoutExtension projectFileNameWithoutExtension)
        {
            var projectName = new ProjectName(projectFileNameWithoutExtension.Value);
            return projectName;
        }

        /// <summary>
        /// The default solution-file file-name-without-extension is the same as the solution-name.
        /// </summary>
        public static FileNameWithoutExtension SolutionFileNameWithoutExtensionFromSolutionName(SolutionName solutionName)
        {
            var solutionFileNameWithoutExtension = new FileNameWithoutExtension(solutionName.Value);
            return solutionFileNameWithoutExtension;
        }

        /// <summary>
        /// The default solution-name is the same as the solution file-name-without-extension
        /// </summary>
        public static SolutionName SolutionNameFromSolutionFileNameWithoutExtension(FileNameWithoutExtension solutionFileNameWithoutExtension)
        {
            var solutionName = new SolutionName(solutionFileNameWithoutExtension.Value);
            return solutionName;
        }
    }
}
