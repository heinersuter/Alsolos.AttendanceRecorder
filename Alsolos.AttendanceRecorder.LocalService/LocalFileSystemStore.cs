using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Alsolos.AttendanceRecorder.WebApi.Model;
using NLog;

namespace Alsolos.AttendanceRecorder.LocalService
{

    public class LocalFileSystemStore
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _directory;

        public LocalFileSystemStore()
        {
            _directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            _directory = Path.Combine(_directory, "..\\AttendanceRecorder");
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public void SaveLifeSign()
        {
            var lifeSign = DateTime.Now;
            lifeSign = lifeSign.AddTicks(-(lifeSign.Ticks % TimeSpan.TicksPerSecond));

            var filePath = Path.Combine(_directory, $"{lifeSign:yyyy-MM-dd}.aar");
            File.AppendAllText(filePath, lifeSign.ToString("O") + Environment.NewLine);
            Logger.Trace($"LifeSign: {lifeSign:O}");
        }

        public IEnumerable<DateTime> LoadAllLifeSigns()
        {
            return Directory.GetFiles(_directory, "*.aar")
                .SelectMany(file => File.ReadAllLines(file)
                    .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture)));
        }

        public void SaveRemoval(Interval interval)
        {
            var filePath = Path.Combine(_directory, $"{interval.Start:yyyy-MM-dd}.rem");
            File.AppendAllText(filePath, $"{interval.Start:O} - {interval.End:O}" + Environment.NewLine);
        }

        public IEnumerable<Interval> LoadAllRemovals()
        {
            return Directory.GetFiles(_directory, "*.rem")
                .SelectMany(file => File.ReadAllLines(file)
                    .Select(ParseInterval));
        }

        public void SaveMerge(IntervalPair intervalPair)
        {
            var filePath = Path.Combine(_directory, $"{intervalPair.Interval1.Start:yyyy-MM-dd}.mer");
            File.AppendAllText(filePath, $"{intervalPair.Interval1.End:O} - {intervalPair.Interval2.Start:O}" + Environment.NewLine);
        }

        public IEnumerable<Interval> LoadAllMerges()
        {
            return Directory.GetFiles(_directory, "*.mer")
                .SelectMany(file => File.ReadAllLines(file)
                    .Select(ParseInterval));
        }

        private static Interval ParseInterval(string line)
        {
            var parts = line.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            return new Interval
            {
                Start = DateTime.Parse(parts[0], CultureInfo.InvariantCulture),
                End = DateTime.Parse(parts[1], CultureInfo.InvariantCulture)
            };
        }
    }
}
