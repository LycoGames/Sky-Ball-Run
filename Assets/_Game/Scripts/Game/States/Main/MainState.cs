using Base.Component;
using Base.State;
using Game.Enums;
using Game.States.Splash;
using UnityEngine;

namespace Game.States.Main
{
    public class MainState : StateMachine
    {
        private readonly PrepareGameState prepareGameState;
        private readonly SettingsState settingsState;

        public MainState(ComponentContainer componentContainer)
        {
            prepareGameState = new PrepareGameState(componentContainer);
            settingsState = new SettingsState(componentContainer);

            AddSubState(prepareGameState);
            AddSubState(settingsState);

            AddTransition(prepareGameState, settingsState, (int) StateTrigger.GoToSettings);
            AddTransition(settingsState, prepareGameState, (int) StateTrigger.ReturnToPreparingGame);
        }


        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }
    }
}