using System.Management;

namespace ProgramTools.Models
{
    public sealed class UpdatableInstalledApplication
    {
        public string Name { get; }             //Назва програми
        public string Code { get; }             //Код програми 
        public string LatestVersion { get; }    //Остання версія
        public string UpdateUrl { get; }        //Лінка по якій оновити програму


        public UpdatableInstalledApplication(string name, string code, string latestVersion, string updateUrl)
        {
            Name = name;
            Code = code;
            LatestVersion = latestVersion;
            UpdateUrl = updateUrl;
        }
    }
}