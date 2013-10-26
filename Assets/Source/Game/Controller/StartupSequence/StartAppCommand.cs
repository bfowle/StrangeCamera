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
			// attach the GameLoop MonoBehaviour to the contextView
			contextView.AddComponent<GameLoop>();
			IGameTimer timer = contextView.GetComponent<GameLoop>();
			// then bind it for injection
			injectionBinder.Bind<IGameTimer>().ToValue(timer);
		}
		
	}
	
}
