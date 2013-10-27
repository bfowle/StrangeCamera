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
				// demo
				view.beginFlythrough();
				cinematicStart = true;
				
			} else if (state == CameraState.CHARACTER) {
				view.attachToCharacter();
				// demo
				characterAttach = true;
			}
		}
		
		private IEnumerator flyToWaypoints() {
			CameraWaypoint waypoint;
			int i = 0,
				len = model.waypoints.Count;
			
			for (; i < len; i++) {
				waypoint = model.waypoints[i];
				
				view.flyToWaypoint(waypoint);
				
				yield return new WaitForSeconds(waypoint.duration + waypoint.delay);
				
				// demo purposes
				currentWaypoint++;
			}
			
			flythroughCompleteSignal.Dispatch();
			
			// demo
			cinematicEnd = true;
			yield return new WaitForSeconds(2f);
			initialSequence = false;
			currentWaypoint = -1;
			
			yield return null;
		}
		
		//-----------------------------------
		//- DEMO DEBUG CONTROLS/INFORMATION -
		//-----------------------------------
		
		private bool initialSequence = true;
		private bool cinematicStart = false;
		private bool cinematicEnd = false;
		private bool characterAttach = false;
		private int currentWaypoint = 0;
		private float sliderDistance = 15f;
		private float sliderHeight = 8f;
		private float sliderSpeed = 2.5f;
		private bool lookAtTarget = false;
		
	    void OnGUI() {
			if (initialSequence) {
				GUI.Box(new Rect(10, 10, 260, (60 + (currentWaypoint * 20) +
					(cinematicStart ? 20 : 0) + (characterAttach ? 20 : 0))), "Demo Console");
				
				if (cinematicStart) {
					GUI.Label(new Rect(20, 40, 200, 40), "State: CINEMATIC started...");
				}
				
				int i = 0,
					y = 60;
				for (; i < model.waypoints.Count; i++) {
					if (currentWaypoint >= i) {
						GUI.Label(new Rect(20, y, 200, 40), " - waypoint #" + (i + 1) + " " + 
							(currentWaypoint > i ? "finished" : "in progress..."));
						y += 20;
					}
				}
				
				if (cinematicEnd) {
					GUI.Label(new Rect(20, y, 200, 40), "State: CINEMATIC finished...");
					y += 20;
				}
				
				if (characterAttach) {
					GUI.Label(new Rect(20, y, 200, 40), "State: CHARACTER attached...");
				}
			} else {
				GUI.Box(new Rect(10, 10, 260, 150), "Demo Controls");
		
				GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
				Texture2D btnInactive = btnStyle.normal.background;
				Texture2D btnActive = btnStyle.active.background;
				
				GUI.Label(new Rect(20, 40, 80, 20), "Mode:");
				
				btnStyle.normal.background = (model.state == CameraState.CINEMATIC ? btnActive : btnInactive);
				if (GUI.Button(new Rect(100, 40, 80, 20), "Cinematic", btnStyle)) {
					model.SetState(CameraState.CINEMATIC);
					view.stateChange(CameraState.CINEMATIC);
				}
				
				btnStyle.normal.background = (model.state == CameraState.CHARACTER ? btnActive : btnInactive);
				if (GUI.Button(new Rect(185, 40, 80, 20), "Character", btnStyle)) {
					model.SetState(CameraState.CHARACTER);
					view.stateChange(CameraState.CHARACTER);
				}
				
				if (model.state == CameraState.CINEMATIC) {
					GUI.Label(new Rect(20, 70, 80, 20), "Waypoints:");
					
					int i = 0,
						y = 70;
					foreach (CameraWaypoint waypoint in model.waypoints) {
						btnStyle.normal.background = (currentWaypoint == i ? btnActive : btnInactive);
						if (GUI.Button(new Rect(100, y, 120, 20), "Waypoint " + (i + 1), btnStyle)) {
							view.flyToWaypoint(waypoint);
							currentWaypoint = i;
						}
						i++;
						y += 25;
					}
				} else if (model.state == CameraState.CHARACTER) {
					GUIStyle sliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
					sliderStyle.normal.background = btnStyle.active.background;
					GUIStyle thumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
					
					float oldDistance = sliderDistance,
						oldHeight = sliderHeight,
						oldSpeed = sliderSpeed;
					bool oldLookAt = lookAtTarget;
					
					GUI.Label(new Rect(20, 70, 115, 20), "Camera Distance:");
					sliderDistance = GUI.HorizontalSlider(new Rect(135, 75, 120, 10),
						sliderDistance, 2f, 30f, sliderStyle, thumbStyle);
					if (sliderDistance != oldDistance) {
						view.setCameraDistance(sliderDistance);
					}
					
					GUI.Label(new Rect(20, 90, 115, 20), "Camera Height:");
					sliderHeight = GUI.HorizontalSlider(new Rect(135, 95, 120, 10),
						sliderHeight, 2f, 20f, sliderStyle, thumbStyle);
					if (sliderHeight != oldHeight) {
						view.setCameraHeight(sliderHeight);
					}
					
					GUI.Label(new Rect(20, 110, 115, 20), "Camera Speed:");
					sliderSpeed = GUI.HorizontalSlider(new Rect(135, 115, 120, 10),
						sliderSpeed, 0.1f, 5f, sliderStyle, thumbStyle);
					if (sliderSpeed != oldSpeed) {
						view.setCameraSpeed(sliderSpeed);
					}
					
					lookAtTarget = GUI.Toggle(new Rect(135, 135, 120, 20), lookAtTarget, " LookAt Target");
					if (lookAtTarget != oldLookAt) {
						view.setLookAtTarget(lookAtTarget);
					}
				}
			}
	    }
	
	}
	
}
