using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace Currency.BLL.JsonSnakeCase
{
    public static class JsonSnakeCaseConverter
    {
        public static T DeserializeSnakeCase<T>(string json)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SnakeCaseContractResolver()
            };
            return JsonConvert.DeserializeObject<T>(json, settings)!;
        }

        private class SnakeCaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return ToSnakeCase(propertyName);
            }

            private string ToSnakeCase(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return str;
                }
                var snakeCase = Regex.Replace(str, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();
                if (snakeCase == str)
                {
                    return snakeCase;
                }
                return ToSnakeCase(snakeCase);
            }
        }
    }
}
