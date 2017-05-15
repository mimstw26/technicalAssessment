using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ImageResizer;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;

namespace GeneralKnowledge.Test.App.Tests
{
    /// <summary>
    /// Image rescaling
    /// </summary>
    public class RescaleImageTest : ITest
    {
        public void Run()
        {
            // TODO:
            // Grab an image from a public URL and write a function thats rescale the image to a desired format
            // The use of 3rd party plugins is permitted
            // For example: 100x80 (thumbnail) and 1200x1600 (preview)

            // For this I am using the ImageResizer Nuget package, found here: https://imageresizing.net/

            // getting the Google logo and setting the path where I would like for it to be stored.
            string url = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png";
            string localFilename = @"C:\temp\newImage.png";

            // check if the file already exists, added for ease while testing
            if (File.Exists(localFilename))
                File.Delete(localFilename);

            // download the file using the WebClient
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, localFilename);
            }

            if (File.Exists(@"C:\temp\resizedImage.png"))
                File.Delete(@"C:\temp\resizedImage.png");

            // ImageJob is provided by the ImageResizer package. ImageJob(source, destination, instructions for resize)
            ImageJob resizedImage = new ImageJob(localFilename, @"C:\temp\resizedImage.png", new Instructions
            {
                Width = 100,
                Height = 100
                //Mode = FitMode.Crop // cropped off the image, left only the middle oo's
            });
            resizedImage.Build();

        }
    }
}
