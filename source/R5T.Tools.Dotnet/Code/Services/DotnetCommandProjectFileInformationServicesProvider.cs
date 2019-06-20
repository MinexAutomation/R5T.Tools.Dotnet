﻿using System;
using System.Xml;

using R5T.Code.VisualStudio;
using R5T.Code.VisualStudio.IO;
using R5T.NetStandard;


namespace R5T.Tools.Dotnet
{
    public class DotnetCommandProjectFileInformationServicesProvider : IProjectFileInformationServicesProvider
    {
        public ProjectFilePath[] GetReferencedProjectFilePaths(ProjectFilePath projectFilePath)
        {
            var projectFilePaths = DotnetCommandServicesProvider.GetProjectReferencedProjectFilePaths(projectFilePath);
            return projectFilePaths;
        }

        public ProjectFilePath[] GetReferencedProjectFilePathsRecursive(ProjectFilePath projectFilePath)
        {
            var projectFilePaths = DotnetCommandServicesProvider.GetProjectReferencedProjectFilePathsRecursive(projectFilePath);
            return projectFilePaths;
        }

        public bool HasVersion(ProjectFilePath projectFilePath, out Version version)
        {
            // Read the project file path in as XML.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFilePath.Value);

            // Determine if there is a Project/PropertyGroup/Version node.
            var versionNodeXPath = "//Project/PropertyGroup/Version";
            var versionNode = xmlDoc.SelectSingleNode(versionNodeXPath);

            var hasVersion = versionNode != null;
            if (hasVersion)
            {
                version = Version.Parse(versionNode.InnerText);
            }
            else
            {
                version = VersionHelper.None;
            }

            return hasVersion;
        }
    }
}
