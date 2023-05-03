using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Timesheet.Controllers;
using Timesheet.Models;

namespace Timesheet
{
    /// <summary>
    /// Lógica interna para EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        Process processStartInfo;
        public List<Day> Days{ get; set; }

        public EditWindow(Schedule schedule)
        {
            InitializeComponent();
            Days = schedule.Days;
            dtGrid.ItemsSource = Days;
            txEmail.Text = Properties.Settings.Default.email;
            txPassword.Password = Properties.Settings.Default.password;
        }

        private void Start_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileHandler.SaveJsonToFile(Days, Properties.Resources.ScheduleFileName);
            var login = "";
            if(!string.IsNullOrEmpty(txEmail.Text) && !string.IsNullOrEmpty(txPassword.Password))
            {
                login = $" {txEmail.Text} {txPassword.Password}";
            }
                
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Properties.Resources.PythonPath;//cmd is full path to python.exe
            start.Arguments = $"{Properties.Resources.ScriptFileName} {FileHandler.GetProjectId(Properties.Resources.ConfigFileName)}{login}";//args is path to .py file and any cmd line args
            start.UseShellExecute = true;
            processStartInfo = Process.Start(start);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(processStartInfo != null)
                processStartInfo.Kill();
        }

        private void txEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["email"] = txEmail.Text;
            Properties.Settings.Default.Save();
        }

        private void txPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["password"] = txPassword.Password;
            Properties.Settings.Default.Save();
        }
    }
}
