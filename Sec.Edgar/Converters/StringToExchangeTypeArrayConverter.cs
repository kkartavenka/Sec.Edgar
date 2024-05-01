using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Models;

namespace Sec.Edgar.Converters;

internal class StringToExchangeTypeArrayConverter : JsonConverter<ExchangeType[]>
{
    public override ExchangeType[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }
        reader.Read();
        
        var elements = new List<ExchangeType>();
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var value = reader.GetString();
            
            if (string.IsNullOrWhiteSpace(value))
            {
                elements.Add(ExchangeType.Unknown);
            }
            else
            {
                elements.Add(value.ToLower().Trim() switch
                {
                    "nyse" => ExchangeType.NYSE,
                    "nasdaq" => ExchangeType.Nasdaq,
                    "euronext" => ExchangeType.Euronext,
                    "xshg" => ExchangeType.XSHG,
                    "xjpx" => ExchangeType.XJPX,
                    "xshe" => ExchangeType.XSHE,
                    "xnse" => ExchangeType.XNSE,
                    "xbom" => ExchangeType.XBOM,
                    "xhkg" => ExchangeType.XHKG,
                    "xtse" => ExchangeType.XTSE,
                    "xlon" => ExchangeType.XLON,
                    "xsau" => ExchangeType.XSAU,
                    "xfra" => ExchangeType.XFRA,
                    "xswx" => ExchangeType.XSWX,
                    "xcse" => ExchangeType.XCSE,
                    "xsto" => ExchangeType.XSTO,
                    "xhel" => ExchangeType.XHEL,
                    "xtal" => ExchangeType.XTAL,
                    "xris" => ExchangeType.XRIS,
                    "xlit" => ExchangeType.XLIT,
                    _ => ExchangeType.Unknown
                });
            }

            reader.Read();
        }

        return elements.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, ExchangeType[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}