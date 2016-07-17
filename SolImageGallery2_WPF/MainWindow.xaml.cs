using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            var urls = await ParseImageUrls(url);

            foreach (var imageUrl in urls)
            {
                using (var stream = await _loader.GetImageAsync(imageUrl))
                {
                    var image = await Task<BitmapImage>.Factory.StartNew((streamObj) =>
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = (Stream)streamObj;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        return bitmap;
                    }, stream);

                    FoundImages.Add(new FoundImage { Thumbnail = new Image { Source = image } });
                }
            }
        }

        private async Task<IEnumerable<string>> ParseImageUrls(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "063a11d97f4743b68b037978a6b90bbe");
            var response = await client.GetAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode) { return Enumerable.Empty<string>(); }

            var content = await response.Content.ReadAsStringAsync();

            return _parser.ExtractImageUrLs(content);
        }
    }
}