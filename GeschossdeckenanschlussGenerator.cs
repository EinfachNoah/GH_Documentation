using System;
using System.Collections.Generic;
using Eto.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace NoahGrasshopper
{
    public class Geschossdeckenanschluss : GH_Component
    {
    
        /// <summary>
        /// Initializes a new instance of the Cube class.
        /// </summary>
        public Geschossdeckenanschluss()
          : base("Geschossdeckenanschluss", "Geschossdeckenanschluss",
              "Description",
              "Stahlbetonbauteile", "Generatoren")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("IN_B_Length", "IL", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_B_LayerThicknesses", "ILB", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_OW_LayerThicknesses", "ILOW", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_UW_LayerThicknesses", "ILUW", "", GH_ParamAccess.item);


        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("LayerBreps", "B", "Alle Schichten als Breps", GH_ParamAccess.list);
            pManager.AddColourParameter("LayerColors", "C", "Farben der Breps", GH_ParamAccess.list);
            pManager.AddTextParameter("Names_List", "N", "", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryData data = new GeometryData();
            StahlbetondeckeData layerThicknesses_B = null;
            StahlbetonwandData layerThicknesses_OW = null;
            StahlbetonwandData layerThicknesses_UW = null;
            double length = 0;

            DA.GetData("IN_B_LayerThicknesses", ref layerThicknesses_B);
            DA.GetData("IN_OW_LayerThicknesses", ref layerThicknesses_OW);
            DA.GetData("IN_UW_LayerThicknesses", ref layerThicknesses_UW);
            DA.GetData("IN_B_Length", ref length);

            double minY = 0;
            double maxY = length;

            double x_offset_OW = 0;
            double x_offset_UW = 0;

            double x_cut = 0;

            if (layerThicknesses_OW.Stahlbeton <= layerThicknesses_UW.Stahlbeton)
            {
                x_cut = layerThicknesses_UW.Stahlbeton + layerThicknesses_UW.Daemmung + layerThicknesses_UW.Aussenputz;
            }
            else
            {
                x_cut = layerThicknesses_OW.Stahlbeton + layerThicknesses_OW.Daemmung + layerThicknesses_OW.Aussenputz;
            }

            double z_cut = layerThicknesses_B.Stahlbeton  + layerThicknesses_B.Splittschuettung + layerThicknesses_B.Trittschalldaemmung + layerThicknesses_B.Estrich + 0.05;

            Box cube_uw_1 = new Box(Plane.WorldXY, new Interval(x_offset_UW, x_offset_UW + layerThicknesses_UW.Aussenputz), new Interval(minY, maxY), new Interval( - layerThicknesses_B.Innenspachtel - 0.05, 0));
            data.AddGeometry("Außenputz Wand", cube_uw_1, ColorData.Colors["Außenputz Wand"]);

            x_offset_UW += layerThicknesses_UW.Aussenputz;
            
            Box cube_uw_2 = new Box(Plane.WorldXY, new Interval(x_offset_UW, x_offset_UW + layerThicknesses_UW.Daemmung), new Interval(minY, maxY), new Interval( - layerThicknesses_B.Innenspachtel - 0.05, 0));
            data.AddGeometry("Dämmung Wand", cube_uw_2, ColorData.Colors["Dämmung Wand"]);

            x_offset_UW += layerThicknesses_UW.Daemmung;
            
            Box cube_uw_3 = new Box(Plane.WorldXY, new Interval(x_offset_UW, x_offset_UW + layerThicknesses_UW.Stahlbeton), new Interval(minY, maxY), new Interval(- layerThicknesses_B.Innenspachtel - 0.05, -layerThicknesses_B.Innenspachtel));
            data.AddGeometry("Stahlbeton Wand", cube_uw_3, ColorData.Colors["Stahlbeton Wand"]);

            Box cube_ow_1 = new Box(Plane.WorldXY, new Interval(x_offset_OW, x_offset_OW + layerThicknesses_OW.Aussenputz), new Interval(minY, maxY), new Interval(0, z_cut));
            data.AddGeometry("Außenputz Wand", cube_ow_1, ColorData.Colors["Außenputz Wand"]);

            x_offset_OW += layerThicknesses_OW.Aussenputz;

            Box cube_ow_2 = new Box(Plane.WorldXY, new Interval(x_offset_OW, x_offset_OW + layerThicknesses_OW.Daemmung), new Interval(minY, maxY), new Interval(0, z_cut));
            data.AddGeometry("Dämmung Wand", cube_ow_2, ColorData.Colors["Dämmung Wand"]);

            x_offset_OW += layerThicknesses_OW.Daemmung;

            Box cube_ow_3 = new Box(Plane.WorldXY, new Interval(x_offset_OW, x_offset_OW + layerThicknesses_OW.Stahlbeton), new Interval(minY, maxY), new Interval(layerThicknesses_B.Stahlbeton, z_cut));
            data.AddGeometry("Stahlbeton Wand", cube_ow_3, ColorData.Colors["Stahlbeton Wand"]);

            double x_offset_UB = x_offset_UW;

            Box cube_ub_1 = new Box(Plane.WorldXY, new Interval(x_offset_UB, x_cut + 0.05), new Interval(minY, maxY), new Interval(-layerThicknesses_B.Innenspachtel, 0));
            data.AddGeometry("Innenspachtel Decke", cube_ub_1, ColorData.Colors["Innenspachtel Decke"]);

            double x_offset_OB = x_offset_OW;

            double z_offset = 0;

            Box cube_ob_1 = new Box(Plane.WorldXY, new Interval(x_offset_OB, x_cut + 0.05), new Interval(minY, maxY), new Interval(z_offset, z_offset + layerThicknesses_B.Stahlbeton));
            data.AddGeometry("Stahlbeton Decke", cube_ob_1, ColorData.Colors["Stahlbeton Decke"]);
            z_offset += layerThicknesses_B.Stahlbeton;

            Box cube_ob_2 = new Box(Plane.WorldXY,new Interval(x_offset_OB + layerThicknesses_OW.Stahlbeton, x_cut + 0.05), new Interval(minY, maxY), new Interval(z_offset, z_offset + layerThicknesses_B.Splittschuettung));
            data.AddGeometry("Splittschüttung Decke", cube_ob_2, ColorData.Colors["Splittschüttung Decke"]);
            z_offset += layerThicknesses_B.Splittschuettung;

            Box cube_ob_3 = new Box( Plane.WorldXY, new Interval(x_offset_OB + layerThicknesses_OW.Stahlbeton, x_cut + 0.05), new Interval(minY, maxY), new Interval(z_offset, z_offset + layerThicknesses_B.Trittschalldaemmung));
            data.AddGeometry("Trittschalldämmung Decke", cube_ob_3, ColorData.Colors["Trittschalldämmung Decke"]);
            z_offset += layerThicknesses_B.Trittschalldaemmung;

            Box cube_ob_4 = new Box(Plane.WorldXY, new Interval(x_offset_OB + layerThicknesses_OW.Stahlbeton, x_cut + 0.05), new Interval(minY, maxY), new Interval(z_offset, z_offset + layerThicknesses_B.Estrich));
            data.AddGeometry("Estrich Decke", cube_ob_4, ColorData.Colors["Estrich Decke"]);

            DA.SetDataList(0, data.Breps);   // Breps 
            DA.SetDataList(1, data.Colors);  // Farben 
            DA.SetDataList(2, data.Names); // Namen
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
            get { return new Guid("CACB05FA-CD90-4AEC-8046-8E1DDAA844D9"); }
        }
    }
}