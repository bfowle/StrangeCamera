using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {
	
	public class GameContext : MVCSContext {
		
		public GameContext() : base() {
		}
		
		public GameContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup) {
		}
		
		protected override void addCoreComponents() {
			base.addCoreComponents();
			
			injectionBinder.Unbind<ICommandBinder>();
			injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
		}
		
		// override Start so that we can fire the StartSignal 
		override public IContext Start() {
			base.Start();
			
			StartSignal startSignal = injectionBinder.GetInstance<StartSignal>() as StartSignal;
			startSignal.Dispatch();
			
			return this;
		}
		
		protected override void mapBindings() {
			injectionBinder.Bind<ICamera>().To<CameraModel>().ToSingleton();

			mediationBinder.Bind<CameraView>().To<CameraMediator>();
			
			commandBinder.Bind<StartSignal>().To<StartAppCommand>()
				.To<StartGameCommand>().Once().InSequence();
		}
		
	}
	
}
