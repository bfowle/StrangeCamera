using System;
using System.Collections;
using UnityEngine;

namespace StrangeCamera.Game {
	
	public class GameLoop : MonoBehaviour, IGameTimer {
		
		private bool sendUpdates = false;
		
		[Inject]
		public CameraSequenceSignal cameraSequenceSignal { get; set; }
		
		public GameLoop() {
		}
		
		public void Start() {
			sendUpdates = true;

			cameraSequenceSignal.Dispatch();
		}
		
		public void Stop() {
			sendUpdates = false;
		}
		
		void Update() {
			if (sendUpdates) {
				//gameUpdateSignal.Dispatch();
			}
		}
		
	}
	
}
