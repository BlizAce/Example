using System;
using UnityEngine;

namespace LootBunnies.Systems
{
    public class StateControlledObject : MonoBehaviour, IStateMachine, IStateManagement, IStateBehaviorModificationAddition, IStateBehaviorModificationRemoval, IStateBehaviorModificationSet, ITransitionSetup
    {
        /// <summary>
        /// TransitionStateMachine that is ran in the Update loop
        /// This will need to be allocated in the Awake function
        /// </summary>
        private TransitionStateMachine stateMachine;

        // Add a safe way to check for a null stateMachine when changing states
        // If its null, queue the last change to be used when the stateMachine
        // is created
        private int lastStateChange = -1;

        /// <summary>
        /// Returns -1 if no state change is queued
        /// </summary>
        public int QueuedStateChange
        {
            get { return lastStateChange; }
        }

        /// <summary>
        /// Use this to change the current state of the state machine
        /// This is a safe way to change states if you call it before the stateMachine is created 
        /// </summary>
        /// <param name="stateID"> ID of the next state </param>
        public void ChangeState(int stateID)
        {
            if (stateMachine == null)
            {
                lastStateChange = stateID;
                return;
            }
            stateMachine.ChangeState(stateID);
        }

        /// <summary>
        /// DO NOT USE THIS FUNCTION, Use Init instead
        /// </summary>
        public void Awake()
        {
            Init();
        }

        /// <summary>
        /// DO NOT USE THIS FUNCTION, Use Tick instead
        /// </summary>
        public void Update()
        {
            Tick();
            if (stateMachine != null)
            {
                if (lastStateChange != -1)
                {
                    stateMachine.ChangeState(lastStateChange);
                    lastStateChange = -1;
                }
                stateMachine.Tick();
            }
        }

        /// <summary>
        /// DO NOT USE THIS FUNCTION, Use FixedTick instead
        /// </summary>
        public void FixedUpdate()
        {
            FixedTick();
            if (stateMachine != null)
            {
                if (lastStateChange != -1)
                {
                    stateMachine.ChangeState(lastStateChange);
                    lastStateChange = -1;
                }
                stateMachine.FixedTick();
            }
        }

        /// <summary>
        /// Use this as the update function
        /// </summary>
        public virtual void Tick() { }

        /// <summary>
        /// Called on Awake
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// Use this as the fixed update function
        /// </summary>
        public virtual void FixedTick() { }

        /// <summary>
        /// Use this to clean up the object for deletion
        /// </summary>
        protected virtual void Cleanup()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Use this to clean up the object for deletion after a delay
        /// </summary>
        protected virtual void Cleanup(float delay)
        {
            Destroy(gameObject, delay);
        }

        #region State Machine Support Functions

        protected void InitStateMachine()
        {
            stateMachine = new TransitionStateMachine();
        }

        /// <summary>
        /// Adds an empty state to the state machine
        /// </summary>
        /// <param name="stateID"> Use enums and cast to int </param>
        public void AddState(int stateID)
        {
            stateMachine.AddState(new State(stateID));
        }

        public void AddState(State state)
        {
            stateMachine.AddState(state);
        }

        /// <summary>
        /// Adds to an existing state tick
        /// </summary>
        /// <param name="stateID"> ID of the state to add to </param>
        /// <param name="tick"> delegate to add - called in update when in state </param>
        public void AddToStateCallback(int stateID, Action tick, StateBehaviorType stateBehaviorType)
        {
            stateMachine.AddToStateCallback(stateID, tick, stateBehaviorType);
        }

        /// <summary>
        /// Remove a state - WARNING all callbacks for state will be lost
        /// </summary>
        /// <param name="stateID"> ID of the state to be removed </param>
        public void RemoveState(int stateID)
        {
            stateMachine.RemoveState(stateID);
        }

        public void RemoveFromStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType)
        {
            stateMachine.RemoveFromStateCallback(StateID, tick, stateBehaviorType);
        }

        public void SetStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType)
        {
            stateMachine.SetStateCallback(StateID, tick, stateBehaviorType);
        }

        public void AddTransition(int fromState, int toState, ITransition Check)
        {
            stateMachine.AddTransition(fromState, toState, Check);
        }

        public void RemoveTransition(int fromState, int toState)
        {
            stateMachine.RemoveTransition(fromState, toState);
        }

        #endregion
    }
}