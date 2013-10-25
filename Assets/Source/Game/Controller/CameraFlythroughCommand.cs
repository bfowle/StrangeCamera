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
			model.AddWaypoint(new CameraWaypoint(new Vector3(127.35866f, 45.87816f, 200.3675f),
				new Vector3(45.8233f, 8.03159f, 89.49033f), 5f));
			model.AddWaypoint(new CameraWaypoint(new Vector3(1.655273f, 8.896175f, 1.212587f),
				new Vector3(0.5085396f, 2.39273f, -21.69198f), 4f));
			model.AddWaypoint(new CameraWaypoint(new Vector3(0.9397072f, 4.838002f, -16.72858f),
				new Vector3(10.917943f, 10.380289f, -42.28947f), 3f));
			
			cameraStateSignal.Dispatch(CameraState.CINEMATIC);
        }
		
		private void onFlythroughComplete() {
			flythroughCompleteSignal.RemoveListener(onFlythroughComplete);
			
			// continue sequencer
			Release();
		}
		
	}
	
}
