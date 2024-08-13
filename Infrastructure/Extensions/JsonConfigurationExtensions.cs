using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VittorioApiT2M.API.Extensions
{
    public static class JsonConfigurationExtensions
    {
        public static void AddCustomJsonConverters(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
            });
        }
    }
}
