using System;
using System.Collections.Generic;

namespace LootBunnies.Systems
{
    public interface ITransitionStateMachine
    {
        // Checks all transitions and triggers state changes based on fulfilled conditions.
        public void CheckTransitions();
    }

    public interface ITransition
    {
        public bool Check();
    }

    public interface ITransitionSetup
    {
        // Adds a transition between two states.
        public void AddTransition(int fromState, int toState, ITransition Check);

        // Removes a transition between two states.
        public void RemoveTransition(int fromState, int toState);
    }

    public class Transition : ITransition
    {
        Func<bool> check;

        public Transition(Func<bool> check)
        {
            this.check = check;
        }

        public bool Check()
        {
            return check.Invoke();
        }
    }

    public class TransitionStateMachine : StateMachine, ITransitionStateMachine, ITransitionSetup
    {
        Dictionary<int, Dictionary<int, ITransition>> transitions = new Dictionary<int, Dictionary<int, ITransition>>();

        public override void Tick()
        {
            base.Tick();
            CheckTransitions();
        }

        public void AddTransition(int fromState, int toState, ITransition Check)
        {
            if (!transitions.ContainsKey(fromState))
            {
                transitions[fromState] = new Dictionary<int, ITransition>();
            }

            transitions[fromState][toState] = Check;
        }

        public void RemoveTransition(int fromState, int toState)
        {
            if (transitions.ContainsKey(fromState))
            {
                transitions[fromState].Remove(toState);
            }
        }

        public void CheckTransitions()
        {
            if (transitions.ContainsKey(currentState.UID))
            {
                foreach (KeyValuePair<int, ITransition> transition in transitions[currentState.UID])
                {
                    if (transition.Value.Check())
                    {
                        ChangeState(transition.Key);
                        break;
                    }
                }
            }
        }
    }
}