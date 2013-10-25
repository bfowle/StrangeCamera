using System;
using System.Collections.Generic;

namespace StrangeCamera.Game {

	public interface ICamera {

		CameraState state { get; }
		bool locked { get; }
		List<CameraWaypoint> waypoints { get; }

        void SetState(CameraState value);
		void SetLocked(bool value);
		void AddWaypoint(CameraWaypoint value);
		void ClearWaypoints();

	}

}

