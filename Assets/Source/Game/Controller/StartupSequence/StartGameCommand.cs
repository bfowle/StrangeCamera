using System;
using UnityEngine;
using strange.extensions.command.impl;

namespace StrangeCamera.Game {

    public class StartGameCommand : Command {

        [Inject]
        public IGameTimer timer { get; set; }

        public override void Execute() {
        }

    }

}
