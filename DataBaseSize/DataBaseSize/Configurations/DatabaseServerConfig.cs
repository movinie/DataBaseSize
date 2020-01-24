// <copyright file="DatabaseServerConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the DatabaseServerConfig class.</summary>

namespace DataBaseSize.Configurations
{
    class DatabaseServerConfig
    {
        /// <summary>
        /// Server name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Total Disk Size (Gb) 
        /// </summary>
        public decimal DiskSize { get; set; }
    }
}
