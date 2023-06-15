using System.Diagnostics;
using ProgramTools.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Threading;

namespace ProgramTools.Service
{
    public interface IUninstallService
    {
        Task<IEnumerable<InstalledApplication>> GetAllInstalledApps();

        Task<IEnumerable<UpdatableInstalledApplication>> GetInstalledApplicationsToUpdate();

        bool Uninstall(string productCode);

    }

    public class UninstallService : IUninstallService
    {
        private List<InstalledApplication> _cachedInstalledApps;
        private const string GetAllAppsQuery = "SELECT * FROM Win32_Product";

        private const string GetAppByProductCodeQueryFormat = "SELECT * FROM Win32_Product WHERE IdentifyingNumber = '{0}'";

        #region Implementation of IUninstallService

        /// <inheritdoc />
        public async Task<IEnumerable<InstalledApplication>> GetAllInstalledApps()
        {
            if (_cachedInstalledApps?.Any() == true)
            {
                return _cachedInstalledApps;
            }

            return await Task.Run(async () =>
            {
                var searcher = new ManagementObjectSearcher(GetAllAppsQuery);
                var apps = searcher
                    .Get()
                    .AsParallel()
                    .Cast<ManagementObject>()
                    .Select(static app => new InstalledApplication(app))
                    .ToArray();
                await WriteActualVersionsAsync(apps).ConfigureAwait(false);
                _cachedInstalledApps = apps.ToList();
                return apps.Any() ? apps : Array.Empty<InstalledApplication>();
            });
        }

        private static async Task WriteActualVersionsAsync(IEnumerable<InstalledApplication> apps)
        {
            if (File.Exists("versions.json"))
            {
                return ;
            }

            await using var fs = File.Create("versions.json", 1024, FileOptions.Asynchronous);
            var updatableInstalledApps = apps
                .AsParallel()
                .Select(static application => new UpdatableInstalledApplication(application.Name, application.ProductCode, application.Version, "NA")).ToArray();
            await JsonSerializer.SerializeAsync(fs, updatableInstalledApps);
        }

        public async Task<IEnumerable<UpdatableInstalledApplication>> GetInstalledApplicationsToUpdate()
        {
            var installedApps = await GetAllInstalledApps().ConfigureAwait(false);

            if (!File.Exists("versions.json"))
            {
                return Enumerable.Empty<UpdatableInstalledApplication>();
            }

            await using var fileStream = File.Open("versions.json", FileMode.Open);
            var updateList = await JsonSerializer.DeserializeAsync<UpdatableInstalledApplication[]>(fileStream);

            var appsToUpdate = new List<UpdatableInstalledApplication>();

            foreach (var update in updateList)
            {
                var installedApp = installedApps.FirstOrDefault(app => app.ProductCode.Equals(update.Code, StringComparison.OrdinalIgnoreCase));

                if (installedApp == null)
                {
                    continue;
                }

                var installedVersion = Version.Parse(installedApp.Version);
                var latestVersion = Version.Parse(update.LatestVersion);

                if (installedVersion < latestVersion)
                {
                    appsToUpdate.Add(update);
                }
            }

            return appsToUpdate;
        }

        /// <inheritdoc />
        public bool Uninstall(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return false;
            }

            var searcher = new ManagementObjectSearcher(string.Format(GetAppByProductCodeQueryFormat, productCode));
            if (searcher.Get().Count == 0)
            {
                return false;
            }

            var process = new Process()
            {
                StartInfo =
                {
                    FileName = "msiexec.exe",
                    Arguments = $"/x {productCode}"
                }
            };
            if (!process.Start())
            {
                return false;
            }

            process.WaitForExit();
            return process.ExitCode == 0;
        }

        #endregion
    }
}