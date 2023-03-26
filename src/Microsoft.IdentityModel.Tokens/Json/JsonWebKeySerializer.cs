// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET6_0
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Microsoft.IdentityModel.Tokens.Json
{
    internal static class JsonWebKeyNet6Serializer
    {
        public static JsonWebKeyNet6 Read(string json)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            return Read(ref reader);
        }

        /// <summary>
        /// Reads an JsonWebKeyNet6.
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <returns>A <see cref="JsonWebKeyNet6"/>.</returns>
        public static JsonWebKeyNet6 Read(ref Utf8JsonReader reader)
        {
            JsonWebKeyNet6 jsonWebKey = new JsonWebKeyNet6();
            Read(ref reader, jsonWebKey);
            return jsonWebKey;
        }

        /// <summary>
        /// Reads an JsonWebKeyNet6.
        /// </summary>
        /// <param name="json">.</param>
        /// <param name="jsonWebKey"></param>
        /// <returns>A <see cref="JsonWebKeyNet6"/>.</returns>
        public static void Read(string json, JsonWebKeyNet6 jsonWebKey)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            Read(ref reader, jsonWebKey);
        }

        /// <summary>
        /// Reads an JsonWebKeyNet6.
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <param name="jsonWebKey"></param>
        /// <returns>A <see cref="JsonWebKeyNet6"/>.</returns>
        public static void Read(ref Utf8JsonReader reader, JsonWebKeyNet6 jsonWebKey)
        {
            JsonSerializerHelper.CheckForTokenType(ref reader, JsonTokenType.StartObject, true);
            do
            {
                while (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = JsonSerializerHelper.GetPropertyName(ref reader, true);
                    switch (propertyName)
                    {
                        case JsonWebKeyParameterNames.Alg:
                            jsonWebKey.Alg = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.Crv:
                            jsonWebKey.Crv = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.D:
                            jsonWebKey.D = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.DP:
                            jsonWebKey.DP = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.DQ:
                            jsonWebKey.DQ = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.E:
                            jsonWebKey.E = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.K:
                            jsonWebKey.K = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.KeyOps:
                            JsonSerializerHelper.ReadStringsFromArray(ref reader, jsonWebKey.KeyOps);
                            break;
                        case JsonWebKeyParameterNames.Keys:
                            // TODO additional data
                            break;
                        case JsonWebKeyParameterNames.Kid:
                            jsonWebKey.Kid = JsonSerializerHelper.ReadString(ref reader);
                            break;
                        case JsonWebKeyParameterNames.X5c:
                            JsonSerializerHelper.ReadStringsFromArray(ref reader, jsonWebKey.X5c);
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }

                if (JsonSerializerHelper.IsEndObject(ref reader, true))
                    break;

            } while (reader.Read());

            return;
        }

        public static string Write(JsonWebKeyNet6 jsonWebKey)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Utf8JsonWriter writer = JsonSerializerHelper.GetUtf8JsonWriter(memoryStream))
                {
                    Write(writer, jsonWebKey);
                    writer.Flush();
                    return UTF8Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static void Write(Utf8JsonWriter writer, JsonWebKeyNet6 jsonWebKey)
        {
            _ = jsonWebKey ?? throw new ArgumentNullException(nameof(jsonWebKey));
            _ = writer ?? throw new ArgumentNullException(nameof(writer));

            writer.WriteStartObject();
            if (!string.IsNullOrEmpty(jsonWebKey.Alg))
                writer.WriteString(JsonWebKeyParameterNames.Alg, jsonWebKey.Alg);

            if (!string.IsNullOrEmpty(jsonWebKey.Crv))
                writer.WriteString(JsonWebKeyParameterNames.Crv, jsonWebKey.Crv);

            if (!string.IsNullOrEmpty(jsonWebKey.D))
                writer.WriteString(JsonWebKeyParameterNames.D, jsonWebKey.D);

            if (!string.IsNullOrEmpty(jsonWebKey.DP))
                writer.WriteString(JsonWebKeyParameterNames.DP, jsonWebKey.DP);

            if (!string.IsNullOrEmpty(jsonWebKey.DQ))
                writer.WriteString(JsonWebKeyParameterNames.DQ, jsonWebKey.DQ);

            if (!string.IsNullOrEmpty(jsonWebKey.E))
                writer.WriteString(JsonWebKeyParameterNames.E, jsonWebKey.E);

            if (!string.IsNullOrEmpty(jsonWebKey.K))
                writer.WriteString(JsonWebKeyParameterNames.K, jsonWebKey.K);

            if (jsonWebKey.KeyOps.Count > 0)
                JsonSerializerHelper.WriteArrayOfStrings(ref writer, JsonWebKeyParameterNames.KeyOps, jsonWebKey.KeyOps);

            if (!string.IsNullOrEmpty(jsonWebKey.Kid))
                writer.WriteString(JsonWebKeyParameterNames.Kid, jsonWebKey.Kid);

            writer.WriteEndObject();
        }
    }
}
#endif // #if NET6_0
