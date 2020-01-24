// <copyright file="GoogleSheetsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the GoogleSheetsService class.</summary>

namespace DataBaseSize.Services
{
    using DataBaseSize.Configurations;
    using DataBaseSize.IServices;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    class GoogleSheetsService : IGoogleSheetsService
    {
        private const string ApplicationName = @"DataBaseSize";

        private readonly GoogleServiceConfig _googleConfig;
        private SheetsService _sheetsService;

        public bool IsAuthorized { get; private set; }

        public GoogleSheetsService(GoogleServiceConfig googleConfig)
        {
            _googleConfig = googleConfig;
        }

        public async Task AuthorizeAsync(CancellationToken cancellationToken)
        {
            if (IsAuthorized)
                return;

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync
            (
                clientSecrets: new ClientSecrets
                {
                    ClientId = _googleConfig.ClientId,
                    ClientSecret = _googleConfig.ClientSecret
                },
                scopes: new[] { SheetsService.Scope.Spreadsheets },
                user: ApplicationName,
                taskCancellationToken: cancellationToken
            );

            _sheetsService = new SheetsService
            (
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                }
            );

            IsAuthorized = true;
        }

        public async Task<Spreadsheet> GetSpreadsheetAsync(string spreadsheetId, 
            CancellationToken cancellationToken)
        {
            if (!IsAuthorized)
                throw new Exception("You must authorize!");

            return await _sheetsService.Spreadsheets.Get(spreadsheetId).ExecuteAsync(cancellationToken);
        }

        public async Task AddSheetAsync(string spreadsheetId, string sheetName, CancellationToken cancellationToken)
        {
            if (!IsAuthorized)
                throw new Exception("You must authorize!");

            var request = _sheetsService.Spreadsheets.BatchUpdate
            (
                new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request>
                    {
                        new Request
                        {
                            AddSheet = new AddSheetRequest
                            {
                                Properties = new SheetProperties
                                {
                                    Title = sheetName
                                }
                            }
                        }
                    }
                },
                spreadsheetId
            );

            await request.ExecuteAsync(cancellationToken);
        }

        public async Task FillSheetAsync(string spreadsheetId, 
            string sheetName, int width, int height, object value, CancellationToken cancellationToken)
        {
            if (!IsAuthorized)
                throw new Exception("You must authorize!");

            var fillValues = new List<IList<object>>();
            for (int i = 0; i < height; i++)
            {
                var values = new List<object>();
                fillValues.Add(values);
                for (int j = 0; j < width; j++)
                    values.Add(value);
            }

            var request = _sheetsService.Spreadsheets.Values
                .Update(new ValueRange { Values = fillValues }, spreadsheetId, $"{sheetName}");

            request.ValueInputOption = 
                SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            await request.ExecuteAsync(cancellationToken);
        }

        public async Task UpdateValuesAsync(string spreadsheetId, string sheetName, 
            List<IList<object>> values, CancellationToken cancellationToken)
        {
            if (!IsAuthorized)
                throw new Exception("You must authorize!");

            var request = _sheetsService.Spreadsheets.Values.Update(new ValueRange { Values = values }, 
                spreadsheetId, sheetName);

            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            await request.ExecuteAsync(cancellationToken);
        }
    }
}
