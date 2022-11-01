using System.Collections;
using System.Collections.Generic;
using Base.Component;
using Base.State;
using UnityEngine;

namespace Game.States.Splash
{
    public class SplashState : StateMachine
    {
        private readonly LoadingGameState loadingGameState;

        public SplashState(ComponentContainer componentContainer)
        {
            loadingGameState = new LoadingGameState(componentContainer);
            AddSubState(loadingGameState);
        }

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}