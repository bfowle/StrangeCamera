using System;
using System.Collections;
using UnityEngine;

namespace StrangeCamera.Game {
	
	public class GameLoop : MonoBehaviour, IGameTimer {
		
		private bool sendUpdates = false;
		
		[Inject]
		public GameUpdateSignal gameUpdateSignal { get; set; }
		
		public GameLoop() {
		}
		
		public void Start() {
			sendUpdates = true;
		}
		
		public void Stop() {
			sendUpdates = false;
		}
		
		void Update() {
			if (sendUpdates) {
				gameUpdateSignal.Dispatch();
			}
		}
		
	}
	
}
