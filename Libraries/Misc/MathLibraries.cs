﻿using System;


namespace ModLibsGeneral.Libraries.Misc {
	/// <summary>
	/// Assorted static library functions pertaining to math.
	/// </summary>
	public class MathLibraries {
		/// <summary>
		/// Finds the value of a curved interpolation between 2 values.
		/// </summary>
		/// <param name="y1"></param>
		/// <param name="y2"></param>
		/// <param name="mu"></param>
		/// <returns></returns>
		public static double CosineInterpolate( double y1, double y2, double mu ) {
			double mu2 = 1d - Math.Cos( mu * Math.PI );
			mu2 *= 0.5d;
			return (y1 * (1d - mu2)) + (y2 * mu2);
		}
	}
}
