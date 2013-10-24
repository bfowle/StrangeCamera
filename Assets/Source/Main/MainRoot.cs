using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace StrangeCamera.Main {
	
	public class MainRoot : ContextView {
	
		void Awake() {
			// instantiate the context, passing it this instance and autoStartup true
			context = new MainContext(this, true);
			context.Start();
		}
		
	}
	
}
