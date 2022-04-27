using System;
using System.Xml;

using Microsoft.Extensions.Logging;

using R5T.Code.VisualStudio;
using R5T.Code.VisualStudio.IO;
using R5T.NetStandard.IO.Paths;
using R5T.Tools.NuGet;

using R5T.T0064;

using PathUtilities = R5T.NetStandard.IO.Paths.Utilities;


namespace R5T.Tools.Dotnet
{
    [ServiceImplementationMarker]
    public class DotnetCommandProjectFileActionServicesProvider : IProjectFileActionServicesProvider, IServiceImplementation
    {
        private IProjectFileInformationServicesProvider ProjectFileInformationServicesProvider { get; }
        private ILogger Logger { get; }


        public DotnetCommandProjectFileActionServicesProvider(IProjectFileInformationServicesProvider projectFileInformationServicesProvider, ILogger<DotnetCommandProjectFileActionServicesProvider> logger)
        {
            this.ProjectFileInformationServicesProvider = projectFileInformationServicesProvider;
            this.Logger = logger;
        }

        public void AddPackageReference(ProjectFilePath projectFilePath, PackageSpecification package)
        {
            DotnetCommandServicesProvider.AddPackageToProject(projectFilePath, package, this.Logger);
        }

        public void AddProjectReference(ProjectFilePath projectFilePath, ProjectFilePath referenceProjectFilePath)
        {
            DotnetCommandServicesProvider.AddProjectFileProjectReference(projectFilePath, referenceProjectFilePath, this.Logger);
        }

        public void CreateProjectFile(DotnetNewProjectType projectType, ProjectFilePath projectFilePath)
        {
            DotnetCommandServicesProvider.CreateProjectFile(projectType, projectFilePath, this.Logger);
        }

        public void SetFileCopyToOutputDirectory(ProjectFilePath projectFilePath, FilePath filePath)
        {
            var fileProjectFileRelativePath = PathUtilities.GetRelativePath(projectFilePath.Value, filePath.Value);

            this.Logger.LogDebug($"{projectFilePath} - Setting copy to output directory for file:\n{fileProjectFileRelativePath}");

            // Read the project file path in as XML.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFilePath.Value);

            // Create the new node.
            var copyToOutputDirectoryNode = xmlDoc.CreateElement("CopyToOutputDirectory");
            copyToOutputDirectoryNode.InnerText = "PreserveNewest";

            var noneNode = xmlDoc.CreateElement("None");
            noneNode.AppendChild(copyToOutputDirectoryNode);

            var updateAttributeNode = xmlDoc.CreateAttribute("Update");
            updateAttributeNode.Value = fileProjectFileRelativePath;
            noneNode.Attributes.Append(updateAttributeNode);

            var itemGroupNode = xmlDoc.CreateElement("ItemGroup");
            itemGroupNode.AppendChild(noneNode);

            var projectNode = xmlDoc.ChildNodes[0];

            var firstItemGroup = projectNode.ChildNodes[0];

            projectNode.InsertAfter(itemGroupNode, firstItemGroup);

            xmlDoc.Save(projectFilePath.Value);

            this.Logger.LogInformation($"{projectFilePath} - Set copy to output directory for file:\n{fileProjectFileRelativePath}");
        }

        public void SetProjectVersion(ProjectFilePath projectFilePath, Version version)
        {
            var versionStringForDisplay = version.ToString();
            var versionStringForProjectFile = version.ToString();

            this.Logger.LogDebug($"{projectFilePath} - Setting project file version:\n{versionStringForDisplay}");

            // Read the project file path in as XML.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFilePath.Value);

            // Determine if there is a Project/PropertyGroup/Version node.
            var versionNodeXPath = "//Project/PropertyGroup/Version";
            var versionNode = xmlDoc.SelectSingleNode(versionNodeXPath);

            if (versionNode == null)
            {
                // Create the new node.
                versionNode = xmlDoc.CreateElement("Version");
            }

            versionNode.InnerText = versionStringForProjectFile;

            // Find the first PropertyGroup node.
            var projectPropertyGroupNodeXPath = "//Project/PropertyGroup";
            var projectPropertyGroupNode = xmlDoc.SelectSingleNode(projectPropertyGroupNodeXPath);
            projectPropertyGroupNode.AppendChild(versionNode);

            xmlDoc.Save(projectFilePath.Value);

            this.Logger.LogInformation($"{projectFilePath} - Set project file version:\n{versionStringForDisplay}");
        }

        public void EnableDocumentationGeneration(ProjectFilePath projectFilePath)
        {
            this.Logger.LogDebug($"{projectFilePath} - Enabling documentation generation.");

            // Read the project file path in as XML.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFilePath.Value);

            // Modify the project file.
            var generateDocumentationFileElementXPath = "//Project/PropertyGroup/GenerateDocumentationFile";
            var generateDocumentationFileElement = xmlDoc.SelectSingleNode(generateDocumentationFileElementXPath);

            if (null == generateDocumentationFileElement)
            {
                var propertyGroupElementXPath = "//Project/PropertyGroup";
                var propertyGroupElement = xmlDoc.SelectSingleNode(propertyGroupElementXPath);

                generateDocumentationFileElement = xmlDoc.CreateElement("GenerateDocumentationFile");
                propertyGroupElement.AppendChild(generateDocumentationFileElement);
            }

            generateDocumentationFileElement.InnerText = "true";

            // Save the project file.
            xmlDoc.Save(projectFilePath.Value);

            this.Logger.LogInformation($"{projectFilePath} - Enabled documentation generation.");
        }

        public void SuppressMissingXmlDocumentationWarnings(ProjectFilePath projectFilePath)
        {
            this.Logger.LogDebug($"{projectFilePath} - Suppressing missing XML documentation warning.");

            // Read the project file path in as XML.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFilePath.Value);

            // Modify the project file.
            var noWarnElementXPath = "//Project/PropertyGroup/NoWarn";
            var noWarnElement = xmlDoc.SelectSingleNode(noWarnElementXPath);

            if (null == noWarnElement)
            {
                var propertyGroupElementXPath = "//Project/PropertyGroup";
                var propertyGroupElement = xmlDoc.SelectSingleNode(propertyGroupElementXPath);

                noWarnElement = xmlDoc.CreateElement("NoWarn");
                propertyGroupElement.AppendChild(noWarnElement);
            }

            noWarnElement.InnerText = "1701;1702;1591;1573"; // NOTE! MUST include 1701 and 1702. These are required for .NET Core, and are included by default in the project template. However, explicitly setting any NoWarn property value will override the project template. Thus, we must include these warning in the list of suppressions.

            // Save the project file.
            xmlDoc.Save(projectFilePath.Value);

            this.Logger.LogInformation($"{projectFilePath} - Suppressed missing XML documentation warning.");
        }
    }
}
