using SmokSmog.Porperties;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following set of attributes.
// Change these attribute values to modify the information associated with an assembly.
[assembly: AssemblyTitle(AssemblyInfo.Title)]
[assembly: AssemblyDescription("SmokSmog Application")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SmokSmog Community")]
[assembly: AssemblyProduct(AssemblyInfo.Title)]
[assembly: AssemblyCopyright("SmokSmog Community 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
// Major Version Minor Version Build Number Revision
//
// You can specify all the values or you can default the Build and Revision Numbers by using the '*'
// as shown below: [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.FileVersion)]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en")]

namespace SmokSmog.Porperties
{
    internal static class AssemblyInfo
    {
        public const string Title = "SmokSmog";
        public const string Version = "2.0.0";
        public const string FileVersion = "2.0.0";
    }
}