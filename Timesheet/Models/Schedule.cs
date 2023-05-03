using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.Models
{
    public class Schedule
    {
        public List<Day> Days { get; set; }
        public Schedule()
        {
            Days = new List<Day>();
        }
        
        public void AddDay(Day day)
        {
            var dayInList = Days.FirstOrDefault(x => x.Date == day.Date);
            if (dayInList is not null) {
                Days.Remove(dayInList);
            }
            Days.Add(day);
        }

        public void FillSchedule(string category, DateTime startDate, DateTime endDate, string startTime, string endTime)
        {
            var initialDay = startDate.Day;
            var finalDay = endDate.Day;

            for(int i = initialDay; i<=finalDay; i++)
            {
                var currentDay = new DateTime(startDate.Year, startDate.Month, i).DayOfWeek;
                if (currentDay == DayOfWeek.Saturday || currentDay == DayOfWeek.Sunday)
                    continue;

                var day = new Day()
                {
                    Category = int.Parse(category.Split(' ')[0]),
                    Date = $"{i.ToString().PadLeft(2,'0')}/{startDate.Month.ToString().PadLeft(2,'0')}/{startDate.Year}",
                    StartTime = startTime,
                    EndTime = endTime,
                };
                Days.Add(day);
            }
        }
    }
}
