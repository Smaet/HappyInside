﻿using System;


namespace VoxelPlay {

	public static class ArrayExtensions {
		public static void Fill<T> (this T[] destinationArray, params T[] value) {
			if (destinationArray == null) {
				throw new ArgumentNullException ("destinationArray");
			}

			if (value.Length >= destinationArray.Length) {
				throw new ArgumentException ("Length of value array must be less than length of destination");
			}

			// set the initial array value
			Array.Copy (value, destinationArray, value.Length);

			int arrayToFillHalfLength = destinationArray.Length / 2;
			int copyLength;

			for (copyLength = value.Length; copyLength < arrayToFillHalfLength; copyLength <<= 1) {
				Array.Copy (destinationArray, 0, destinationArray, copyLength, copyLength);
			}

			Array.Copy (destinationArray, 0, destinationArray, copyLength, destinationArray.Length - copyLength);
		}

		public static void Fill<T> (this T[] destinationArray, T value) {
			if (destinationArray == null) {
                return;
			}

			if (0 >= destinationArray.Length) {
                return;
			}

			// set the initial array value
			destinationArray [0] = value;

			int arrayToFillHalfLength = destinationArray.Length / 2;
			int copyLength;

			for (copyLength = 1; copyLength < arrayToFillHalfLength; copyLength <<= 1) {
				Array.Copy (destinationArray, 0, destinationArray, copyLength, copyLength);
			}

			Array.Copy (destinationArray, 0, destinationArray, copyLength, destinationArray.Length - copyLength);
		}

		public static T[] Extend<T>(this T[] array) {
			int newSize = array.Length * 2;
			T[] newArray = new T[newSize];
			System.Array.Copy (array, newArray, array.Length);
			return newArray;
		}

		public static void Remap(this float[] array, float min, float max)
        {
			float maxValue = float.MinValue;
			float minValue = float.MaxValue;
			for (int k=0;k<array.Length;k++) {
				float v = array [k];
				maxValue = v > maxValue ? v : maxValue;
				minValue = v < minValue ? v : minValue;
			}
			float range = maxValue - minValue;
			if (range == 0) {
				for (int k = 0; k < array.Length; k++) {
					array [k] = min;
				}
			} else {
				float outputRange = max - min;
				for (int k = 0; k < array.Length; k++) {
					float t = (array [k] - minValue) / range;
					array [k] = min + t * outputRange;
				}
			}
        }
	}
}

