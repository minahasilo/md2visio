namespace md2visio.vsdx.@tool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VColorGenerator
    {
        // Method to calculate the brightness of a color
        private static double CalculateBrightness(int r, int g, int b)
        {
            return 0.299 * r + 0.587 * g + 0.114 * b;
        }

        // Method to generate a list of unique and distinguishable background colors
        public static List<Tuple<int, int, int>> Generate(int count)
        {
            var colors = new HashSet<Tuple<int, int, int>>();
            var random = new Random();

            while (colors.Count < count)
            {
                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);

                // Ensure the brightness is sufficient for readability with black text
                if (CalculateBrightness(r, g, b) > 128) // Adjust this threshold as needed
                {
                    colors.Add(new Tuple<int, int, int>(r, g, b));
                }
            }

            // Convert HashSet to List to return
            return colors.ToList();
        }
    }
}
