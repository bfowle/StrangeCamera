using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace StrangeCamera.Game {
	
	public class GameRoot : ContextView {
	
		void Awake() {
			// Instantiate the context, passing it this instance and a 'true' for autoStartup.
			context = new GameContext(this, true);
			context.Start();
		}
		
	}
	
}
