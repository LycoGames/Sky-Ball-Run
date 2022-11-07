using _Game.Scripts.Base.Component;
using _Game.Scripts.Base.State;
using _Game.Scripts.Game.Enums;

namespace _Game.Scripts.Game.States.Main
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