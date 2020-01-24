// <copyright file="DatabaseInfoService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the DatabaseInfoService class.</summary>
namespace DataBaseSize.Services
{
    using Dapper;
    using DataBaseSize.Configurations;
    using DataBaseSize.IServices;
    using DataBaseSize.Model;
    using Npgsql;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;


    class DatabaseInfoService : IDatabaseInfoService
    {
        public async Task<IEnumerable<DatabaseInfo>> GetDatabaseSizeAsync(DatabaseServerConfig config,
            CancellationToken cancellationToken)
        {
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = config.Host,
                Port = config.Port,
                Username = config.Username,
                Password = config.Password,
                Database = "databasesize"
            }.ToString();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                return await connection.QueryAsync<DatabaseInfo>
                (
                    new CommandDefinition
                    (
                        @"select 
                            t1.datname as name, 
                            pg_database_size(t1.datname) as size 
                        from pg_database t1",
                        cancellationToken: cancellationToken
                    )
                );
            }
        }
    }
}
