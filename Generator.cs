using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.Render.ChangeQueue;

namespace NoahGrasshopper
{
    public class Generator : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Generator()
          : base("Generator", "Generator",
            "Description",
            "Example", "Generatoren")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("IN_ExampleThickness", "IL", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("IN_Boundarycurve", "IB", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("OUT_GeneratedBreps", "G", "", GH_ParamAccess.list);
            pManager.AddColourParameter("OUT_LayerColors", "C", "", GH_ParamAccess.list);
            pManager.AddTextParameter("OUT_LayerNames", "N", "", GH_ParamAccess.list);

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryData data = new GeometryData();

            DataClass get_set_data = null;
            Curve baseCurve = null;

            DA.GetData(0, ref get_set_data);
            DA.GetData(1, ref baseCurve);

            BoundingBox bbox = baseCurve.GetBoundingBox(true);
            double minY = bbox.Min.Y, maxY = bbox.Max.Y;
            double minZ = bbox.Min.Z, maxZ = bbox.Max.Z;
            double minX = bbox.Min.X;

            Box cube = new Box(Plane.WorldXY, new Interval(minX, get_set_data.Example), new Interval(minY, maxY), new Interval(minZ, maxZ));
            data.AddGeometry("Cube", cube, Color.FromArgb(250, 250, 250));
                

            DA.SetDataList(0, data.Breps);   // Breps (für Preview)
            DA.SetDataList(1, data.Colors);  // Farben (GH_Colour → Preview)
            DA.SetDataList(2, data.Names);  // Namen (für Preview und Layer)

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("652610cf-7deb-4bdf-bda9-f576d13e326a");
    }
}