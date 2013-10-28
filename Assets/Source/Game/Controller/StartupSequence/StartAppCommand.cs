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
            contextView.AddComponent<GameLoop>();

            IGameTimer timer = contextView.GetComponent<GameLoop>();
            injectionBinder.Bind<IGameTimer>().ToValue(timer);
        }

    }

}
