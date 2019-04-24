using System.Windows;

namespace Alsolos.AttendanceRecorder.Client.Views
{
    public partial class ChangeStartWindowView
    {
        public ChangeStartWindowView()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
