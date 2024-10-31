using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models;
using System.Text.Json;

namespace FasoQRCode.ViewModels
{
    public partial class PageHistoryVM : ObservableObject
    {

        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

        [ObservableProperty]
        private HistoryItem selectedItem;

        public PageHistoryVM()
        {
            Manager.HistoryItems.Add(new HistoryItem("Scan 1", DateTime.Now, "https://example.com", "banfora.jpg"));
            Manager.HistoryItems.Add(new HistoryItem("Scan 2", DateTime.Now, "Hello World!", "sindou.jpg"));
            //SaveHistoryItemsAsync(Manager.HistoryItems.ToList());
            //LoadHistoryItemsAsync();

        }

        [RelayCommand]
        public void Delete(HistoryItem item)
        {
            if (item != null)
            {
                Manager.HistoryItems.Remove(item);
            }
        }

        [RelayCommand]
        public void ClearHistory()
        {
            Manager.HistoryItems.Clear();
        }

        public void LoadHistoryItemsAsync()
        {
            var items = loadHistoryItemsAsync();
            foreach (var item in items)
            {
                Manager.HistoryItems.Add(item);
            }
            SaveHistoryItemsAsync(Manager.HistoryItems.ToList());
        }


        public void SaveHistoryItemsAsync(List<HistoryItem> historyItems)
        {
            string fileName = "history.json"; // File name for saving
            string json = JsonSerializer.Serialize(historyItems, new JsonSerializerOptions { WriteIndented = true });

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            //string filePath = "C:/Users/groth/Desktop/NovaLynx/history.json";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

                using (var streamWriter = new StreamWriter(filePath))
            {
                 streamWriter.Write(json);
            }
        }


        private List<HistoryItem> loadHistoryItemsAsync()
        {
            string fileName = "history.json"; // File name for loading
            //string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            //Path.
            //string filePath = "C:\\Users\\groth\\Desktop\\NovaLynx\\history.json";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);



            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                return new List<HistoryItem>(); // Return an empty list if the file doesn't exist
            }

            using (var streamReader = new StreamReader(filePath))
            {
                string json = streamReader.ReadToEnd();
                return JsonSerializer.Deserialize<List<HistoryItem>>(json);
            }
        }
    }
}
