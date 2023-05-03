using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Timesheet.Models;

namespace Timesheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Schedule Schedule { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(DownloadPythonRequirements);
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Application.Current.Shutdown();
        }

        private void TitleWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Generate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Schedule = new Schedule();
            Schedule.FillSchedule(cbCategory.Text, dtStart.SelectedDate.Value, dtEnd.SelectedDate.Value, txStartTime.Text, txEndtime.Text);
            new EditWindow(Schedule).Show();
        }

        private void DownloadPythonRequirements()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "pip3";//cmd is full path to python.exe
            start.Arguments = "install -r requirements.txt";//args is path to .py file and any cmd line args
            start.UseShellExecute = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(start);
        }
    }
}
