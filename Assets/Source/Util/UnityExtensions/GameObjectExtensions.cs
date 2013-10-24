using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrangeCamera.Util {

	public static class GameObjectExtensions {
		
		public static T[] GetComponentsInChildrenWithTag<T>(this GameObject self, string tag) where T: Component {
    		List<T> results = new List<T>();

    		if (self.CompareTag(tag)) {
        		results.Add(self.GetComponent<T>());
			}

			foreach (Transform t in self.transform) {
    			results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));
			}

    		return results.ToArray();
		}
		
		public static T GetComponentInParents<T>(this GameObject self) where T : Component {
			Transform t = self.transform;
    		for (; t != null; t = t.parent) {
        		T result = t.GetComponent<T>();
        		if (result != null) {
            		return result;
				}
    		}

    		return null;
		}
		
		public static T[] GetComponentsInParents<T>(this GameObject self) where T: Component {
    		List<T> results = new List<T>();
			Transform t = self.transform;
    		for (; t != null; t = t.parent) {
				T result = t.GetComponent<T>();
        		if (result != null) {
            		results.Add(result);
				}
    		}

    		return results.ToArray();
		}
		
		public static bool LayerMaskEquals(this GameObject self, LayerMask mask) {
			return (bool)((mask.value & (1 << self.layer)) > 0);
		}
		
		public static void SetLayerRecursively(this GameObject self, int layer) {
        	self.layer = layer;
        	foreach (Transform t in self.transform) {
            	t.gameObject.layer = layer;
			}
    	}
		
		public static int GetCollisionMask(this GameObject self, int layer=-1) {
    		if (layer == -1) {
        		layer = self.layer;
			}

    		int mask = 0;
			int i = 0;
    		for (; i < 32; i++) {
        		mask |= (Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1) << i;
			}

    		return mask;
		}
		
		public static void SetVisualRecursively(this GameObject self, bool value) {
    		foreach (Renderer renderer in self.GetComponentsInChildren<Renderer>()) {
        		renderer.enabled = value;
			}
		}
		
		public static void SetCollisionRecursively(this GameObject self, bool value) {
    		foreach (Collider collider in self.GetComponentsInChildren<Collider>()) {
        		collider.enabled = value;
			}
		}

	}
	
}
