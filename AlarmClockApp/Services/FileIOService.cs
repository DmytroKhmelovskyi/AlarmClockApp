using AlarmClockApp.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace AlarmClockApp.Services
{
    class FileIOService
    {
        private readonly string path;
        public FileIOService(string path)
        {
            this.path = path;
        }
        public BindingList<Alarm> LoadData()
        {
            var fileExists = File.Exists(path);
            if (!fileExists)
            {
                File.CreateText(path).Dispose();
                return new BindingList<Alarm>();
            }
            using (var reader = File.OpenText(path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Alarm>>(fileText);
            }
        }
        public void SaveData(object alarmsList)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                string output = JsonConvert.SerializeObject(alarmsList);
                writer.Write(output);
            }
        }
    }
}
