using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.States;
using UnityEngine;

namespace _Game.Scripts.Game
{
    public class MainComponent : MonoBehaviour
    {
        private ComponentContainer componentContainer;
        
        private UIComponent uiComponent;
        private PrepareGameComponent prepareGameComponent;
        private IntroComponent introComponent;
        private StartGameComponent startGameComponent;
        private InGameComponent inGameComponent;
        private EndGameComponent endGameComponent;

        private AppState appState;

        void Awake()
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");

            CreateComponentContainer();
        }

        void Start()
        {
            CreateUIComponent();
            CreatePrepareGameComponent();
            CreateIntroComponent();
            CreateStartGameComponent();
            CreateInGameComponent();
            CreateEndGameComponent();

            InitializeComponents();
            CreateAppState();
            EnterAppState();
        }
        

        private void CreateComponentContainer()
        {
            componentContainer = new ComponentContainer();
        }

        private void InitializeComponents()
        {
            uiComponent.Initialize(componentContainer);
            introComponent.Initialize(componentContainer);
            prepareGameComponent.Initialize(componentContainer);
            startGameComponent.Initialize(componentContainer);
            inGameComponent.Initialize(componentContainer);
            endGameComponent.Initialize(componentContainer);
        }
        private void CreateStartGameComponent()
        {
            startGameComponent = FindObjectOfType<StartGameComponent>();
            string componentKey = startGameComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, startGameComponent);
        }
        

        private void CreateUIComponent()
        {
            uiComponent = FindObjectOfType<UIComponent>();
            string componentKey = uiComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, uiComponent);
        }

        private void CreateIntroComponent()
        {
            introComponent = FindObjectOfType<IntroComponent>();
            string componentKey = introComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, introComponent);
        }
        
        private void CreatePrepareGameComponent()
        { 
            prepareGameComponent = FindObjectOfType<PrepareGameComponent>();
            string componentKey = prepareGameComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, prepareGameComponent);
        }

        private void CreateInGameComponent()
        {
            inGameComponent = FindObjectOfType<InGameComponent>();
            string componentKey = inGameComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, inGameComponent);
        }

        private void CreateEndGameComponent()
        {
            endGameComponent = FindObjectOfType<EndGameComponent>();
            string componentKey = endGameComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, endGameComponent);
        }

        private void CreateAppState()
        {
            appState = new AppState(componentContainer);
        }

        private void EnterAppState()
        {
            appState.Enter();
        }
    }
}

