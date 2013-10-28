using System;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

namespace StrangeCamera.Main {

    public class StartCommand : EventCommand {

        public override void Execute() {
            dispatcher.Dispatch(MainEvent.LOAD_SCENE, "game");
        }

    }

}

