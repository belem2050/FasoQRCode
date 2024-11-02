using System.Text.Json;

namespace FasoQRCode.Models.Data
{
    public class HistoryItem
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string Content { get; set; } = string.Empty;
        public string QrThumbnail { get; set; } = string.Empty;


        public HistoryItem(string title, DateTime date, string content, string qrThumbnail)
        {
            Title = title;
            Date = date;
            Content = content;
            QrThumbnail = qrThumbnail;
        }

        public HistoryItem()
        {
        }
    }
}
