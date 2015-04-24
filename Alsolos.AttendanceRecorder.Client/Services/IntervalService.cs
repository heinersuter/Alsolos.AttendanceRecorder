namespace Alsolos.AttendanceRecorder.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Alsolos.AttendanceRecorder.Client.Models;
    using Alsolos.AttendanceRecorder.WebApiModel;
    using NLog;

    public class IntervalService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Uri _baseAddress = new Uri("http://localhost:30515/");

        public async Task<IEnumerable<Date>> GetDatesAsync()
        {
            using (var client = InitClient())
            {
                var response = await client.GetAsync("api/intervals/dates");
                if (response.IsSuccessStatusCode)
                {
                    var dates = await response.Content.ReadAsAsync<IEnumerable<string>>();
                    return dates.Select(DateConverter.StringToDate);
                }
                _logger.Error("Getting dates failed. {0} - {1}", response.StatusCode, response.ReasonPhrase);
            }
            return Enumerable.Empty<Date>();
        }

        public async Task<IEnumerable<Interval>> GetIntervalsInRangeAsync(Date from, Date to)
        {
            return await Task.Run(async () =>
            {
                using (var client = InitClient())
                {
                    var requestUri = string.Format("api/intervals/range/{0}/{1}", DateConverter.DateToString(from), DateConverter.DateToString(to));
                    var response = await client.GetAsync(requestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        var intervals = await response.Content.ReadAsAsync<IEnumerable<Interval>>();
                        return intervals;
                    }
                    _logger.Error("Getting intervals failed. {0} - {1}", response.StatusCode, response.ReasonPhrase);
                }
                return Enumerable.Empty<Interval>();
            });
        }

        public async Task RemoveIntervalAsync(Interval interval)
        {
            using (var client = InitClient())
            {
                var response = await client.PostAsJsonAsync("api/intervals/remove", interval);
                if (response.IsSuccessStatusCode)
                {
                    var isSuccessful = await response.Content.ReadAsAsync<bool>();
                    if (isSuccessful)
                    {
                        _logger.Error("Removing interval not possible.");
                    }
                }
                _logger.Error("Removing interval failed. {0} - {1}", response.StatusCode, response.ReasonPhrase);
            }
        }

        public async Task MergeIntervalsAsync(Interval interval1, Interval interval2)
        {
            using (var client = InitClient())
            {
                var response = await client.PostAsJsonAsync("api/intervals/merge", new IntervalPair { Interval1 = interval1, Interval2 = interval2 });
                if (response.IsSuccessStatusCode)
                {
                    var isSuccessful = await response.Content.ReadAsAsync<bool>();
                    if (isSuccessful)
                    {
                        _logger.Error("Removing interval not possible.");
                    }
                }
                _logger.Error("Removing interval failed. {0} - {1}", response.StatusCode, response.ReasonPhrase);
            }
        }

        private HttpClient InitClient()
        {
            var client = new HttpClient { BaseAddress = _baseAddress };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
