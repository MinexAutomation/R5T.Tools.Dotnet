﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Microsoft.Extensions.Logging;

using R5T.Code.VisualStudio;
using R5T.Code.VisualStudio.IO;
using R5T.NetStandard;
using R5T.NetStandard.IO.Paths;
using R5T.NetStandard.OS;
using R5T.Tools.NuGet;

using PathUtilities = R5T.NetStandard.IO.Paths.Utilities;


namespace R5T.Tools.Dotnet
{
    /// <summary>
    /// Provides services of the "dotnet" executable command.
    /// </summary>
    public static class DotnetCommandServicesProvider
    {
        public static void AddPackageToProject(ProjectFilePath projectFilePath, PackageSpecification packageSpecification, ILogger logger)
        {
            logger.LogDebug($"{projectFilePath} - Adding package to project file:\n{packageSpecification}");

            var packageArgument = " " + $"package {packageSpecification.ID}";
            var versionArgument = packageSpecification.HasVersion() ? " " + $"--version {packageSpecification.Version}" : String.Empty;
            var arguments = $"add {projectFilePath}{packageArgument}{versionArgument}";

            ProcessRunner.Run(DotnetCommand.Value, arguments);

            logger.LogInformation($"{projectFilePath} - Added package to project file:\n{packageSpecification}");
        }

        public static void AddProjectFileProjectReference(ProjectFilePath projectFilePath, ProjectFilePath referenceProjectFilePath, ILogger logger)
        {
            logger.LogDebug($"{projectFilePath} - Adding reference to project file:\n{referenceProjectFilePath}");

            var arguments = $@"add {projectFilePath} reference {referenceProjectFilePath}";

            ProcessRunner.Run(DotnetCommand.Value, arguments);

            logger.LogInformation($"{projectFilePath} - Added reference to project file:\n{referenceProjectFilePath}");
        }

        public static ProjectFilePath CreateProjectFile(DotnetNewProjectType projectType, ProjectName projectName, ProjectDirectoryPath projectDirectoryPath, ILogger logger)
        {
            var projectFilePath = Utilities.GetCSharpProjectFilePath(projectDirectoryPath, projectName);

            logger.LogDebug($"{projectName} - Creating project file:\n{projectFilePath}");

            var dotnetCommandProjectType = projectType.ToDotnetCommandProjectType();

            // Notes:
            //  --language: This is hard-coded to C#.
            //  --output: This must be the directory in which the solution file should be placed (tested).
            //  --name: This is the name of the solution, to which the .sln extension will be added. If .sln is suffixed to the name argument the resulting solution file will be .sln.sln!
            var arguments = $@"new {dotnetCommandProjectType} --language ""C#"" --output {projectDirectoryPath} --name {projectName}";

            ProcessRunner.Run(DotnetCommand.Value, arguments);

            logger.LogInformation($"{projectName} - Created project file:\n{projectFilePath}");

            return projectFilePath;
        }

        public static void CreateProjectFile(DotnetNewProjectType projectType, ProjectFilePath projectFilePath, DotnetNewConventions conventions, ILogger logger)
        {
            var projectDirectoryPath = PathUtilities.GetDirectoryPath(projectFilePath).AsProjectDirectoryPath();
            var projectFileName = PathUtilities.GetFileName(projectFilePath).AsProjectFileName();
            var projectFileNameWithoutExtension = PathUtilities.GetFileNameWithoutExtension(projectFileName);
            var projectName = conventions.ProjectNameFromProjectFileNameWithoutExtension(projectFileNameWithoutExtension);

            var createdProjectFilePath = DotnetCommandServicesProvider.CreateProjectFile(projectType, projectName, projectDirectoryPath, logger);

            // Throw an exception if the solution file-path created by dotnet new is not the one we were expecting.
            if (createdProjectFilePath.Value != projectFilePath.Value)
            {
                throw new Exception($"Project creation file path mismatch.\nExpected: {projectFilePath}\nCreated: {createdProjectFilePath}");
            }
        }

        /// <summary>
        /// Uses the <see cref="DotnetNewConventions.Instance"/>.
        /// </summary>
        public static void CreateProjectFile(DotnetNewProjectType projectType, ProjectFilePath projectFilePath, ILogger logger)
        {
            DotnetCommandServicesProvider.CreateProjectFile(projectType, projectFilePath, DotnetNewConventions.Instance, logger);
        }

        public static void AddProjectFileToSolutionFile(SolutionFilePath solutionFilePath, ProjectFilePath projectFilePath, ILogger logger)
        {
            logger.LogDebug($"Adding project file to solution file.\nSolution: {solutionFilePath}\nProject: {projectFilePath}");

            var arguments = $@"sln {solutionFilePath} add {projectFilePath}";

            ProcessRunner.Run(DotnetCommand.Value, arguments);

            logger.LogInformation($"Added project file to solution file.\nSolution: {solutionFilePath}\nProject: {projectFilePath}");
        }

