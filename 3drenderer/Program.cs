using System.Drawing;
using System.Drawing.Imaging;

int width = 100;
int height = 100;

Bitmap image = new Bitmap(width, height);

DrawLine(13, 20, 80, 40, image, Color.White);
DrawLine(20, 13, 40, 80, image, Color.Red);
DrawLine(80, 40, 13, 20, image, Color.Red);

image.RotateFlip(RotateFlipType.RotateNoneFlipY); // I want to have the origin at the left bottom corner of the image


Stream stream = File.Create("output.bmp");
image.Save(stream, ImageFormat.Bmp);
stream.Close();


void DrawLine(int x0, int y0, int x1, int y1, Bitmap image, Color color)
{
    for (int x = x0; x <= x1; x++)
    {
        double t = (x - x0)/((double)(x1 - x0));
        int y = (int)(y0 + (y1 - y0) * t);
        image.SetPixel(x, y, color);
    }
}

