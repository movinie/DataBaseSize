// <copyright file="ApplicationConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the ApplicationConfig class.</summary>
namespace DataBaseSize.Configurations
{
    using System;

    class ApplicationConfig
    {
        /// <summary>
        /// Database Servers
        /// </summary>
        public DatabaseServerConfig[] DatabaseServers { get; set; }

        /// <summary>
        /// Interval for update
        /// </summary>
        public TimeSpan RefreshDelay { get; set; }
    }
}
