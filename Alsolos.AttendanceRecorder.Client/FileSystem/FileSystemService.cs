using System.Threading.Tasks;
using Alsolos.AttendanceRecorder.Client.WebApi;

namespace Alsolos.AttendanceRecorder.Client.FileSystem
{
    public class FileSystemService
    {
        private readonly WebApiClient _client;

        public FileSystemService(WebApiClient client)
        {
            _client = client;
        }

        public async Task<string> GetLocalDirectoryAsync()
        {
            return await _client.GetAsync<string>("api/fileSystem/localDirectory");
        }
    }
}
