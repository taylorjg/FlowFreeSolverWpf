using System;
using System.Reflection;
using System.Windows.Media;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf.View
{
    public static class DotColourExtensions
    {
        public static Color ToWpfColour(this DotColour dotColour)
        {
            var colourName = dotColour.ColourName;
            var colourNameWithNoSpaces = colourName.Replace(" ", "");

            var propertyInfo = typeof(Colors).GetProperty(colourNameWithNoSpaces, BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo != null)
            {
                var colour = (Color)propertyInfo.GetValue(null, BindingFlags.Public | BindingFlags.Static, null, new object[] {}, null);
                return colour;
            }

            throw new InvalidOperationException(string.Format("Unknown colour name, \"{0}\".", colourName));
        }
    }
}
