using System;

namespace MAD.JsonConverters.NestedJsonConverterNS
{
    internal class InvalidSelectorSyntaxException : Exception
    {
        private string path;

        public InvalidSelectorSyntaxException(string selector) : base($"A selector must start with a wildcard (*) or with {{}} or [].")
        {
            this.path = selector;
        }
    }
}