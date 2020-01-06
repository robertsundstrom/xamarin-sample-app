using System;

using NJsonSchema.Generation;

namespace App1.MobileAppService
{
    internal class CustomSchemaNameGenerator : ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            return type.Name.Replace("Dto", string.Empty);
        }
    }
}
