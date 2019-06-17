using System;

using R5T.NetStandard;


namespace R5T.Tools.Dotnet
{
    public static class DotnetNewProjectTypeExtensions
    {
        public static string ToDotnetCommandProjectType(this DotnetNewProjectType projectType)
        {
            switch (projectType)
            {
                case DotnetNewProjectType.Library:
                    return "classlib";

                case DotnetNewProjectType.Console:
                    return "console";

                case DotnetNewProjectType.MSTest:
                    return "mstest";

                default:
                    throw new ArgumentException(EnumHelper.UnexpectedEnumerationValueMessage(projectType));
            }
        }
    }
}
