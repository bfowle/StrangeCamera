using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace StrangeCamera.Game {
	
	public class CameraView : View {
		
		private const float DISTANCE = 15f;
		private const float DELAY = 2.5f;
		private const float ROTATION_AMOUNT = 90f;

		private Vector3 axisPosition = Vector3.zero;
		private Quaternion axisRotation = Quaternion.identity;
		private Vector3 initialVector;
		
		private float relativeRotation = 0;
		private float relativeDistanceX = -DISTANCE;
		private float relativeDistanceZ = -DISTANCE;
		
		private Transform _transform;
		private GameObject _target;
		private CameraState _state;
		
		internal void init() {
			_transform = transform;
			_target = GameObject.Find("Cube");
			
			axisPosition = _target.transform.position + 
				new Vector3(relativeDistanceX, DISTANCE / 2f, relativeDistanceZ);
			axisRotation = Quaternion.Euler(new Vector3(30f, 45f, 0));
			
			initialVector = _transform.position - _target.transform.position;
			initialVector.y = 0;
		}
		
		void LateUpdate() {
			if (_state == CameraState.CHARACTER) {
				updateCharacterCamera();
			} else if (_state == CameraState.CINEMATIC) {
				updateCinematicCamera();
			}
		}
		
	    void OnGUI() {
			GUI.Label(new Rect(120, 5, 200, 50), "==== CAMERA ====");
			GUI.Label(new Rect(120, 25, 200, 50), "State: " + _state.ToString());
	    	GUI.Label(new Rect(120, 45, 200, 50), "Position: " + _transform.position.ToString());
	    	GUI.Label(new Rect(120, 65, 250, 50), "Rotation: " + relativeRotation.ToString());
	    }
		
		internal void stateChange(CameraState state) {
			_state = state;
		}
		
		internal void flyToWaypoint(CameraWaypoint waypoint) {
			// set camera position to the waypoint `to` vector
			_transform.position = waypoint.to;
			
			/*
			HOTween.To(_transform, waypoint.duration, 
				// move camera position to the waypoint `from` vector
				new TweenParms().Prop("position", waypoint.from)
					.Ease(waypoint.ease)
			);
			*/
		}
		
		internal void attachToCharacter() {
		}
		
		private void updateCinematicCamera() {
		//	_transform.LookAt(_target.transform);
		}
		
		private void updateCharacterCamera() {
			// \todo check for isDirty and only update if necessary
			
			// rotate faster later, smoother, lerp with tween?
			float t = DELAY * Time.deltaTime;
			
			axisPosition = _target.transform.position + new Vector3(relativeDistanceX,
				DISTANCE / 2f, relativeDistanceZ);
			
	        _transform.position = Vector3.Lerp(_transform.position, axisPosition, t);
			_transform.rotation = Quaternion.Slerp(_transform.rotation, axisRotation, t);
		}
		
	}

}
