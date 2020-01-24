// <copyright file="IGoogleSheetsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the IGoogleSheetsService interface.</summary>
namespace DataBaseSize.IServices
{
    using Google.Apis.Sheets.v4.Data;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    interface IGoogleSheetsService
    {
        /// <summary>
        /// Authorized
        /// </summary>
        bool IsAuthorized { get; }

        /// <summary>
        /// Authorization
        /// </summary>
        Task AuthorizeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Return table by Id
        /// </summary>
        Task<Spreadsheet> GetSpreadsheetAsync(string spreadsheetId, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет лист в указанную таблицу.
        /// </summary>
        Task AddSheetAsync(string spreadsheetId, string sheetName, CancellationToken cancellationToken);

        /// <summary>
        /// Заполняет указанный квадрат в листе определенным значением.
        /// </summary>
        Task FillSheetAsync(string spreadsheetId, string sheetName, int width, int height, object value, CancellationToken cancellationToken);

        /// <summary>
        /// Записывает указанные значения в лист.
        /// </summary>
        Task UpdateValuesAsync(string spreadsheetId, string sheetName, List<IList<object>> values, CancellationToken cancellationToken);
    }
}
