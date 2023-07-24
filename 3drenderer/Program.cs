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
    var screenCoords = new Tuple<int, int>[3];
    for (int j = 0; j < face.Count(); j++)
    {
        var wordCoords = model.Vertices[face[j]];
        screenCoords[j] = new Tuple<int, int>((int)((wordCoords.X + 1) * width / 2), (int)((wordCoords.Y + 1) * height / 2));
    }

    DrawTriangle(screenCoords[0], screenCoords[1], screenCoords[2], image, Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
}

image.RotateFlip(RotateFlipType.RotateNoneFlipY); // I want to have the origin at the left bottom corner of the image

Stream stream = File.Create("output.bmp");
image.Save(stream, ImageFormat.Bmp);
stream.Close();


void DrawTriangle(Tuple<int,int> t0, Tuple<int, int> t1, Tuple<int, int> t2, Bitmap image, Color color)
{
    if (t0.Item2 > t1.Item2) Swap(ref t0, ref t1);
    if (t0.Item2 > t2.Item2) Swap(ref t0, ref t2);
    if (t1.Item2 > t2.Item2) Swap(ref t1, ref t2);

    int total_height = t2.Item2 - t0.Item2;

    for (int i = 0; i < total_height; i++)
    {
        bool isSecondHalf = i > t1.Item2 - t0.Item2 || t1.Item2 == t0.Item2;

        int segment_height = isSecondHalf ? t2.Item2 - t1.Item2 : t1.Item2 - t0.Item2;

        float alpha = (float) i / total_height;
        float beta = (float)(i - (isSecondHalf ? t1.Item2 - t0.Item2 : 0)) / segment_height;

        int ax = (int)(t0.Item1 + (t2.Item1 - t0.Item1) * alpha);
        int bx = isSecondHalf ? (int)(t1.Item1 + (t2.Item1 - t1.Item1) * beta) : (int)(t0.Item1 + (t1.Item1 - t0.Item1) * beta);

        if (ax > bx) Swap(ref ax, ref bx);

        for (int j = ax; j <= bx; j++)
        {
            image.SetPixel(j, t0.Item2 + i, color);
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

