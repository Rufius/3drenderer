using System.Drawing;
using System.Drawing.Imaging;

int width = 100;
int height = 100;

Bitmap image = new Bitmap(width, height);
image.SetPixel(52, 41, Color.Red);
image.RotateFlip(RotateFlipType.RotateNoneFlipY); // I want to have the origin at the left bottom corner of the image


Stream stream = File.Create("output.bmp");
image.Save(stream, ImageFormat.Bmp);
stream.Close();

