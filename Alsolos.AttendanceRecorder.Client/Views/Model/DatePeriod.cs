namespace Alsolos.AttendanceRecorder.Client.Views.Model
{
    using Alsolos.AttendanceRecorder.WebApiModel;

    public class DatePeriod
    {
        public DatePeriod(DatePeriodType type, string name, Date start, Date end)
        {
            Type = type;
            Name = name;
            Start = start;
            End = end;
        }

        public DatePeriodType Type { get; private set; }

        public string Name { get; private set; }

        public Date Start { get; private set; }

        public Date End { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
