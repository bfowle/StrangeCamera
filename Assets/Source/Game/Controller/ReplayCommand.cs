using System;
using UnityEngine;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {
        
	public class ReplayCommand : Command {
	
		public override void Execute() {
			Application.LoadLevel("_Main");
		}
	
	}
        
}