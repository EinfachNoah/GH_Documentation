using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Drawing;

namespace NoahGrasshopper
{
    class ColorData
    {
        public static readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>
        {
            // --- Wand ---
            { "Außenputz Wand", Color.FromArgb(193,205,205) },   // azure
            { "Dämmung Wand",  Color.FromArgb(238,201,00) },    // gelblich
            { "Stahlbeton Wand", Color.FromArgb(139,134,130) },  // gräulich
            { "Innenspachtel Wand", Color.FromArgb(255,250,250) }, //

            // --- Dach ---
            { "Dämmung Dach", Color.FromArgb(238,201,0) },
            { "Stahlbeton Dach", Color.FromArgb(139,134,130) },  // gleiche Farbe wie Wand
            { "Splittschüttung Dach", Color.FromArgb(160, 150, 140) },
            { "Innenspachtel Dach", Color.FromArgb(255,250,250) }, // gleiche Farbe wie Wand

            // --- Decke ---
            { "Stahlbeton Decke", Color.FromArgb(139,134,130) },   // Mittelgrau
            { "Splittschüttung Decke", Color.FromArgb(160, 150, 140) },
            { "Trittschalldämmung Decke", Color.FromArgb(200, 220, 230) },
            { "Estrich Decke", Color.FromArgb(139,90,43) },
            { "Innenspachtel Decke", Color.FromArgb(255,250,250) }
        };
    }
}
