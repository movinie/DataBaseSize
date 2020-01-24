// <copyright file="IDatabaseInfoService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the IDatabaseInfoService class.</summary>
namespace DataBaseSize.IServices
{
    using DataBaseSize.Configurations;
    using DataBaseSize.Model;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    interface IDatabaseInfoService
    {
        /// <summary>
        /// GetDatabaseSizeAsync.
        /// </summary>
        /// <param name="config">DataBase config</param>
        /// <param name="cancellationToken">DataBase config</param>
        /// <returns>CancellationToken</returns>
        Task<IEnumerable<DatabaseInfo>> GetDatabaseSizeAsync(DatabaseServerConfig config,
            CancellationToken cancellationToken);
    }
}
