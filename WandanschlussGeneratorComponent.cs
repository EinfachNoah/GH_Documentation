using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Render.ChangeQueue;

namespace NoahGrasshopper
{

    public class WandanschlussGeneratorComponent : GH_Component
    {

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        public WandanschlussGeneratorComponent()
          : base("Wandanschlussgenerator", "Wandanschlussgenerator",
              "Description",
              "Stahlbetonbauteile", "Generatoren")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("IN_L_Stahlbetonwand", "ILW", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("IN_R_Stahlbetonwand", "IRW", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("IN_Length", "ILL", "", GH_ParamAccess.item);
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


            StahlbetonwandData stahlbetonwandDataL = null;
            StahlbetonwandData stahlbetonwandDataR = null;
            double length = 0;
            DA.GetData("IN_L_Stahlbetonwand", ref stahlbetonwandDataL);
            DA.GetData("IN_R_Stahlbetonwand", ref stahlbetonwandDataR);
            DA.GetData("IN_Length", ref length);

            double tolerance = 0.05;
            double start_LW = 0;
            double start_RW = 0;

            double y_cut = stahlbetonwandDataL.Stahlbeton + stahlbetonwandDataL.Daemmung + stahlbetonwandDataL.Aussenputz + tolerance;
            double x_cut = stahlbetonwandDataR.Stahlbeton + stahlbetonwandDataR.Daemmung + stahlbetonwandDataR.Aussenputz + tolerance;


            Box cube_lw_1 = new Box(Plane.WorldXY, new Interval(0, x_cut), new Interval(start_LW, stahlbetonwandDataL.Aussenputz), new Interval(0, length));
            data.AddGeometry("Außenputz Wand", cube_lw_1, ColorData.Colors["Außenputz Wand"]);

            start_LW += stahlbetonwandDataL.Aussenputz;

            Box cube_lw_2 = new Box(Plane.WorldXY, new Interval(stahlbetonwandDataR.Aussenputz, x_cut), new Interval(start_LW, start_LW + stahlbetonwandDataL.Daemmung), new Interval(0, length));
            data.AddGeometry("Dämmung Wand", cube_lw_2, ColorData.Colors["Dämmung Wand"]);

            start_LW += stahlbetonwandDataL.Daemmung;


            Box cube_lw_3 = new Box(Plane.WorldXY, new Interval(stahlbetonwandDataR.Daemmung + stahlbetonwandDataR.Aussenputz, x_cut), new Interval(start_LW, start_LW + stahlbetonwandDataL.Stahlbeton), new Interval(0, length));
            data.AddGeometry("Stahlbeton Wand", cube_lw_3, ColorData.Colors["Stahlbeton Wand"]);



            Box cube_rw_1 = new Box(Plane.WorldXY, new Interval(start_RW, start_RW + stahlbetonwandDataR.Aussenputz), new Interval(stahlbetonwandDataL.Aussenputz, y_cut), new Interval(0, length));
            data.AddGeometry("Außenputz Wand", cube_rw_1, ColorData.Colors["Außenputz Wand"]);

            start_RW += stahlbetonwandDataR.Aussenputz;



            Box cube_rw_2 = new Box(Plane.WorldXY, new Interval(start_RW, start_RW + stahlbetonwandDataR.Daemmung), new Interval(stahlbetonwandDataL.Aussenputz + stahlbetonwandDataL.Daemmung, y_cut), new Interval(0, length));
            data.AddGeometry("Dämmung Wand", cube_rw_2, ColorData.Colors["Dämmung Wand"]);

            start_RW += stahlbetonwandDataR.Daemmung;


            Box cube_rw_3 = new Box(Plane.WorldXY, new Interval(start_RW, start_RW + stahlbetonwandDataR.Stahlbeton), new Interval(stahlbetonwandDataL.Aussenputz + stahlbetonwandDataL.Daemmung + stahlbetonwandDataL.Stahlbeton, y_cut), new Interval(0, length));
            data.AddGeometry("Stahlbeton Wand", cube_rw_3, ColorData.Colors["Stahlbeton Wand"]);


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
            get { return new Guid("9B19E0D2-9561-4562-ADDD-40B8AF9FE494"); }
        }
    }
}
