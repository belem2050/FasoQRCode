using FasoQRCode.Models.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

public class HistoryService
{



    public void SaveHistory(ObservableCollection<HistoryItem> historyItems)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "history_items.xml");


        if (!File.Exists(filePath))
        {
            using (File.Create(filePath)) { };
        }

        try
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<HistoryItem>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, historyItems);
            }

        }catch (Exception ex)
        {
            //
        }
    }

    public ObservableCollection<HistoryItem> LoadHistory()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "history_items.xml");


        if (!File.Exists(filePath))
        {
            return new ObservableCollection<HistoryItem>();
        }

        try
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<HistoryItem>));
            using (var reader = new StreamReader(filePath))
            {
                return (ObservableCollection<HistoryItem>)serializer.Deserialize(reader);
            }

        }catch( Exception ex)
        {
            return new ObservableCollection<HistoryItem>();
        }
    }
}
