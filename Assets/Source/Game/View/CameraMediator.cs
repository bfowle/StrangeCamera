using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace StrangeCamera.Game {
	
	public class CameraMediator : Mediator {
		
		[Inject]
		public ICamera model { get; set; }
		
		[Inject]
		public CameraView view { get; set; }
		
		[Inject]
		public GameOverSignal gameOverSignal { get; set; }
		[Inject]
		public ReplaySignal replaySignal { get; set; }
		[Inject]
		public RestartGameSignal restartGameSignal { get; set; }
		[Inject]
		public GameUpdateSignal gameUpdateSignal { get; set; }
		
		public override void OnRegister() {
			AddListeners();
			
			// initialize the View
			view.init();
		}
		
		public override void OnRemove() {
			RemoveListeners();
		}
		
		private void AddListeners() {
			gameOverSignal.AddListener(onGameOver);
			replaySignal.AddListener(onReplay);
			restartGameSignal.AddListener(onRestartGame);
			gameUpdateSignal.AddListener(onGameUpdate);
		}
		
		private void RemoveListeners() {
			gameOverSignal.RemoveListener(onGameOver);
			replaySignal.RemoveListener(onReplay);
			restartGameSignal.RemoveListener(onRestartGame);
			gameUpdateSignal.RemoveListener(onGameUpdate);
		}
		
		private void onGameOver() {
			RemoveListeners();
			
			view.gameOver();
		}
		
		private void onReplay() {
		}
		
		private void onRestartGame() {
			OnRegister();
		}
		
		private void onGameUpdate() {
		}
		
	}
	
}
