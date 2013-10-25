using System;
using UnityEngine;

namespace StrangeCamera.Game {
	
	public class CameraWaypoint {
		
		private Vector3 _to;
		private Vector3 _from;
		private float _duration = 5.0f;
		
		public Vector3 to {
			get { return _to; }
		}
		
		public Vector3 from {
			get { return _from; }
		}
		
		public float duration {
			get { return _duration; }
		}
		
		public CameraWaypoint(Vector3 toPt, Vector3 fromPt, float dur) {
			_to = toPt;
			_from = fromPt;
			_duration = dur;
		}
		
	}

}
