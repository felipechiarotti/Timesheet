using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var (fieldsAreValid, errors) = Validate();
            if (!fieldsAreValid)
            {
                var error = "";
                errors.ForEach(e => error += $"- {e}\n");
                MessageBox.Show($"{error}","Erro ao gerar relatório",MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Schedule = new Schedule();
            Schedule.FillSchedule(cbCategory.Text, dtStart.SelectedDate.Value, dtEnd.SelectedDate.Value, txStartTime.Text, txEndtime.Text);
            new EditWindow(Schedule).Show();
        }

        private (bool, List<string>) Validate()
        {
            var errors = new List<string>();

            if (!dtStart.SelectedDate.HasValue || !dtEnd.SelectedDate.HasValue)
                errors.Add("Selecione uma data valida");

            if (!TimeSpan.TryParse(txStartTime.Text, out var _))
                errors.Add("Hora Inicio inválida");

            if (!TimeSpan.TryParse(txEndtime.Text, out var _))
                errors.Add("Hora Fim inválida");

            if(errors.Count > 0)
                return (false, errors);

            return (true,errors);
        }
        private void DownloadPythonRequirements()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "pip3";
            start.Arguments = "install -r requirements.txt";
            start.UseShellExecute = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(start);
        }
    }
}
