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
    bool isSteep = false;

    if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
    {
        swap(ref x0, ref y0);
        swap(ref x1, ref y1);

        isSteep = true;
    }

    if (x0 > x1)
    {
        swap(ref x0, ref x1);
        swap(ref y0, ref y1);
    }

    for (int x = x0; x <= x1; x++)
    {
        double t = (x - x0)/((double)(x1 - x0));
        int y = (int)(y0 + (y1 - y0) * t);

        if(isSteep)
            image.SetPixel(y, x, color); // if transposed, de−transpose 
        else
            image.SetPixel(x, y, color);

    }
}

void swap<T>(ref T x, ref T y) => (x, y) = (y, x);

