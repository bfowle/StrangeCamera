using System;
using System.Collections.Generic;

namespace StrangeCamera.Game {

	public class CameraModel : ICamera {

        private CameraState _state;
		private bool _locked;
		private List<CameraWaypoint> _waypoints;

        public CameraState state {
            get { return _state; }
        }
		
		public bool locked {
			get { return _locked; }
		}
		
		public List<CameraWaypoint> waypoints {
			get { return _waypoints; }
		}

        public CameraModel() {
			_waypoints = new List<CameraWaypoint>();
        }

        public void SetState(CameraState value) {
            _state = value;
        }
		
		public void SetLocked(bool value) {
			_locked = value;
		}
		
		public void AddWaypoint(CameraWaypoint value) {
			_waypoints.Add(value);
		}
		
		public void ClearWaypoints() {
			waypoints.Clear();
		}

	}

}

