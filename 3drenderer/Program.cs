using _3drenderer;
using System.Drawing;
using System.Drawing.Imaging;

const int width = 1000;
const int height = 1000;

Bitmap image = new Bitmap(width+1, height+1);

var model = Model.Load("Assets//head.obj");

for (int i = 0; i < model.Faces.Count(); i++)
{
    var face = model.Faces[i];
    for (int j = 0; j < face.Count(); j++)
    {
        var v0 = model.Vertices[face[j]];
        var v1 = model.Vertices[face[(j+1)%face.Count()]];

        var x0 = (int)(width/2 + v0.X*(width/2));
        var y0 = (int)(height/2 + v0.Y*(height/2));

        var x1 = (int)(width /2 + v1.X*(width/2));
        var y1 = (int)(height /2 + v1.Y*(height/2));

        DrawLine(x0, y0, x1, y1, image, Color.White);
    }
}

image.RotateFlip(RotateFlipType.RotateNoneFlipY); // I want to have the origin at the left bottom corner of the image

Stream stream = File.Create("output.bmp");
image.Save(stream, ImageFormat.Bmp);
stream.Close();


void DrawTriangle(Tuple<int,int> t0, Tuple<int, int> t1, Tuple<int, int> t2, Bitmap image, Color color)
{
    DrawLine(t0.Item1, t0.Item2, t1.Item1, t1.Item2, image, color);
    DrawLine(t1.Item1, t1.Item2, t2.Item1, t2.Item2, image, color);
    DrawLine(t2.Item1, t2.Item2, t0.Item1, t0.Item2, image, color);
}

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

void swap<T>(ref T x, ref T y) => (x, y) = (y, x);

