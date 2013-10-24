using System;
using UnityEngine;
using StrangeCamera.Main;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace StrangeCamera.Game {
	
	public class GameOverCommand : Command {
		
		[Inject]
		public IGameTimer gameTimer { get; set; }
		
		[Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
		public IEventDispatcher crossContextDispatcher { get; set; }
		
		public override void Execute() {
			gameTimer.Stop();
			
			//dispatch between contexts
			Debug.Log("GAME OVER...dispatch across contexts");
			
			crossContextDispatcher.Dispatch(MainEvent.GAME_COMPLETE);
		}
		
	}
	
}
