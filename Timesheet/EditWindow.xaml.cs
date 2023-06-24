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
        Process? processStartInfo;
        public List<Day> Days { get; set; }

        public EditWindow(Schedule schedule)
        {
            InitializeComponent();
            Days = schedule.Days;
            dtGrid.ItemsSource = Days;
            txEmail.Text = Properties.Settings.Default.email;
            txPassword.Password = Properties.Settings.Default.password;
        }

        private async void Start_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var totalHoursWorked = SumTotalHoursWorked();
            var dialogResult = ShowConfirmationDialog(totalHoursWorked);
            switch (dialogResult)
            {
                case MessageBoxResult.Yes:
                    await RunScript();
                        break;
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => processStartInfo?.Kill();

        private void txEmail_LostFocus(object sender, RoutedEventArgs e)
            => SaveProperty(txEmail.Text, PropertySetting.Email);


        private void txPassword_LostFocus(object sender, RoutedEventArgs e)
            => SaveProperty(txPassword.Password, PropertySetting.Password);

        private void SaveProperty(string value, PropertySetting property)
        {
            var propertyName = Enum.GetName(typeof(PropertySetting), property);
            Properties.Settings.Default[propertyName] = value;
            Properties.Settings.Default.Save();
        }

        private string GetLoginInfo()
        {
            var login = "";
            if (!string.IsNullOrEmpty(txEmail.Text) && !string.IsNullOrEmpty(txPassword.Password))
                login = $" {txEmail.Text} {txPassword.Password}";
            return login;
        }

        private Process? StartProcess(string login)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Properties.Resources.PythonPath;//cmd is full path to python.exe
            start.Arguments = $"{Properties.Resources.ScriptFileName} {FileHandler.GetProjectId(Properties.Resources.ConfigFileName)}{login}";//args is path to .py file and any cmd line args
            start.UseShellExecute = true;
            return Process.Start(start);
        }

        private async Task RunScript()
        {
            await FileHandler.SaveJsonToFile(Days, Properties.Resources.ScheduleFileName);
            var login = GetLoginInfo();
            processStartInfo = StartProcess(login);
        }

        private double SumTotalHoursWorked()
        {
            TimeSpan totalHoursWorked = new TimeSpan();
            Days.ForEach(d => totalHoursWorked = totalHoursWorked.Add(d.GetTotalHoursWorked()));
            return totalHoursWorked.TotalHours;
        }
        private MessageBoxResult ShowConfirmationDialog(double totalHoursWorked)
        {
            string messageBoxText = $"Você declarou um total de {totalHoursWorked}  horas, deseja continuar?";
            string caption = "Confirmação";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
