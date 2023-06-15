using System;
using System.Management;
using System.Runtime.CompilerServices;

namespace ProgramTools.Models
{
    public class InstalledApplication
    {
        public string Name { get; private set; }               //Назва встановленої програми
        public string Version { get; private set; }            //Версія встановленої програми
        public DateTime InstallDate { get; private set; }      //Дата, коли програма встановлена
        public string ProductCode { get; private set; }        //Код програми
    



        public InstalledApplication(ManagementBaseObject app)
        {
            Name = (string)app[nameof(Name)];
            Version = (string)app[nameof(Version)];
            InstallDate = ParseInstallDate((string)app[nameof(InstallDate)]);
            ProductCode = (string)app["IdentifyingNumber"];
        }

        private static DateTime ParseInstallDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var date))
            {
                return date;
            }

            throw new FormatException("Не вдалося конвертувати дату.");
        }
    }
}