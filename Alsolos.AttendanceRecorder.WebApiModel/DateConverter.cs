namespace Alsolos.AttendanceRecorder.WebApiModel
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    public class DateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Date);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return StringToDate((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (Date)value;
            writer.WriteValue(DateToString(date));
            writer.Flush();
        }

        public static string DateToString(Date date)
        {
            return string.Format("{0:D4}-{1:D2}-{2:D2}", date.Year, date.Month, date.Day);
        }

        public static Date StringToDate(string s)
        {
            return new Date(DateTime.Parse(s, CultureInfo.InvariantCulture));
        }
    }
}
