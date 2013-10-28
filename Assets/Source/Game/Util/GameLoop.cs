using System;
using System.Collections;
using UnityEngine;

namespace StrangeCamera.Game {
	
	public class GameLoop : MonoBehaviour, IGameTimer {
		
		[Inject]
		public CameraSequenceSignal cameraSequenceSignal { get; set; }
		
		public GameLoop() {
		}
		
		public void Start() {
			cameraSequenceSignal.Dispatch();
		}
		
		public void Stop() {
		}
		
	}
	
}
