using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Base.Component;
using Game.States;
using UnityEngine;

namespace Game.Components
{
    public class MainComponent : MonoBehaviour
    {
        private ComponentContainer componentContainer;

        private DataComponent dataComponent;
        private UIComponent uiComponent;
        private LoadingGameComponent loadingGameComponent;
        private PrepareGameComponent prepareGameComponent;
        private InGameComponent inGameComponent;
        private EndGameComponent endGameComponent;

        private AppState appState;

        private void Awake()
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            CreateComponentContainer();
        }


        // Start is called before the first frame update
        void Start()
        {
            CreateDataComponent();
            CreateUIComponent();
            CreateLoadingGameComponent();
            CreatePrepareGameComponent();
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
            dataComponent.Initialize(componentContainer);
            uiComponent.Initialize(componentContainer);
            loadingGameComponent.Initialize(componentContainer);
            prepareGameComponent.Initialize(componentContainer);
            inGameComponent.Initialize(componentContainer);
            endGameComponent.Initialize(componentContainer);
        }

        private void CreateDataComponent()
        {
            dataComponent = FindObjectOfType<DataComponent>();
            string componentKey = dataComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, dataComponent);
        }

        private void CreateUIComponent()
        {
            uiComponent = FindObjectOfType<UIComponent>();
            string componentKey = uiComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, uiComponent);
        }

        private void CreateLoadingGameComponent()
        {
            loadingGameComponent = FindObjectOfType<LoadingGameComponent>();
            string componentKey = loadingGameComponent.GetType().Name;

            componentContainer.AddComponent(componentKey, loadingGameComponent);
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