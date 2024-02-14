using System;
using System.Collections.Generic;

namespace LootBunnies.Systems
{
    /// <summary>
    /// Interface for the main behavior of a state machine.
    /// </summary>
    public interface IStateMachine
    {
        public void Tick();
        public void FixedTick();
        public void ChangeState(int stateUID);
    }

    /// <summary>
    /// Interface for managing states.
    /// </summary>
    public interface IStateManagement
    {
        public void AddState(State state);
        public void AddState(int stateID);
        public void RemoveState(int stateUID);
    }

    /// <summary>
    /// Interface for adding behaviors to states.
    /// </summary>
    public interface IStateBehaviorModificationAddition
    {
        public void AddToStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType);
    }

    /// <summary>
    /// Interface for removing behaviors form states.
    /// </summary>
    public interface IStateBehaviorModificationRemoval
    {
        public void RemoveFromStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType);
    }

    /// <summary>
    /// Interface for setting behaviors of states.
    /// </summary>
    public interface IStateBehaviorModificationSet
    {
        public void SetStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType);
    }

    /// <summary>
    /// Implementation of a state machine.
    /// Tick - Runs the Update behavior of the current state.
    /// Fixed Tick - Runs the Fixed Update behavior of the current state.
    /// </summary>
    public class StateMachine : IStateMachine, IStateManagement, IStateBehaviorModificationAddition, IStateBehaviorModificationRemoval, IStateBehaviorModificationSet
    {
        Dictionary<int, State> states = new Dictionary<int, State>();
        protected State currentState;
        protected State previousState;

        public StateMachine() { }

        public StateMachine(State initialState)
        {
            AddState(initialState);
            currentState = initialState;
            currentState.RunStateCallback(StateBehaviorType.Enter);
        }

        public StateMachine(int[] UIDs, int initialState)
        {
            foreach (int i in UIDs)
            {
                AddState(new State(i));
            }
            ChangeState(initialState);
        }

        public virtual void Tick()
        {
            if (currentState != null)
                currentState.RunStateCallback(StateBehaviorType.Tick);
        }

        public virtual void FixedTick()
        {
            if (currentState != null)
                currentState.RunStateCallback(StateBehaviorType.FixedTick);
        }

        public void AddState(State state)
        {
            states.Add(state.UID, state);
        }

        public void AddState(int stateID)
        {
            AddState(new State(stateID));
        }

        public void RemoveState(int stateUID)
        {
            states.Remove(stateUID);
        }

        public void ChangeState(int stateUID)
        {
            if (!states.ContainsKey(stateUID))
            {
                throw new Exception("State with UID " + stateUID + " does not exist.");
            }
            if (currentState == null)
            {
                currentState = states[stateUID];
                currentState.RunStateCallback(StateBehaviorType.Enter);
                return;
            }
            if (currentState.UID == stateUID)
                return;

            previousState = currentState;
            currentState.RunStateCallback(StateBehaviorType.Exit);
            currentState = states[stateUID];
            currentState.RunStateCallback(StateBehaviorType.Enter);
        }

        public void RevertToPreviousState()
        {
            ChangeState(previousState.UID);
        }

        public State GetCurrentState()
        {
            return currentState;
        }

        public State GetPreviousState()
        {
            return previousState;
        }

        #region State Support Methods

        public void AddToStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType)
        {
            states[StateID].AddCallback(tick, stateBehaviorType);
        }

        public void RemoveFromStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType)
        {
            states[StateID].RemoveCallback(tick, stateBehaviorType);
        }

        public void SetStateCallback(int StateID, Action tick, StateBehaviorType stateBehaviorType)
        {
            states[StateID].SetCallback(tick, stateBehaviorType);
        }
        #endregion
    }
}