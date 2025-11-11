using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace NoahGrasshopper
{
    public class StahlbetonwandParameterComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StahlbetonwandParameterComponent class.
        /// </summary>
        public StahlbetonwandParameterComponent()
          : base("Wand", "Wandparameter",
              "Description",
              "Stahlbetonbauteile", "Parameter")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Außenputz", "AP", "Außenputz thickness in m", GH_ParamAccess.item, 0.02);
            pManager.AddNumberParameter("Dämmung", "D", "Dämmung thickness in m", GH_ParamAccess.item, 0.12);
            pManager.AddNumberParameter("Stahlbeton", "SB", "Stahlbeton thickness in m", GH_ParamAccess.item, 0.18);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "D", "Stahlbetonwand data", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double aussenputz = 0;
            double daemmung = 0;
            double stahlbeton = 0;
            DA.GetData("Außenputz", ref aussenputz);
            DA.GetData("Dämmung", ref daemmung);
            DA.GetData("Stahlbeton", ref stahlbeton);
            var data = new StahlbetonwandData
            {
                Aussenputz = aussenputz,
                Daemmung = daemmung,
                Stahlbeton = stahlbeton
            };
            DA.SetData("data", data);
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
            get { return new Guid("47B2AF10-DD7D-4DD5-84B9-5A0C18335D94"); }
        }
    }
}