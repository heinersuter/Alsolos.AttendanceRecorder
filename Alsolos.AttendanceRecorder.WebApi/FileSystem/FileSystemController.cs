using System.Web.Http;
using NLog;

namespace Alsolos.AttendanceRecorder.WebApi.FileSystem
{
    [RoutePrefix("api/fileSystem")]
    public class FileSystemController : ApiController
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IFileSystemStore _fileSystemStore;

        public FileSystemController()
        {
            _fileSystemStore = WebApiStarter.FileSystemStore;
        }

        [Route("localDirectory")]
        [HttpGet]
        public string GetLocalDirectory()
        {
            Logger.Trace($"API: GetLocalDirectory: {_fileSystemStore.LocalDirectory}");
            return _fileSystemStore.LocalDirectory;
        }
    }
}
