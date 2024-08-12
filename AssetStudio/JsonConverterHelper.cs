using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetStudio
{
    public static class JsonConverterHelper
    {
        public class ByteArrayConverter : JsonConverter<byte[]>
        {
            public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.TokenType == JsonTokenType.StartArray
                    ? JsonSerializer.Deserialize<List<byte>>(ref reader).ToArray() //JsonArray to ByteArray
                    : JsonSerializer.Deserialize<byte[]>(ref reader);
            }

            public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
            {
                writer.WriteBase64StringValue(value);
            }
        }

        public class FloatConverter : JsonConverter<float>
        {
            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<float>(ref reader, new JsonSerializerOptions
                {
                    NumberHandling = options.NumberHandling
                });
            }

            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                {
                    if (options.NumberHandling == JsonNumberHandling.AllowNamedFloatingPointLiterals)
                    {
                        writer.WriteStringValue($"{value.ToString(CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        writer.WriteStringValue(JsonSerializer.Serialize(value));
                    }
                }
                else
                {
                    writer.WriteNumberValue((decimal)value);
                }
            }
        }
    }
}
