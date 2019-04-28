﻿namespace OffUploader.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;

    internal static class DirectoryExtensions
    {
        public static IEnumerable<string> GetJpegs(this IDirectory directory, string path) => directory.EnumerateFiles(path, "*.jpeg", SearchOption.TopDirectoryOnly).Union(
                directory.EnumerateFiles(path, "*.jpg", SearchOption.TopDirectoryOnly));
    }
}
