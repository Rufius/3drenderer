using System.Drawing;
using System.Drawing.Imaging;

int width = 100;
int height = 100;

Bitmap image = new Bitmap(width, height);

DrawLine(13, 20, 80, 40, image, Color.White);

image.RotateFlip(RotateFlipType.RotateNoneFlipY); // I want to have the origin at the left bottom corner of the image


Stream stream = File.Create("output.bmp");
image.Save(stream, ImageFormat.Bmp);
stream.Close();


void DrawLine(int x0, int y0, int x1, int y1, Bitmap image, Color color)
{
    const double dt = 0.01;

    for (double t = 0; t < 1.0; t += dt)
    {
        int x = (int)(x0 + (x1 - x0) * t);
        int y = (int)(y0 + (y1 - y0) * t);
        image.SetPixel(x, y, color);
    }
}

