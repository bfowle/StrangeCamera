using System;
using System.Collections.Generic;

namespace StrangeCamera.Game {

    public interface ICamera {

        CameraState state { get; }
        List<CameraWaypoint> waypoints { get; }

        void SetState(CameraState value);
        void AddWaypoint(CameraWaypoint value);
        void ClearWaypoints();

    }

}

