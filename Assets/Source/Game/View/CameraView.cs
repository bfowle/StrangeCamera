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
		
		// demo controls
		private float cameraDistance = DISTANCE;
		private float cameraHeight = HEIGHT;
		private float cameraSpeed = SPEED;
		private bool cameraLookAt = false;
		
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
		
		internal void beginFlythrough() {
			// demo - disable controls
			target.GetComponent<ThirdPersonController>().isControllable = false;
		}
		
		internal void attachToCharacter() {
			// demo - enable controls
			target.GetComponent<ThirdPersonController>().isControllable = true;
		}
		
		private void updateCinematicCamera() {
			float t = _waypoint.duration / 10f * Time.deltaTime;
			
    		_transform.position = Vector3.Lerp(_transform.position, _waypoint.to.position, t);
    		_transform.localRotation = Quaternion.Slerp(_transform.localRotation, _waypoint.to.rotation, t);
		}
		
		private void updateCharacterCamera() {
			float t = cameraSpeed * Time.deltaTime;
			
	        _transform.position = Vector3.Lerp(_transform.position, target.transform.position + 
				new Vector3(cameraDistance, cameraHeight, -cameraDistance), t);
			
			if (cameraLookAt) {
				_transform.rotation = Quaternion.Slerp (_transform.rotation, 
					Quaternion.LookRotation(target.transform.position - _transform.position), t);
			} else {
				_transform.rotation = Quaternion.Slerp(_transform.rotation, 
					Quaternion.Euler(new Vector3(30f, -45f, 0)), t);
			}
		}
		
		//-----------------------------------
		//- DEMO CONTROLS                   -
		//-----------------------------------
		
		internal void setCameraDistance(float distance) {
			cameraDistance = distance;
		}
		
		internal void setCameraHeight(float height) {
			cameraHeight = height;
		}
		
		internal void setCameraSpeed(float speed) {
			cameraSpeed = speed;
		}
		
		internal void setLookAtTarget(bool lookAt) {
			cameraLookAt = lookAt;
		}
		
	}

}
