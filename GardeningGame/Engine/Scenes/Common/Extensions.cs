using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame.Utilities;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GardeningGame.Engine.Scenes.Common
{
    public static class Extensions
    {
        public static Color NextColor(this Random R)
        {
            return new Color(R.Next(255), R.Next(255), R.Next(255));
        }

        //func(v* Vect) Slerp(o* Vect, t Float) (slerp* Vect) {
        //  omega := v.Dot(o).Acos()
        //    if !(omega.IsNaN() || omega == 0.0) {
        //      denom := 1.0 / omega.Sin()
        //      slerp = v.Mult(((1.0 - t) * omega).Sin() * denom).
        //      Add(o.Mult(((t * omega) * denom).Sin()));
        //    } else {
        //      slerp = v
        //    }
        //  return
        //}

        /// <summary>
        /// Spherically interpolates a pair of vectors given a percentage <c>t</c>
        /// </summary>
        /// <param name="end">The End result once t is equal to 1</param>
        /// <param name="t">The percent such that 0 < t < 1 </param>
        /// <remarks>
        /// Adapted from https://gist.github.com/manveru/384873
        /// </remarks>
        public static Vector3 Slerp(this Vector3 source, Vector3 end, float t)
        {
            float Omega = (float)Math.Acos(Vector3.Dot(source, end));
            if (Omega != float.NaN && Omega != 0f)
            {
                float Denom = 1f / (float)Math.Sin(Omega);
                return source * (float)(Math.Sin((1 - t) * Omega) * Denom) +
                    (end * (float)Math.Sin((t * Omega) * Denom));
            }
            else
                return source;
        }

        public static Vector4 Slerp(this Vector4 source, Vector4 end, float t)
        {
            float Omega = (float)Math.Acos(Vector4.Dot(source, end));
            if (Omega != float.NaN && Omega != 0f)
            {
                float Denom = 1f / (float)Math.Sin(Omega);
                return source * (float)(Math.Sin((1 - t) * Omega) * Denom) +
                    (end * (float)Math.Sin((t * Omega) * Denom));
            }
            else
                return source;
        }
    }
}
