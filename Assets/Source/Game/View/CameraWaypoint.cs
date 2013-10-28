using System;
using UnityEngine;

namespace StrangeCamera.Game {

    public class WaypointStruct {
        public Vector3 position;
        public Quaternion rotation;

        public WaypointStruct(Vector3 pos, Quaternion rot) {
            position = pos;
            rotation = rot;
        }
    }

    public class CameraWaypoint {

        private WaypointStruct _from;
        private WaypointStruct _to;
        private float _duration = 5.0f;
        private float _delay = 0;

        public WaypointStruct to {
            get { return _to; }
        }

        public WaypointStruct from {
            get { return _from; }
        }

        public float duration {
            get { return _duration; }
        }

        public float delay {
            get { return _delay; }
        }

        public CameraWaypoint(Vector3 fromPosition, Vector3 toPosition, Vector3 fromRotation,
            Vector3 toRotation, float dur, float del) {

            _from = new WaypointStruct(fromPosition, Quaternion.Euler(fromRotation));
            _to = new WaypointStruct(toPosition, Quaternion.Euler(toRotation));
            _duration = dur;
            _delay = del;
        }

    }

}
