using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace StrangeCamera.Game {
	
	public class CameraView : View {
		
		private const float DISTANCE = 15f;
		private const float HEIGHT = 8f;
		private const float SPEED = 2.5f;

		public GameObject target;
		
		
		private Transform _transform;
		private CameraState _state;
		private CameraWaypoint _waypoint;
		
		internal void init() {
			_transform = transform;
		}
		
		void LateUpdate() {
			if (_state == CameraState.CHARACTER) {
				updateCharacterCamera();
			} else if (_state == CameraState.CINEMATIC) {
				updateCinematicCamera();
			}
		}
		
		internal void stateChange(CameraState state) {
			_state = state;
		}
		
		internal void flyToWaypoint(CameraWaypoint waypoint) {
			_transform.position = waypoint.from.position;
			_transform.localRotation = waypoint.from.rotation;
			
			_waypoint = waypoint;
		}
		
		internal void attachToCharacter() {
			// enable controls
			target.GetComponent<ThirdPersonController>().enabled = true;
		}
		
		private void updateCinematicCamera() {
			float t = _waypoint.duration / 10f * Time.deltaTime;
			
    		_transform.position = Vector3.Lerp(_transform.position, _waypoint.to.position, t);
    		_transform.localRotation = Quaternion.Slerp(_transform.localRotation, _waypoint.to.rotation, t);
		}
		
		private void updateCharacterCamera() {
			float t = SPEED * Time.deltaTime;
			
	        _transform.position = Vector3.Lerp(_transform.position, target.transform.position + 
				new Vector3(DISTANCE, HEIGHT, -DISTANCE), t);
			_transform.rotation = Quaternion.Slerp(_transform.rotation, 
				Quaternion.Euler(new Vector3(30f, -45f, 0)), t);
		}
		
	}

}
