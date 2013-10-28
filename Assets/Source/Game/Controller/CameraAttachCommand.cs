using System;
using UnityEngine;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {
	
	public class CameraAttachCommand : Command {

		[Inject]
		public CameraStateSignal cameraStateSignal { get; set; }
		//[Inject]
		//public CharacterControlSignal characterControlSignal { get; set; }

		public override void Execute() {
			cameraStateSignal.Dispatch(CameraState.CHARACTER);
			//characterControlSignal.Dispatch(true);
		}
		
	}
	
}
