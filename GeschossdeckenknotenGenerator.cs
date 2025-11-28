using System;
using System.Collections.Generic;
using Eto.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace NoahGrasshopper
{
    public class GeschossdeckenknotenGenerator : GH_Component
    {

        /// <summary>
        /// Initializes a new instance of the Cube class.
        /// </summary>
        public GeschossdeckenknotenGenerator()
          : base("GeschossdeckenknotenGenerator", "GeschossdeckenknotenGenerator",
              "Description",
              "Stahlbetonbauteile", "Generatoren")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("IN_B_LayerThicknesses", "ILB", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_OWR_LayerThicknesses", "ILOWR", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_UWR_LayerThicknesses", "ILUWR", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_OWL_LayerThicknesses", "ILOWL", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_UWL_LayerThicknesses", "ILUWL", "", GH_ParamAccess.item);


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
            StahlbetondeckeData layerThicknesses_BR = null;
            StahlbetondeckeData layerThicknesses_BL = null;
            StahlbetonwandData layerThicknesses_OWR = null;
            StahlbetonwandData layerThicknesses_UWR = null;
            StahlbetonwandData layerThicknesses_OWL = null;
            StahlbetonwandData layerThicknesses_UWL = null;

            DA.GetData("IN_B_LayerThicknesses", ref layerThicknesses_BR);
            DA.GetData("IN_B_LayerThicknesses", ref layerThicknesses_BL);
            DA.GetData("IN_OWR_LayerThicknesses", ref layerThicknesses_OWR);
            DA.GetData("IN_UWR_LayerThicknesses", ref layerThicknesses_UWR);
            DA.GetData("IN_OWL_LayerThicknesses", ref layerThicknesses_OWL);
            DA.GetData("IN_UWL_LayerThicknesses", ref layerThicknesses_UWL);


            double x_offset_OWR = 0;
            double x_offset_UWR = 0;
            double cut_offset_UWR = 0;
            double cut_offset_OWR = 0;

            double x_offset_OWL = 0;
            double x_offset_UWL = 0;

            double x_cutR = 0;
            double x_cutUWR = layerThicknesses_UWR.Stahlbeton + layerThicknesses_UWR.Daemmung + layerThicknesses_UWR.Aussenputz;
            double x_cutOWR = layerThicknesses_OWR.Stahlbeton + layerThicknesses_OWR.Daemmung + layerThicknesses_OWR.Aussenputz;
            double length = x_cutUWR + 0.05;
            double maxYR = length;
            double minXL = 0;
            double maxXL = maxYR;

            if (x_cutOWR <= x_cutUWR)
            {
                x_cutR = x_cutUWR;
            }
            else
            {
                x_cutR = x_cutOWR;
            }

            double z_cutR = layerThicknesses_BR.Stahlbeton + layerThicknesses_BR.Splittschuettung + layerThicknesses_BR.Trittschalldaemmung + layerThicknesses_BR.Estrich + 0.05;

            double x_cutL = 0;

            if (layerThicknesses_OWL.Stahlbeton <= layerThicknesses_UWL.Stahlbeton)
            {
                x_cutL = layerThicknesses_UWL.Stahlbeton + layerThicknesses_UWL.Daemmung + layerThicknesses_UWL.Aussenputz;
            }
            else
            {
                x_cutL = layerThicknesses_OWL.Stahlbeton + layerThicknesses_OWL.Daemmung + layerThicknesses_OWL.Aussenputz;
            }

            double z_cutL = layerThicknesses_BL.Stahlbeton + layerThicknesses_BL.Splittschuettung + layerThicknesses_BL.Trittschalldaemmung + layerThicknesses_BL.Estrich + 0.05;


            Box cube_UWR_1 = new Box(Plane.WorldXY, new Interval(x_offset_UWR, x_offset_UWR + layerThicknesses_UWR.Aussenputz), new Interval(layerThicknesses_UWL.Aussenputz, maxYR), new Interval(-layerThicknesses_BR.Innenspachtel - 0.05, 0));
            data.AddGeometry("Außenputz Wand", cube_UWR_1, ColorData.Colors["Außenputz Wand"]);

            x_offset_UWR += layerThicknesses_UWR.Aussenputz;    
            cut_offset_UWR += layerThicknesses_UWL.Aussenputz;

            Box cube_UWR_2 = new Box(Plane.WorldXY, new Interval(x_offset_UWR, x_offset_UWR + layerThicknesses_UWR.Daemmung), new Interval(cut_offset_UWR + layerThicknesses_UWL.Daemmung, maxYR), new Interval(-layerThicknesses_BR.Innenspachtel - 0.05, 0));
            data.AddGeometry("Dämmung Wand", cube_UWR_2, ColorData.Colors["Dämmung Wand"]);

            x_offset_UWR += layerThicknesses_UWR.Daemmung;
            cut_offset_UWR += layerThicknesses_UWL.Daemmung;

            Box cube_UWR_3 = new Box(Plane.WorldXY, new Interval(x_offset_UWR, x_offset_UWR + layerThicknesses_UWR.Stahlbeton), new Interval(cut_offset_UWR + layerThicknesses_UWL.Stahlbeton, maxYR), new Interval(-layerThicknesses_BR.Innenspachtel - 0.05, 0));
            data.AddGeometry("Stahlbeton Wand", cube_UWR_3, ColorData.Colors["Stahlbeton Wand"]);


            Box cube_OWR_1 = new Box(Plane.WorldXY, new Interval(x_offset_OWR, x_offset_OWR + layerThicknesses_OWR.Aussenputz), new Interval(cut_offset_OWR + layerThicknesses_OWL.Aussenputz, maxYR), new Interval(0, z_cutR));
            data.AddGeometry("Außenputz Wand", cube_OWR_1, ColorData.Colors["Außenputz Wand"]);

            x_offset_OWR += layerThicknesses_OWR.Aussenputz;
            cut_offset_OWR += layerThicknesses_OWL.Aussenputz;

            Box cube_OWR_2 = new Box(Plane.WorldXY, new Interval(x_offset_OWR, x_offset_OWR + layerThicknesses_OWR.Daemmung), new Interval(cut_offset_OWR + layerThicknesses_OWL.Daemmung, maxYR), new Interval(0, z_cutR));
            data.AddGeometry("Dämmung Wand", cube_OWR_2, ColorData.Colors["Dämmung Wand"]);

            x_offset_OWR += layerThicknesses_OWR.Daemmung;
            cut_offset_OWR += layerThicknesses_OWL.Daemmung;

            Box cube_OWR_3 = new Box(Plane.WorldXY, new Interval(x_offset_OWR, x_offset_OWR + layerThicknesses_OWR.Stahlbeton), new Interval(cut_offset_OWR + layerThicknesses_OWL.Stahlbeton, maxYR), new Interval(layerThicknesses_BR.Stahlbeton, z_cutR));
            data.AddGeometry("Stahlbeton Wand", cube_OWR_3, ColorData.Colors["Stahlbeton Wand"]);


            //// LEFT EDGE  
            ///

            

            Box cube_UWL_1 = new Box(Plane.WorldXY,new Interval(minXL, maxXL), new Interval(x_offset_UWL, x_offset_UWL + layerThicknesses_UWL.Aussenputz), new Interval( -layerThicknesses_BL.Innenspachtel - 0.05, 0));
            data.AddGeometry("Außenputz Wand", cube_UWL_1, ColorData.Colors["Außenputz Wand"]);

            x_offset_UWL += layerThicknesses_UWL.Aussenputz;

            Box cube_UWL_2 = new Box(Plane.WorldXY, new Interval(layerThicknesses_UWR.Aussenputz, maxXL), new Interval(x_offset_UWL, x_offset_UWL + layerThicknesses_UWL.Daemmung), new Interval(-layerThicknesses_BL.Innenspachtel - 0.05, 0));
            data.AddGeometry("Dämmung Wand", cube_UWL_2, ColorData.Colors["Dämmung Wand"]);

            x_offset_UWL += layerThicknesses_UWL.Daemmung;

            Box cube_UWL_3 = new Box(Plane.WorldXY, new Interval(layerThicknesses_UWR.Aussenputz + layerThicknesses_UWR.Daemmung, maxXL), new Interval(x_offset_UWL, x_offset_UWL + layerThicknesses_UWL.Stahlbeton), new Interval(-layerThicknesses_BL.Innenspachtel - 0.05, 0));
            data.AddGeometry("Stahlbeton Wand", cube_UWL_3, ColorData.Colors["Stahlbeton Wand"]);

            Box cube_OWL_1 = new Box(Plane.WorldXY, new Interval(minXL, maxXL), new Interval(x_offset_OWL, x_offset_OWL + layerThicknesses_OWL.Aussenputz), new Interval(0, z_cutL));
            data.AddGeometry("Außenputz Wand", cube_OWL_1, ColorData.Colors["Außenputz Wand"]);

            x_offset_OWL += layerThicknesses_OWL.Aussenputz;

            Box cube_OWL_2 = new Box(Plane.WorldXY, new Interval(layerThicknesses_OWR.Aussenputz, maxXL), new Interval(x_offset_OWL, x_offset_OWL + layerThicknesses_OWL.Daemmung), new Interval(0, z_cutL));
            data.AddGeometry("Dämmung Wand", cube_OWL_2, ColorData.Colors["Dämmung Wand"]);

            x_offset_OWL += layerThicknesses_OWL.Daemmung;

            Box cube_OWL_3 = new Box(Plane.WorldXY, new Interval(layerThicknesses_OWR.Aussenputz + layerThicknesses_OWR.Daemmung, maxXL), new Interval(x_offset_OWL, x_offset_OWL + layerThicknesses_OWL.Stahlbeton), new Interval(layerThicknesses_BL.Stahlbeton, z_cutR));
            data.AddGeometry("Stahlbeton Wand", cube_OWL_3, ColorData.Colors["Stahlbeton Wand"]);

            double x_offset_UBL = x_offset_UWL + layerThicknesses_UWL.Stahlbeton;

            Box cube_UBL_1 = new Box(Plane.WorldXY, new Interval(x_cutUWR, maxXL), new Interval(x_offset_UBL, maxYR), new Interval(-layerThicknesses_BL.Innenspachtel, 0));
            data.AddGeometry("Innenspachtel Decke", cube_UBL_1, ColorData.Colors["Innenspachtel Decke"]);


            double x_offset_OBL = x_offset_OWL;

            double z_offset_OBL = 0;

            Box cube_OBL_1 = new Box(Plane.WorldXY, new Interval(x_cutOWR - layerThicknesses_OWR.Stahlbeton, maxXL), new Interval(x_offset_OBL, maxYR), new Interval(z_offset_OBL, z_offset_OBL + layerThicknesses_BL.Stahlbeton));
            data.AddGeometry("Stahlbeton Decke", cube_OBL_1, ColorData.Colors["Stahlbeton Decke"]);
            z_offset_OBL += layerThicknesses_BL.Stahlbeton;

            Box cube_OBL_2 = new Box(Plane.WorldXY, new Interval(x_cutOWR, maxXL), new Interval(x_offset_OBL + layerThicknesses_OWL.Stahlbeton, maxYR),new Interval(z_offset_OBL, z_offset_OBL + layerThicknesses_BL.Splittschuettung));
            data.AddGeometry("Splittschüttung Decke", cube_OBL_2, ColorData.Colors["Splittschüttung Decke"]);
            z_offset_OBL += layerThicknesses_BL.Splittschuettung;

            Box cube_OBL_3 = new Box(Plane.WorldXY, new Interval(x_cutOWR, maxXL), new Interval(x_offset_OBL + layerThicknesses_OWL.Stahlbeton, maxYR), new Interval(z_offset_OBL, z_offset_OBL + layerThicknesses_BL.Trittschalldaemmung));
            data.AddGeometry("Trittschalldämmung Decke", cube_OBL_3, ColorData.Colors["Trittschalldämmung Decke"]);
            z_offset_OBL += layerThicknesses_BL.Trittschalldaemmung;

            Box cube_OBL_4 = new Box(Plane.WorldXY, new Interval(x_cutOWR, maxXL), new Interval(x_offset_OBL + layerThicknesses_OWL.Stahlbeton, maxYR),new Interval(z_offset_OBL, z_offset_OBL + layerThicknesses_BL.Estrich));
            data.AddGeometry("Estrich Decke", cube_OBL_4, ColorData.Colors["Estrich Decke"]);


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
            get { return new Guid("CACB05FA-CD90-4AEC-8046-8E1DDAB844D9"); }
        }
    }
}