        public static void RemoveProjectFileFromSolutionFile(SolutionFilePath solutionFilePath, ProjectFilePath projectFilePath, ILogger logger)
        {
            logger.LogDebug($"Removing project file from solution file.\nSolution: {solutionFilePath}\nProject: {projectFilePath}");

            var arguments = $@"sln {solutionFilePath} remove {projectFilePath}";

            ProcessRunner.Run(DotnetCommand.Value, arguments);

            logger.LogInformation($"Removed project file from solution file.\nSolution: {solutionFilePath}\nProject: {projectFilePath}");
        }

        /// <summary>
        /// The "dotnet new sln" command allows specifying the directory in which to produce a solution file, and the name of the solution.
        /// However, it does not allows specifying the exact file path of the output solution file.
        /// </summary>
        public static SolutionFilePath CreateSolutionFile(SolutionDirectoryPath solutionDirectoryPath, SolutionName solutionName, ILogger logger)
        {
            var solutionFilePath = Utilities.GetSolutionFilePath(solutionDirectoryPath, solutionName);

            logger.LogDebug($"{solutionName} - Creating solution file:\n{solutionFilePath}");

            // Notes:
            //  --output: This must be the directory in which the solution file should be placed (tested).
            //  --name: This is the name of the solution, to which the .sln extension will be added. If .sln is suffixed to the name argument the resulting solution file will be .sln.sln!
            var arguments = $@"new sln --output {solutionDirectoryPath} --name {solutionName}";

            ProcessRunner.Run(DotnetCommand.Value, arguments);

            logger.LogInformation($"{solutionName} - Created solution file:\n{solutionFilePath}");

            return solutionFilePath;
        }

        /// <summary>
        /// Creates a solution file at the specified file-path.
        /// </summary>
        /// <remarks>
        /// This method feeds the solution directory-path and solution-name value required to have the dotnet new command create a solution-file at the specified path.
        /// </remarks>
        public static void CreateSolutionFile(SolutionFilePath solutionFilePath, DotnetNewConventions conventions, ILogger logger)
        {
            var solutionDirectoryPath = PathUtilities.GetDirectoryPath(solutionFilePath).AsSolutionDirectoryPath();
            var solutionFileName = PathUtilities.GetFileName(solutionFilePath).AsSolutionFileName();
            var solutionFileNameWithoutExtension = PathUtilities.GetFileNameWithoutExtension(solutionFileName);
            var solutionName = conventions.SolutionNameFromSolutionFileNameWithoutExtension(solutionFileNameWithoutExtension);

            var createdSolutionFilePath = DotnetCommandServicesProvider.CreateSolutionFile(solutionDirectoryPath, solutionName, logger);

            // Throw an exception if the solution file-path created by dotnet new is not the one we were expecting.
            if (createdSolutionFilePath.Value != solutionFilePath.Value)
            {
                throw new Exception($"Solution creation file path mismatch.\nExpected: {solutionFilePath}\nCreated: {createdSolutionFilePath}");
            }
        }

        /// <summary>
        /// Uses the <see cref="DotnetNewConventions.Instance"/>.
        /// </summary>
        public static void CreateSolutionFile(SolutionFilePath solutionFilePath, ILogger logger)
        {
            DotnetCommandServicesProvider.CreateSolutionFile(solutionFilePath, DotnetNewConventions.Instance, logger);
        }

        public static ProjectFilePath[] GetSolutionReferencedProjectFilePaths(SolutionFilePath solutionFilePath)
        {
            // Get all project file paths that are referenced by the solution file path.
            var arguments = $@"sln {solutionFilePath} list";

            var projectFilePaths = new List<ProjectFilePath>();

            var runOptions = new ProcessRunOptions
            {
                Command = DotnetCommand.Value,
                Arguments = arguments,
                ReceiveOutputData = (sender, e) => DotnetCommandServicesProvider.ProcessListSolutionProjectReferencesOutput(sender, e, solutionFilePath, projectFilePaths),
            };

            ProcessRunner.Run(runOptions);

            return projectFilePaths.ToArray();
        }

        private static void ProcessListSolutionProjectReferencesOutput(object sender, DataReceivedEventArgs e, SolutionFilePath solutionFilePath, List<ProjectFilePath> projectFilePaths)
        {
            var dataString = e.Data ?? String.Empty;
            using (var reader = new StringReader(dataString))
            {
                while (!reader.ReadLineIsEnd(out string line))
                {
                    if (String.IsNullOrWhiteSpace(line) || line == "Project(s)" || line == "----------")
                    {
                        continue;
                    }

                    var projectFileRelativePath = new FileRelativePath(line);
                    var projectFilePath = PathUtilities.GetFilePath(solutionFilePath, projectFileRelativePath).AsProjectFilePath();

                    projectFilePaths.Add(projectFilePath);
                }
            }
        }

