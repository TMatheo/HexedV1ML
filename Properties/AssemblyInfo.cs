using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(BuildInfo.Description)]
[assembly: AssemblyDescription(BuildInfo.Description)]
[assembly: AssemblyCompany(BuildInfo.Company)]
[assembly: AssemblyProduct(BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + BuildInfo.Author)]
[assembly: AssemblyTrademark(BuildInfo.Company)]
[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]
[assembly: MelonInfo(typeof(LUXED.Main), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author, BuildInfo.DownloadLink)]
[assembly: MelonColor()]
[assembly: MelonGame(null, null)]

public static class BuildInfo
{
    public const string Name = "LUXED";
    public const string Description = "Best client ever made.";
    public const string Author = "Boobs-Corp";
    public const string Company = null;
    public const string Version = "1.0.0";
    public const string DownloadLink = null;
}