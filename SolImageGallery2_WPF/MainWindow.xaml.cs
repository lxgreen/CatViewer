using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CatViewer
{
    public partial class MainWindow
    {
        private const string Url = "https://api.cognitive.microsoft.com/bing/v5.0/images/search?q=cat";

        private ImageSourceExtractor _parser = new ImageSourceExtractor();
        private ImageLoader _loader = new ImageLoader();

        public MainWindow()
        {
            InitializeComponent();

            FoundImages = new ObservableCollection<FoundImage>();
            DataContext = this;

            Loaded += (s, o) =>
            {
                LoadImages(Url);
            };
        }

        public ObservableCollection<FoundImage> FoundImages { get; set; }

        private async void LoadImages(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "063a11d97f4743b68b037978a6b90bbe");

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return;

            var content = await response.Content.ReadAsStringAsync();

            var urls = _parser.ExtractImageURLs(content).ToList();

            urls.ForEach(async imageUrl =>
            {
                using (var stream = await _loader.GetImageAsync(imageUrl))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    FoundImages.Add(new FoundImage { Thumbnail = new Image { Source = bitmap } });
                }
            });
        }
    }
}