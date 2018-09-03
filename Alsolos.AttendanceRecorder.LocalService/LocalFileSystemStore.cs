using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Alsolos.AttendanceRecorder.WebApi.FileSystem;
using Alsolos.AttendanceRecorder.WebApi.Intervals;
using NLog;

namespace Alsolos.AttendanceRecorder.LocalService
{
    public class LocalFileSystemStore : IFileSystemStore
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public LocalFileSystemStore()
        {
            LocalDirectory = @"C:\Users\hsu\Dropbox\Zühlke\AttendanceRecorder";
            if (!Directory.Exists(LocalDirectory))
            {
                Directory.CreateDirectory(LocalDirectory);
            }
            Logger.Info($"Files are stored in '{LocalDirectory}'.");
        }

        public string LocalDirectory { get; }

        public void SaveLifeSign()
        {
            var lifeSign = DateTime.Now;
            lifeSign = lifeSign.AddTicks(-(lifeSign.Ticks % TimeSpan.TicksPerSecond));

            var filePath = Path.Combine(LocalDirectory, $"{lifeSign:yyyy-MM-dd}.aar");
            File.AppendAllText(filePath, lifeSign.ToString("O") + Environment.NewLine);
            Logger.Trace($"LifeSign: {lifeSign:O}");
        }

        public IEnumerable<DateTime> LoadAllLifeSigns()
        {
            return Directory.GetFiles(LocalDirectory, "*.aar")
                .SelectMany(file => File.ReadAllLines(file)
                    .Select(s => ParseDateTime(s)));
        }

        public void SaveRemoval(Interval interval)
        {
            var filePath = Path.Combine(LocalDirectory, $"{interval.Start:yyyy-MM-dd}.rem");
            File.AppendAllText(filePath, $"{interval.Start:O} - {interval.End:O}" + Environment.NewLine);
        }

        public IEnumerable<Interval> LoadAllRemovals()
        {
            return Directory.GetFiles(LocalDirectory, "*.rem")
                .SelectMany(file => File.ReadAllLines(file)
                    .Select(ParseInterval));
        }

        public void SaveMerge(IntervalPair intervalPair)
        {
            var filePath = Path.Combine(LocalDirectory, $"{intervalPair.Interval1.Start:yyyy-MM-dd}.mer");
            File.AppendAllText(filePath, $"{intervalPair.Interval1.End:O} - {intervalPair.Interval2.Start:O}" + Environment.NewLine);
        }

        public IEnumerable<Interval> LoadAllMerges()
        {
            return Directory.GetFiles(LocalDirectory, "*.mer")
                .SelectMany(file => File.ReadAllLines(file)
                    .Select(ParseInterval));
        }

        private static DateTime ParseDateTime(string line)
        {
            return DateTime.Parse(line, CultureInfo.InvariantCulture);
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
