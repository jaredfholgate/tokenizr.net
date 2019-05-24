using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace tokenizr.net.structures
{
  public class TokenTableSerialiser : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return typeof(TokenTable).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      var table = value as TokenTable;
      writer.WriteStartObject();
      writer.WritePropertyName("columns");
      writer.WriteStartArray();
      foreach (var column in table)
      {
        writer.WriteStartObject();
        writer.WritePropertyName("rows");
        writer.WriteStartArray();
        foreach (var key in column.Keys)
        {
          writer.WriteStartObject();
          writer.WritePropertyName("f");
          writer.WriteValue((int)key);
          writer.WritePropertyName("t");
          writer.WriteValue((int)column[key].Item1);
          writer.WritePropertyName("n");
          writer.WriteValue(column[key].Item2);
          writer.WriteEndObject();
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
      }
      writer.WriteEndArray();
      writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var table = new TokenTable();
      var json = JObject.Load(reader);

      foreach(JToken jsonColumn in json.SelectToken("columns"))
      {
        var column = new Dictionary<char, Tuple<char, int>>();
        foreach(JToken jsonRow in jsonColumn.SelectToken("rows"))
        {
          column.Add(Convert.ToChar((int)jsonRow.SelectToken("f")), new Tuple<char, int>(Convert.ToChar((int)jsonRow.SelectToken("t")), (int)jsonRow.SelectToken("n")));
        }
        table.Add(column);
      }

      return table;
    }
  }
}
