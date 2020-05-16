﻿using System;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Search.Util
{
    internal static class ApplicationLogging
    {
        internal static ILoggerFactory LoggerFactory { get; set; } //= new LoggerFactory();
        internal static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        internal static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
    }
}
