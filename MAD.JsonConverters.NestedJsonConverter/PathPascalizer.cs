using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS
{
    internal class PathPascalizer
    {
        public string PascalizePath(string path)
        {
            string[] pathSplit = path
                .Split('.')
                .Select(y => y.Pascalize())
                .ToArray();

            string pascalizedPath = String.Join(".", pathSplit);

            return pascalizedPath;
        }
    }
}
