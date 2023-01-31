using System.Globalization;
using System.Numerics;

namespace _3drenderer
{
    internal class Model
    {
        public List<Vector3> Vertices { get; set; }
        public List<int[]> Faces { get; set; }

        public static Model Load(string fileName)
        {
            var model = new Model();
            
            model.Vertices = new List<Vector3>() { new Vector3() };
            model.Faces = new List<int[]>();

            var reader = new StreamReader(fileName);
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(str)) continue;

                var array = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (array[0] == "v")
                {
                    var x = float.Parse(array[1], CultureInfo.InvariantCulture);
                    var y = float.Parse(array[2], CultureInfo.InvariantCulture);
                    var z = float.Parse(array[3], CultureInfo.InvariantCulture);

                    var vertex = new Vector3(x, y, z);

                    model.Vertices.Add(vertex);
                }
                else if (array[0] == "f")
                {
                    var v1 = int.Parse(array[1].Split("/")[0]);
                    var v2 = int.Parse(array[2].Split("/")[0]);
                    var v3 = int.Parse(array[3].Split("/")[0]);
                    model.Faces.Add(new int[] { v1, v2, v3 });
                }
            }

            reader.Close();
            return model;
        }
    }
}
