using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Timesheet.Models;

namespace Timesheet.Controllers
{
    public static class FileHandler
    {
        private static readonly JsonSerializerOptions _options = new() {};
        public static void SaveJsonToFile(List<Day> schedule, string fileName)
        {
            var formattedDays = schedule.Select(x => x.ToFileObject());
            var jsonString = JsonSerializer.Serialize(formattedDays, _options);
            File.WriteAllText(fileName, jsonString);
        }

        public static string GetProjectId(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
