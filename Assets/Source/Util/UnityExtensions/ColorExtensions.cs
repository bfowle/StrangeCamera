using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrangeCamera.Util {

	public static class ColorExtensions {
		
		public static Color WithAlpha(this Color color, float alpha) {
    		return new Color(color.r, color.g, color.b, alpha);
		}
	
	}
	
}
