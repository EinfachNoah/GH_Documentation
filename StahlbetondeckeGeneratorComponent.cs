using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Render.ChangeQueue;



namespace NoahGrasshopper
{
    public class StahlbetondeckeGeneratorComponent : GH_Component
    {

        /// <summary>
        /// Initializes a new instance of the geschossdecke_1 class.
        /// </summary>
        public StahlbetondeckeGeneratorComponent()
          : base("Geschossdecke", "Geschossdeckengenerator",
              "Description",
              "Stahlbetonbauteile", "Generatoren")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("IN_BoundaryCurve", "B", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_LayerThicknesses", "IS", "", GH_ParamAccess.item);
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
        /// <param name="DA">The DA object is used t    o retrieve from inputs and store in outputs.</param>
        /// 

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryData data = new GeometryData();
            StahlbetondeckeData stahlbetondeckeData = null;

            DA.GetData(1, ref stahlbetondeckeData);
            Curve baseCurve = null;
            DA.GetData(0, ref baseCurve);


            BoundingBox bbox = baseCurve.GetBoundingBox(true);
            double minY = bbox.Min.Y, maxY = bbox.Max.Y;
            double minZ = bbox.Min.Z;
            double minX = bbox.Min.X, maxX = bbox.Max.X;



            Box cube_d1 = new Box(Plane.WorldXY, new Interval(minX, maxX), new Interval(minY, maxY), new Interval(minZ, minZ + stahlbetondeckeData.Stahlbeton));
            data.AddGeometry("Stahlbeton Decke", cube_d1, ColorData.Colors["Stahlbeton Decke"]);

            minZ += stahlbetondeckeData.Stahlbeton;

            Box cube_d2 = new Box(Plane.WorldXY, new Interval(minX, maxX), new Interval(minY, maxY), new Interval(minZ, minZ + stahlbetondeckeData.Splittschuettung));
            data.AddGeometry("Splittschüttung Decke", cube_d2, ColorData.Colors["Splittschüttung Decke"]);

            minZ += stahlbetondeckeData.Splittschuettung;

            Box cube_d3 = new Box(Plane.WorldXY, new Interval(minX, maxX), new Interval(minY, maxY), new Interval(minZ, minZ + stahlbetondeckeData.Trittschalldaemmung));
            data.AddGeometry("Trittschalldämmung Decke", cube_d3, ColorData.Colors["Trittschalldämmung Decke"]);

            minZ += stahlbetondeckeData.Trittschalldaemmung;

            Box cube_d4 = new Box(Plane.WorldXY, new Interval(minX, maxX), new Interval(minY, maxY), new Interval(minZ, minZ + stahlbetondeckeData.Estrich));
            data.AddGeometry("Estrich Decke", cube_d4, ColorData.Colors["Estrich Decke"]);
            
            Box cube_du = new Box(Plane.WorldXY, new Interval(minX, maxX), new Interval(minY, maxY), new Interval(-stahlbetondeckeData.Innenspachtel, 0));
            data.AddGeometry("Innenspachtel Decke", cube_du, ColorData.Colors["Innenspachtel Decke"]);





            DA.SetDataList(0, data.Breps);   // Breps (für Preview)
            DA.SetDataList(1, data.Colors);  // Farben (GH_Colour → Preview)
            DA.SetDataList(2, data.Names);  // Namen (für Preview und Layer)

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("B7BB8D5B-789A-4B6D-8BBA-0C716F2F2B47"); }
        }
    }
}