using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using System.Drawing;

namespace sexivirt.IntegrationTest.Tools
{
    public class Imaginarium : Filerarium
    {
        public static string GetRandomSourceImage()
        {
            return GetRandomSourceFile("D:\\test\\sandbox\\images\\", "*.jpg");
        }

        public static string SaveRandomImage(string folder, int maxwidth = 1600, int quality = 90)
        {
            var imageUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var absFile = MakeAbsFolder(imageUrl);
            using (var fileSource = new FileStream(GetRandomSourceImage(), FileMode.Open))
            {
                var settings = new ResizeSettings("maxwidth=" + maxwidth.ToString() + "&crop=auto");
                settings.Quality = quality;
                ImageBuilder.Current.Build(fileSource, absFile, settings);
            }
            return imageUrl;
        }

        public static string SaveRandomImage(string folder, string resizeSettings, int quality = 90)
        {
            var imageUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var absFile = MakeAbsFolder(imageUrl);
            using (var fileSource = new FileStream(GetRandomSourceImage(), FileMode.Open))
            {
                var settings = new ResizeSettings(resizeSettings);
                settings.Quality = quality;
                ImageBuilder.Current.Build(fileSource, absFile, settings);
            }
            return imageUrl;
        }
    }
}
