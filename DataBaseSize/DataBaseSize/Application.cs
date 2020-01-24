// <copyright file="Application.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the Application class.</summary>
namespace DataBaseSize
{
    using DataBaseSize.Configurations;
    using DataBaseSize.IServices;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    class Application
    {
        private readonly ApplicationConfig _applicationConfig;
        private readonly GoogleServiceConfig _googleServiceConfig;
        private readonly IDatabaseInfoService _dbInfo;
        private readonly IGoogleSheetsService _googleSheets;

        public Application(IOptions<ApplicationConfig> applicationConfig,
            IOptions<GoogleServiceConfig> googleConfig,
            IDatabaseInfoService dbInfo,
            IGoogleSheetsService googleSheets)
        {
            _applicationConfig = applicationConfig.Value;
            _googleServiceConfig = googleConfig.Value;
            _dbInfo = dbInfo;
            _googleSheets = googleSheets;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            Console.Write("Google authorization");
            await _googleSheets.AuthorizeAsync(cancellationToken);
            Console.WriteLine("ok");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var serverConfig in _applicationConfig.DatabaseServers)
                {
                    Console.Write($"Refresh '{serverConfig.Name}'");

                    string serverName = serverConfig.Name;

                    string updateStatisticsDate = DateTime.Today.ToShortDateString();

                    var dbInfos = (await _dbInfo.GetDatabaseSizeAsync(serverConfig, cancellationToken))
                        .ToList();

                    var spreadsheet = await _googleSheets.GetSpreadsheetAsync(_googleServiceConfig.SpreadsheetId, 
                        cancellationToken);
                    string spreadsheetId = spreadsheet.SpreadsheetId;

                    var serverSheet = spreadsheet.Sheets.FirstOrDefault(x => x.Properties.Title == serverName);

                    if (serverSheet == null)
                        await _googleSheets.AddSheetAsync(spreadsheetId, serverName, cancellationToken);
                    else
                        await _googleSheets.FillSheetAsync(spreadsheetId, serverName, 16, 64, string.Empty, 
                            cancellationToken);

                    var values = new List<IList<object>> {
                        new List<object> { "Сервер", "База данных", "Размер в ГБ", "Дата обновления" } };

                    for (var i = 0; i < dbInfos.Count; i++)
                    {
                        var dbInfo = dbInfos[i];
                        values.Add
                        (
                            new List<object>
                            {
                                i == 0 ? serverName : string.Empty,
                                dbInfo.Name,
                                ConvertToGb(dbInfo.Size),
                                updateStatisticsDate
                            }
                        );
                    }

                    values.Add(new List<object>());
                    values.Add(new List<object> { string.Empty, "Свободно",
                        serverConfig.DiskSize - ConvertToGb(dbInfos.Sum(x => x.Size)) });

                    await _googleSheets.UpdateValuesAsync(spreadsheetId, serverName, values, cancellationToken);

                    Console.WriteLine("ok");
                }

                Console.WriteLine("...");
                await Task.Delay(_applicationConfig.RefreshDelay, cancellationToken);
            }
        }

        private decimal ConvertToGb(decimal bytes)
        {
            return Math.Round(bytes / 1024m / 1024 / 1024, 2);
        }
    }
}
