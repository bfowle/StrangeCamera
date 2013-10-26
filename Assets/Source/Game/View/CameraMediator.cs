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
				// demo purposes
				currentWaypoint = i;
				
				view.flyToWaypoint(waypoint);
				
				yield return new WaitForSeconds(waypoint.duration + waypoint.delay);
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
		private int currentWaypoint = -1;
		
	    void OnGUI() {
			if (initialSequence) {
				GUI.Box(new Rect(10, 10, 260, (60 + (currentWaypoint * 20) +
					(cinematicStart ? 20 : 0) + (cinematicEnd ? 20 : 0) +
					(characterAttach ? 20 : 0))), "Demo Console");
				
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
				GUI.Box(new Rect(10, 10, 260, 140), "Demo Controls");
		
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
				}
			}
	    }
	
	}
	
}
