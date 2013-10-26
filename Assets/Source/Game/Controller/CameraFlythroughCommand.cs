using System;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {
	
	public class CameraFlythroughCommand : Command {
		
		[Inject]
		public ICamera model { get; set; }
		
		[Inject]
		public CameraStateSignal cameraStateSignal { get; set; }
		[Inject]
		public FlythroughCompleteSignal flythroughCompleteSignal { get; set; }
		
		public override void Execute() {
			// stop sequencer until release is called
			Retain();
			
			flythroughCompleteSignal.AddListener(onFlythroughComplete);
			
            startFlythrough();
		}

        private void startFlythrough() {
			// waypoint:
			// - from position, to position
			// - from rotation, to rotation
			// - duration, delay
			
			model.AddWaypoint(new CameraWaypoint(
				new Vector3(-133.3f, 20.1f, -21.1f),
				new Vector3(-107.8f, 32f, 31.2f),
				new Vector3(34.4f, 278.5f, 0),
				new Vector3(8.8f, 0.9f, 0),
				5f, 0.5f
			));
			
		    model.AddWaypoint(new CameraWaypoint(
				new Vector3(-141.4f, 42.4f, 222f),
				new Vector3(-63.1f, 50.1f, 188f),
				new Vector3(38.2f, 138.8f, 0),
				new Vector3(37.3f, 152.7f, 0),
				4f, 0.5f
			));
			
			model.AddWaypoint(new CameraWaypoint(
				new Vector3(-86.5f, 27.3f, 31.6f),
				new Vector3(6f, 20.6f, -24.4f),
				new Vector3(24.1f, 104.6f, 0),
				new Vector3(57.8f, 7.5f, 0),
				5f, 0.1f
			));
			
			cameraStateSignal.Dispatch(CameraState.CINEMATIC);
        }
		
		private void onFlythroughComplete() {
			flythroughCompleteSignal.RemoveListener(onFlythroughComplete);
			
			// continue sequencer
			Release();
		}
		
	}
	
}
