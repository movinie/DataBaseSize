// <copyright file="GoogleServiceConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the GoogleServiceConfig class.</summary>
namespace DataBaseSize.Configurations
{
    class GoogleServiceConfig
    {
        /// <summary>
        /// Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Secret key
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Table Id
        /// </summary>
        public string SpreadsheetId { get; set; }
    }
}
