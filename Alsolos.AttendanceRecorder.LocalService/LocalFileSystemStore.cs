namespace Alsolos.AttendanceRecorder.LocalService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    public class LocalFileSystemStore
    {
        private readonly string _file;

        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Interval>));

        public LocalFileSystemStore()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            dir = Path.Combine(dir, "..\\AttendanceRecorder");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            _file = Path.Combine(dir, "intervals.xml");
        }

        public void Save(IList<Interval> intervals)
        {
            using (var stream = new FileStream(_file, FileMode.Create, FileAccess.Write))
            {
                _serializer.Serialize(stream, intervals);
            }
        }

        public IList<Interval> Load()
        {
            if (!File.Exists(_file))
            {
                return new List<Interval>();
            }
            using (var stream = new FileStream(_file, FileMode.Open, FileAccess.Read))
            {
                var obj = _serializer.Deserialize(stream);
                return obj as IList<Interval>;
            }
        }
    }
}
