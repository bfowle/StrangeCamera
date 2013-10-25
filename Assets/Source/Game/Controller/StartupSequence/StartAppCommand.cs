using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {
	
	public class StartAppCommand : Command {
		
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView { get; set; }
		
		[Inject(ContextKeys.CONTEXT)]
		public IContext context { get; set; }
		
		public override void Execute() {
			// if we're not the first context, we need to shut down the AudioListener
			// this is one way to do it, but there is no "right" way
			if (context != Context.firstContext) {
				foreach (AudioListener listener in Camera.FindObjectsOfType(
					typeof(AudioListener)) as AudioListener[]) {
					listener.enabled = false;
				}
			}
			
			// MonoBehaviours can only be injected after they've been instantiated manually
			// here we create the main GameLoop, attaching it to the ContextView.
			
			// attach the GameLoop MonoBehaviour to the contextView
			contextView.AddComponent<GameLoop>();
			IGameTimer timer = contextView.GetComponent<GameLoop>();
			// then bind it for injection
			injectionBinder.Bind<IGameTimer>().ToValue(timer);
		}
		
	}
	
}
