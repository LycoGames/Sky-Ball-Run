using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Base.State
{
    public abstract class StateMachine
    {
        protected abstract void OnEnter();
        protected abstract void OnExit();

        private StateMachine parent;
        private StateMachine defaultSubState;
        private StateMachine currentSubState;

        private readonly Dictionary<Type, StateMachine> subStates = new Dictionary<Type, StateMachine>();
        private readonly Dictionary<int, StateMachine> transitions = new Dictionary<int, StateMachine>();

        public void Enter()
        {
            OnEnter();
            if (currentSubState == null && defaultSubState != null)
                currentSubState = defaultSubState;

            currentSubState?.Enter();
            
            Debug.Log(
                "<color=cyan>" + GetType().Name + " " +
                System.Reflection.MethodBase.GetCurrentMethod().Name + "</color>");
        }

        protected void AddSubState(StateMachine subState)
        {
            if (subStates.Count == 0)
                defaultSubState = subState;

            subState.parent = this;

            if (subStates.ContainsKey(subState.GetType()))
            {
                Debug.LogWarning("Duplicated sub state : " + subState.GetType());
                return;
            }

            subStates.Add(subState.GetType(), subState);
        }

        protected void AddTransition(StateMachine sourceStateMachine, StateMachine targetStateMachine, int trigger)
        {
            if (sourceStateMachine.transitions.ContainsKey(trigger))
            {
                Debug.Log("Duplicated transition! : " + trigger);
                return;
            }

            sourceStateMachine.transitions.Add(trigger, targetStateMachine);
        }

        protected void SendTrigger(int trigger)
        {
            StateMachine root = this;
            while (root?.parent != null)
            {
                root = root.parent;
            }

            while (root != null)
            {
                if (root.transitions.TryGetValue(trigger, out StateMachine toState))
                {
                    root.parent?.ChangeSubState(toState);
                    return;
                }

                root = root.currentSubState;
            }
        }

        protected void SetCurrentSubStateToDefaultSubState()
        {
            if (defaultSubState != null)
                currentSubState = defaultSubState;
        }

        private void ChangeSubState(StateMachine state)
        {
            currentSubState?.Exit();

            StateMachine nextState = subStates[state.GetType()];
            currentSubState = nextState;
            nextState.Enter();
        }

        private void Exit()
        {
            currentSubState?.Exit();
            OnExit();

            Debug.Log(
                "<color=cyan>" + GetType().Name + " " +
                System.Reflection.MethodBase.GetCurrentMethod().Name + "</color>");
        }
    }
}