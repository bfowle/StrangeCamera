using System;
using System.Collections;
using StrangeCamera.Game;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;
using strange.extensions.command.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.sequencer.api;
using strange.extensions.sequencer.impl;

namespace StrangeCamera.UnitTests {

	public class StubContext : CrossContext {
	
	    public ICommandBinder commandBinder;
	    public IMediationBinder mediationBinder;
	
	    public StubContext() : base() {}
	    public StubContext(object view, bool autoStartup) : base(view, autoStartup) {}
	
	    protected override void addCoreComponents() {
	        base.addCoreComponents();
	
	        injectionBinder.Bind<IInjectionBinder>().ToValue(injectionBinder);
	        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
	        injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>().ToSingleton()
	            .ToName(ContextKeys.CONTEXT_DISPATCHER);
	        injectionBinder.Bind<ISequencer>().To<EventSequencer>().ToSingleton();
	        commandBinder = injectionBinder.GetInstance<ICommandBinder>() as ICommandBinder;
	    }
	
	    public override void AddView(object view) {
	        mediationBinder.Trigger(MediationEvent.AWAKE, view as IView);
	    }
	
	}

    public class MockView : IView {
        public bool requiresContext { get; set; }
        public bool registeredWithContext { get; set; }
    }
    
}
