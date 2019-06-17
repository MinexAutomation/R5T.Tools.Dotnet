using System;

using R5T.Code.VisualStudio;
using R5T.NetStandard.IO.Paths;


namespace R5T.Tools.Dotnet
{
    /// <summary>
    /// Conventions used by the "dotnet new" command.
    /// </summary>
    public class DotnetNewConventions
    {
        #region Static

        /// <summary>
        /// Instance used at all sites requiring a static <see cref="DotnetNewConventions"/> instance.
        /// Can be replaced.
        /// </summary>
        public static DotnetNewConventions Instance { get; } = DotnetNewConventions.GetDefault();


        public static DotnetNewConventions GetDefault()
        {
            var conventions = new DotnetNewConventions(
                DotnetNewDefaultConventions.ProjectFileNameWithoutExtensionFromProjectName,
                DotnetNewDefaultConventions.ProjectNameFromProjectFileNameWithoutExtension,
                DotnetNewDefaultConventions.SolutionFileNameWithoutExtensionFromSolutionName,
                DotnetNewDefaultConventions.SolutionNameFromSolutionFileNameWithoutExtension
                );

            return conventions;
        }

        #endregion


        /// <summary>
        /// Solution-name required to get the dotnet new command to create a solution file with the desired file-name-without-extension (the desired file-name just includes the solution-file file-extension).
        /// </summary>
        public Func<FileNameWithoutExtension, SolutionName> SolutionNameFromSolutionFileNameWithoutExtension { get; } //
        /// <summary>
        /// The solution file-name-without-extension that will be created by the dotnet new command given a solution-name.
        /// </summary>
        public Func<SolutionName, FileNameWithoutExtension> SolutionFileNameWithoutExtensionFromSolutionName { get; } //

        /// <summary>
        /// The project file-name-without-extension that will be created by the dotnet new command given a project-name.
        /// </summary>
        public Func<ProjectName, FileNameWithoutExtension> ProjectFileNameWithoutExtensionFromProjectName { get; } //
        /// <summary>
        /// Project-name required to get the dotnet new command to create a project file with the desired file-name-without-extension (the desired file-name just includes the project-file file-extension).
        /// </summary>
        public Func<FileNameWithoutExtension, ProjectName> ProjectNameFromProjectFileNameWithoutExtension { get; } //


        public DotnetNewConventions(
            Func<ProjectName, FileNameWithoutExtension> projectFileNameWithoutExtensionFromProjectName,
            Func<FileNameWithoutExtension, ProjectName> projectNameFromProjectFileNameWithoutExtension,
            Func<SolutionName, FileNameWithoutExtension> solutionFileNameWithoutExtensionFromSolutionName,
            Func<FileNameWithoutExtension, SolutionName> solutionNameFromSolutionFileNameWithoutExtension)
        {
            this.ProjectFileNameWithoutExtensionFromProjectName = projectFileNameWithoutExtensionFromProjectName;
            this.ProjectNameFromProjectFileNameWithoutExtension = projectNameFromProjectFileNameWithoutExtension;
            this.SolutionFileNameWithoutExtensionFromSolutionName = solutionFileNameWithoutExtensionFromSolutionName;
            this.SolutionNameFromSolutionFileNameWithoutExtension = solutionNameFromSolutionFileNameWithoutExtension;
        }
    }
}
