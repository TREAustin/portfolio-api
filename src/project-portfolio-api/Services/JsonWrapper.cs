using System.Text.Json;
using System.Text.Json.Serialization;

namespace project_portfolio_api.Services
{
    public class JsonWrapper
    {
        ///<summary>
        /// Accepts any model from the application and converts it into a json string.
        /// </summary>
        /// <returns>json string</returns>
        public static string ToJSON(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        
        /// <summary>
        /// Accepts any Json string and converts into a object from the application.
        /// This is a templated method.
        /// </summary>
        /// <returns>Object of type T</returns>
        public static T FromJSON<T>(string json)
        {
            JsonSerializerOptions deserializeOptions = new();
            deserializeOptions.Converters.Add(new CustomConverter());
            return JsonSerializer.Deserialize<T>(json, deserializeOptions)!;
        }

        /// <summary>
        /// Accepts any model from the application and converts it into a Dictionary, key/value pair string/dynamic.
        /// This is the format Firestore uses to store documents.
        /// </summary>
        /// <returns>Dictionary<string, dynamic></returns>
        public static Dictionary<string, dynamic> ToDictionary(object obj)
        {
            JsonSerializerOptions deserializeOptions = new();
            deserializeOptions.Converters.Add(new CustomConverter());
            return JsonSerializer.Deserialize<Dictionary<string, dynamic>>(ToJSON(obj), deserializeOptions)!;
        }

        /// <summary>
        /// Accepts a Dictionary<string,dynamic> and converts it into a class object with JsonPropertyName attribute tags.
        /// </summary>
        /// <returns>generic object</returns>
        public static T FromDictionary<T>(Dictionary<string, dynamic> dict)
        {
            JsonSerializerOptions deserializeOptions = new();
            deserializeOptions.Converters.Add(new CustomConverter());
            return JsonSerializer.Deserialize<T>(ToJSON(dict), deserializeOptions)!;
        }
    }

    internal class CustomConverter : JsonConverter<Dictionary<string, dynamic>>
    {
        public override Dictionary<string, dynamic> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
            }

            var dictionary = new Dictionary<string, dynamic>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("JsonTokenType was not PropertyName");
                }

                var propertyName = reader.GetString();

                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new JsonException("Failed to get property name");
                }

                reader.Read();

                dictionary.Add(propertyName, GetValue(ref reader, options));
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        private object GetValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString()!;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.Null:
                    return null!;
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out var resultInt))
                    {
                        return resultInt;
                    }
                    else if (reader.TryGetDecimal(out var resultDec))
                    {
                        return Double.Parse(resultDec.ToString());
                    }
                    else if (reader.TryGetDouble(out var resultDoub))
                    {
                        return resultDoub;
                    }
                    return reader.GetDecimal();
                case JsonTokenType.StartObject:
                    return Read(ref reader, null!, options);
                case JsonTokenType.StartArray:
                    var list = new List<object>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        list.Add(GetValue(ref reader, options));
                    }
                    return list;
                default:
                    throw new JsonException($"'{reader.TokenType}' is not supported");
            }
        }
    }

}