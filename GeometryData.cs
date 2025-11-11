using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NoahGrasshopper
{
    public class GeometryData
    {
        public List<Brep> Breps { get; } = new List<Brep>();
        public List<string> Names { get; } = new List<string>();
        public List<Color> Colors { get; } = new List<Color>();


        public void SetOutput(IGH_DataAccess DA, int parameterIndex)
        {
            DA.SetDataList("Geometry", Breps);
            DA.SetDataList("Names", Names);
            DA.SetDataList("Colors", Colors);
        }

        public void AddGeometry(string name, Brep geometry, Color color)
        {
            Breps.Add(geometry);
            Names.Add(name);
            Colors.Add(color);
        }

        public void AddGeometry(string name, Box geometry, Color color)
        {
            AddGeometry(name, geometry.ToBrep(), color);
        }


    }

}
