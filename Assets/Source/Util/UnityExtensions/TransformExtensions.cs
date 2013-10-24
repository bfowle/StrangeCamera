using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrangeCamera.Util {

	public static class TransformExtensions {
		
		public static IEnumerable<Transform> Hierarchy(this Transform self, bool parent=true) {
			if (parent) {
				yield return self;
			}
			
			foreach (Transform child in self.Children()) {
				foreach (Transform leaf in child.Children()) {
					yield return leaf;
				}
			}
		}
		
		public static IEnumerable<Transform> Children(this Transform self) {
			int i = 0;
			for (; i < self.childCount; i++) {
				yield return self.GetChild(i);
			}
		}
	
	}
	
}
