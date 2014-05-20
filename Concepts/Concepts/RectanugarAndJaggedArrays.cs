using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    public static class RectanugarAndJaggedArrays
    {
        public static void Demo()
        {
            //Ranks of Rectanugar and Jagged arrays
            int[, ,] rectangularArray = new int[5, 2, 3];
            int rank = rectangularArray.Rank;

            int[][][] jaggedArray = new int[5][][];
            int rank2 = jaggedArray.Rank;
        }
    }
}
