using Alsolos.AttendanceRecorder.Client.FileSystem;
using Alsolos.Commons.Wpf.Mvvm;

namespace Alsolos.AttendanceRecorder.Client.Views
{

    public class MainWindowViewModel : ViewModel
    {
        public FileSystemViewModel FileSystemViewModel
        {
            get { return BackingFields.GetValue(() => new FileSystemViewModel()); }
        }

        public IntervalsSelectorViewModel IntervalsSelectorViewModel
        {
            get { return BackingFields.GetValue(() => new IntervalsSelectorViewModel()); }
        }
    }
}
