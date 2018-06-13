using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Alsolos.AttendanceRecorder.Client.WebApi;
using Alsolos.Commons.Wpf.Mvvm;

namespace Alsolos.AttendanceRecorder.Client.FileSystem
{
    public class FileSystemViewModel : ViewModel
    {
        public DelegateCommand OpenLocalDirectoryCommand => BackingFields.GetCommand(OpenLocalDirectory);

        private void OpenLocalDirectory()
        {
            Task.Run(() =>
            {
                var fileSystemService = new FileSystemService(new WebApiClient());
                var directory = fileSystemService.GetLocalDirectoryAsync().Result;
                if (Directory.Exists(directory))
                {
                    Process.Start(directory);
                }
            });
        }
    }
}
