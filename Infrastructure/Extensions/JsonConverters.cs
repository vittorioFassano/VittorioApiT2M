using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VittorioApiT2M.API.Extensions
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd";
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParseExact(reader.GetString(), DateFormat, FormatProvider, DateTimeStyles.None, out var date))
            {
                return date;
            }
            throw new JsonException("Invalid date format.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat, FormatProvider));
        }
    }
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        private const string TimeFormat = @"hh\:mm\:ss";
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (TimeSpan.TryParseExact(reader.GetString(), TimeFormat, FormatProvider, out var time))
            {
                return time;
            }
            throw new JsonException("Invalid time format.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat, FormatProvider));
        }
    }
}
