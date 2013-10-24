using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;

namespace StrangeCamera.Main {
	
	public class MainContext : MVCSContext {
		
		public MainContext() : base() {
		}
		
		public MainContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup) {
		}
		
		protected override void mapBindings() {
			commandBinder.Bind(ContextEvent.START).To<StartCommand>().Once();
			
			commandBinder.Bind(MainEvent.LOAD_SCENE).To<LoadSceneCommand>();
			commandBinder.Bind(MainEvent.GAME_COMPLETE).To<GameCompleteCommand>();
		}
	}
	
}
