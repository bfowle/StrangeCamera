using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace StrangeCamera.Game {
	
	public class CameraMediator : Mediator {
		
		[Inject]
		public ICamera model { get; set; }
		
		[Inject]
		public CameraView view { get; set; }
		
		[Inject]
		public CameraStateSignal cameraStateSignal { get; set; }
		[Inject]
		public FlythroughCompleteSignal flythroughCompleteSignal { get; set; }
		
		public override void OnRegister() {
			AddListeners();
			
			// initialize the View
			view.init();
		}
		
		public override void OnRemove() {
			RemoveListeners();
		}
		
		private void AddListeners() {
			cameraStateSignal.AddListener(onCameraStateChanged);
		}
		
		private void RemoveListeners() {
			cameraStateSignal.RemoveListener(onCameraStateChanged);
		}
		
		private void onCameraStateChanged(CameraState state) {
			model.SetState(state);
			
			view.stateChange(state);
			if (state == CameraState.CINEMATIC) {
				StartCoroutine(flyToWaypoints());
			} else if (state == CameraState.CHARACTER) {
				view.attachToCharacter();
			}
		}
		
		private IEnumerator flyToWaypoints() {
			CameraWaypoint waypoint;
			int i = 0,
				len = model.waypoints.Count;
			
			for (; i < len; i++) {
				waypoint = model.waypoints[i];
				
				view.flyToWaypoint(waypoint);
				
				if (i < len-1) {
					yield return StartCoroutine(waypointDelay(waypoint.duration));
				} else {
					yield return new WaitForSeconds(waypoint.duration);
				}
			}
			
			flythroughCompleteSignal.Dispatch();
			
			yield return null;
		}
		
		private IEnumerator waypointDelay(float duration) {
			float delayOffset = 0.25f,
				speed = 1f;
			
			yield return new WaitForSeconds(duration - delayOffset);
			yield return new WaitForSeconds(speed/* + delayOffset*/);
		}
		
	}
	
}
