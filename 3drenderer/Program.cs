using _3drenderer;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

const int width = 1000;
const int height = 1000;

var rand = new Random();

Bitmap image = new Bitmap(width + 1, height + 1);

var model = Model.Load("Assets//head.obj");

for (int i = 0; i < model.Faces.Count(); i++)
{
    var face = model.Faces[i];
    var screenCoords = new MyVector2[3];
    for (int j = 0; j < face.Count(); j++)
    {
        var wordCoords = model.Vertices[face[j]];
        screenCoords[j] = new MyVector2((int)((wordCoords.X + 1) * width / 2), (int)((wordCoords.Y + 1) * height / 2));
    }

    DrawTriangle(screenCoords[0], screenCoords[1], screenCoords[2], image, Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
}

image.RotateFlip(RotateFlipType.RotateNoneFlipY); // I want to have the origin at the left bottom corner of the image

Stream stream = File.Create("output.bmp");
image.Save(stream, ImageFormat.Bmp);
stream.Close();


void DrawTriangle(MyVector2 t0, MyVector2 t1, MyVector2 t2, Bitmap image, Color color)
{
    if (t0.Y > t1.Y) Swap(ref t0, ref t1);
    if (t0.Y > t2.Y) Swap(ref t0, ref t2);
    if (t1.Y > t2.Y) Swap(ref t1, ref t2);

    int total_height = t2.Y - t0.Y;

    for (int i = 0; i < total_height; i++)
    {
        bool isSecondHalf = i > t1.Y - t0.Y || t1.Y == t0.Y;

        int segment_height = isSecondHalf ? t2.Y - t1.Y : t1.Y - t0.Y;

        float alpha = (float) i / total_height;
        float beta = (float)(i - (isSecondHalf ? t1.Y - t0.Y : 0)) / segment_height;

        int ax = (int)(t0.X + (t2.X - t0.X) * alpha);
        int bx = isSecondHalf ? (int)(t1.X + (t2.X - t1.X) * beta) : (int)(t0.X + (t1.X - t0.X) * beta);

        if (ax > bx) Swap(ref ax, ref bx);

        for (int j = ax; j <= bx; j++)
        {
            image.SetPixel(j, t0.Y + i, color);
        }
    }
}

void DrawLine(int x0, int y0, int x1, int y1, Bitmap image, Color color)
{
    bool isSteep = false;

    if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
    {
        Swap(ref x0, ref y0);
        Swap(ref x1, ref y1);

        isSteep = true;
    }

    if (x0 > x1)
    {
        Swap(ref x0, ref x1);
        Swap(ref y0, ref y1);
    }

    int dx = x1 - x0;
    int dy = y1 - y0;

    double error = 0;
    double derror = Math.Abs(dy) * 2;
    int y = y0;

    for (int x = x0; x <= x1; x++)
    {
        if(isSteep)
            image.SetPixel(y, x, color); // if transposed, de−transpose 
        else
            image.SetPixel(x, y, color);

        error += derror;
        if(error > dx)
        {
            y += y1 > y0 ? 1 : -1;
            error -= dx * 2;
        }
    }
}

void Swap<T>(ref T x, ref T y) => (x, y) = (y, x);

