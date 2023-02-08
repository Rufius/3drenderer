using System.Drawing;
using System.Drawing.Imaging;

const int width = 200;
const int height = 200;

Bitmap image = new Bitmap(width, height);

var triangle0 = new Tuple<int, int>[3]
{
    new Tuple<int, int> (10, 70),
    new Tuple<int, int> (50, 160),
    new Tuple<int, int> (70, 80)
};

var triangle1 = new Tuple<int, int>[3]
{
    new Tuple<int, int> (180, 50),
    new Tuple<int, int> (150, 1),
    new Tuple<int, int> (70, 180)
};

var triangle2 = new Tuple<int, int>[3]
{
    new Tuple<int, int> (180, 150),
    new Tuple<int, int> (120, 160),
    new Tuple<int, int> (130, 180)
};

DrawTriangle(triangle0[0], triangle0[1], triangle0[2], image, Color.Red);
DrawTriangle(triangle1[0], triangle1[1], triangle1[2], image, Color.White);
DrawTriangle(triangle2[0], triangle2[1], triangle2[2], image, Color.Green);

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

    int segment_height1 = t1.Item2 - t0.Item2 + 1;

    for (int y = t0.Item2; y <= t1.Item2; y++)
    {
        float alpha = (float)(y - t0.Item2) / total_height;
        float betha = (float)(y - t0.Item2) / segment_height1;

        int ax = (int)(t0.Item1 + (t2.Item1 - t0.Item1) * alpha);
        int bx = (int)(t0.Item1 + (t1.Item1 - t0.Item1) * betha);

        if (ax > bx) Swap(ref ax, ref bx);

        for (int j = ax; j <= bx; j++)
        {
            image.SetPixel(j, y, color);
        }
    }

    int segment_height2 = t2.Item2 - t1.Item2 + 1;

    for (int y = t1.Item2; y <= t2.Item2; y++)
    {
        float alpha = (float)(y - t0.Item2) / total_height;
        float betha = (float)(y - t1.Item2) / segment_height2;

        int ax = (int)(t0.Item1 + (t2.Item1 - t0.Item1) * alpha);
        int bx = (int)(t1.Item1 + (t2.Item1 - t1.Item1) * betha);

        if (ax > bx) Swap(ref ax, ref bx);

        for (int j = ax; j <= bx; j++)
        {
            image.SetPixel(j, y, color);
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

