﻿using lfmachadodasilva.MyExpenses.Api.Models.Config;
using Microsoft.Extensions.Options;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    /// <summary>
    /// Interface for <see cref="WebSettings"/> settings
    /// </summary>
    public interface IWebSettings
    {
        bool ClearDatabaseAndSeedData { get; }

        bool UseFakeDatabase { get; }
    }

    public class WebSettings : IWebSettings
    {
        private readonly IOptions<WebSettingsConfig> _webSettingsOptions;

        /// <summary>
        /// Constructor for <see cref="WebSettings"/>
        /// </summary>
        /// <param name="webSettingsOptions">Web settings options</param>
        public WebSettings(IOptions<WebSettingsConfig> webSettingsOptions)
        {
            _webSettingsOptions = webSettingsOptions;
        }

        /// <inheritdoc />
        public bool ClearDatabaseAndSeedData => _webSettingsOptions.Value.ClearDatabaseAndSeedData;

        /// <inheritdoc />
        public bool UseFakeDatabase => _webSettingsOptions.Value.UseFakeDatabase;
    }
}