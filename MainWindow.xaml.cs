using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Reflection;
using System.Windows.Media;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage originalImage = new BitmapImage(new Uri(openFileDialog.FileName));
                // set value to display the main image and convert the original image and display by R,G,B
                DisplayImages(originalImage);
            }
        }
        private void UseDefaultImage_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage originalImage = new BitmapImage(new Uri("pack://application:,,,/Images/red_car_default.jpg"));
            // set value to display the main image and convert the original image and display by R,G,B
            DisplayImages(originalImage);
        }
        private void DisplayImages(BitmapImage originalImage)
        {
            // set value to display the main image
            Main_image.Source = originalImage;

            // convert the original image by R,G,B
            DisplayChannelImages(originalImage);
        }
        private void DisplayChannelImages(BitmapImage originalImage)
        {
            //this function convert bitmapimage by R,G,B
            Bitmap bitmap = BitmapImage2Bitmap(originalImage);

            ImageR.Source = ConvertBitmapToImageSource(GetChannelImage(bitmap, "R"));
            ImageG.Source = ConvertBitmapToImageSource(GetChannelImage(bitmap, "G"));
            ImageB.Source = ConvertBitmapToImageSource(GetChannelImage(bitmap, "B"));
        }

        private Bitmap GetChannelImage(Bitmap bitmap, string channel)
        {
            Bitmap channelBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    System.Drawing.Color originalPixel = bitmap.GetPixel(x, y);

                    int r = 0, g = 0, b = 0;

                    if (channel == "R") r = originalPixel.R;
                    if (channel == "G") g = originalPixel.G;
                    if (channel == "B") b = originalPixel.B;

                    channelBitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));
                }
            }

            return channelBitmap;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private BitmapSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
