using System;
using UnityEngine;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {
	
	public class ReplayCommand : Command {
		
		[Inject]
		public IGameTimer gameTimer { get; set; }

        [Inject]
        public RestartGameSignal restartGameSignal { get; set; }

		public override void Execute() {
            restartGameSignal.Dispatch();

			gameTimer.Start();
		}
		
	}
	
}