        public static ProjectFilePath[] GetProjectReferencedProjectFilePaths(ProjectFilePath projectFilePath)
        {
            // Get all project file paths that are referenced by the solution file path.
            var arguments = $@"list {projectFilePath} reference";

            var projectFilePaths = new List<ProjectFilePath>();

            var runOptions = new ProcessRunOptions
            {
                Command = DotnetCommand.Value,
                Arguments = arguments,
                ReceiveOutputData = (sender, e) => DotnetCommandServicesProvider.ProcessListProjectProjectReferencesOutput(sender, e, projectFilePath, projectFilePaths),
            };

            ProcessRunner.Run(runOptions);

            return projectFilePaths.ToArray();
        }

        private static void ProcessListProjectProjectReferencesOutput(object sender, DataReceivedEventArgs e, ProjectFilePath projectFilePath, List<ProjectFilePath> projectFilePaths)
        {
            var dataString = e.Data ?? String.Empty;
            using (var reader = new StringReader(dataString))
            {
                while (!reader.ReadLineIsEnd(out string line))
                {
                    if (String.IsNullOrWhiteSpace(line) || line == "Project reference(s)" || line == "--------------------" || line.BeginsWith("There are no Project to Project references in project"))
                    {
                        continue;
                    }

                    var referencedProjectFileRelativePath = new FileRelativePath(line);
                    var referenedProjectFilePath = PathUtilities.GetFilePath(projectFilePath, referencedProjectFileRelativePath).AsProjectFilePath();

                    projectFilePaths.Add(referenedProjectFilePath);
                }
            }
        }

        /// <summary>
        /// Provides any project file paths referenced by the input project file, and any project file paths referenced by those project file paths, and so on recursively, until all project file paths are accumulated.
        /// </summary>
        public static ProjectFilePath[] GetProjectReferencedProjectFilePathsRecursive(ProjectFilePath projectFilePath)
        {
            var uniqueProjects = new HashSet<ProjectFilePath>();
            var queue = new Queue<ProjectFilePath>();

            queue.Enqueue(projectFilePath);
            // Do not include the initial project file path in the output list of referenced project file paths.

            while (queue.Count > 0)
            {
                var currentProjectFilePath = queue.Dequeue();

                var currentProjectReferencedProjects = DotnetCommandServicesProvider.GetProjectReferencedProjectFilePaths(currentProjectFilePath);
                foreach (var referencedProject in currentProjectReferencedProjects)
                {
                    if (!uniqueProjects.Contains(referencedProject))
                    {
                        uniqueProjects.Add(referencedProject);
                        queue.Enqueue(referencedProject);
                    }
                }
            }

            var projectFilePaths = new List<ProjectFilePath>(uniqueProjects);

            projectFilePaths.Sort();

            return projectFilePaths.ToArray();
        }

        ///// Note: currently no need for this capability.
        //public static ProjectFilePath[] GetProjectReferencedPackages(ProjectFilePath projectFilePath)
        //{
        //    // Get all project file paths that are referenced by the solution file path.
        //    var arguments = $@"list {projectFilePath} package";

        //    var projectFilePaths = new List<ProjectFilePath>();

        //    var runOptions = new ProcessRunOptions
        //    {
        //        Command = Dotnet.Command,
        //        Arguments = arguments,
        //        ReceiveOutputData = (sender, e) => DotnetCommandServicesProvider.ProcessListProjectPackageReferencesOutput(sender, e, projectFilePath, projectFilePaths),
        //    };

        //    ProcessRunner.Run(runOptions);

        //    return projectFilePaths.ToArray();
        //}

        //private static void ProcessListProjectPackageReferencesOutput(object sender, DataReceivedEventArgs e, ProjectFilePath projectFilePath, List<ProjectFilePath> projectFilePaths)
        //{
        //    var dataString = e.Data ?? String.Empty;
        //    using (var reader = new StringReader(dataString))
        //    {
        //        while (!reader.ReadLineIsEnd(out string line))
        //        {
        //            Console.WriteLine(line);
        //            //if (String.IsNullOrWhiteSpace(line) || line == "Project reference(s)" || line == "--------------------")
        //            //{
        //            //    continue;
        //            //}

        //            //var referencedProjectFileRelativePath = new FileRelativePath(line);
        //            //var referenedProjectFilePath = IoUtilities.GetFilePath(projectFilePath, referencedProjectFileRelativePath).ToProjectFilePath();

        //            //projectFilePaths.Add(referenedProjectFilePath);
        //        }
        //    }
        //}
    }
}
