using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace NoahGrasshopper
{
    public class StahlbetondeckeParameterComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Stahlbetondecke class.
        /// </summary>
        public StahlbetondeckeParameterComponent()
          : base("Geschossdecke", "Geschossdeckenparameter",
              "Description",
              "Stahlbetonbauteile", "Parameter")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Estrich", "E", "Estrich thickness in m", GH_ParamAccess.item, 0.06);
            pManager.AddNumberParameter("Trittschalldaemmung", "T", "Trittschalldaemmung thickness in m", GH_ParamAccess.item, 0.03);
            pManager.AddNumberParameter("Splittschuettung", "S", "Splittschuettung thickness in m", GH_ParamAccess.item, 0.05);
            pManager.AddNumberParameter("Stahlbeton", "SB", "Stahlbeton thickness in m", GH_ParamAccess.item, 0.18);
            pManager.AddNumberParameter("Innenspachtel", "IS", "Innenspachtel thickness in m", GH_ParamAccess.item, 0.002);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "D", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double estrich = 0;
            double trittschaalldaemmung = 0;
            double splittschuettung = 0;
            double stahlbeton = 0;
            double innenspachtel = 0;

            DA.GetData("Estrich", ref estrich);
            DA.GetData("Trittschalldaemmung", ref trittschaalldaemmung);
            DA.GetData("Splittschuettung", ref splittschuettung);
            DA.GetData("Stahlbeton", ref stahlbeton);
            DA.GetData("Innenspachtel", ref innenspachtel);

            var data = new StahlbetondeckeData
            {
                Estrich = estrich,
                Trittschalldaemmung = trittschaalldaemmung,
                Splittschuettung = splittschuettung,
                Stahlbeton = stahlbeton,
                Innenspachtel = innenspachtel
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
            get { return new Guid("3D238FFE-63E6-4986-953A-3FE04DE903A1"); }
        }
    }
}