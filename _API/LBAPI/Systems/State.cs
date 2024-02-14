using System;
using System.Collections.Generic;

namespace LootBunnies.Systems
{
    public enum StateBehaviorType
    {
        Tick,
        Enter,
        Exit,
        FixedTick
    }

    public interface IStateControl
    {
        public void RunStateCallback(StateBehaviorType stateBehaviorType);
        public void AddCallback(Action callback, StateBehaviorType stateBehaviorType);
        public void RemoveCallback(Action callback, StateBehaviorType stateBehaviorType);
        public void SetCallback(Action callback, StateBehaviorType stateBehaviorType);
    }

    public interface IStateAction
    {
        public void RunStateCallback();
        public void AddCallback(Action callback);
        public void RemoveCallback(Action callback);
        public void SetCallback(Action callback);
    }

    public class StateAction : IStateAction
    {
        public Action action;

        public StateAction(Action action)
        {
            this.action = action;
        }

        public void RunStateCallback()
        {
            action?.Invoke();
        }

        public void AddCallback(Action callback)
        {
            action += callback;
        }
        public void RemoveCallback(Action callback)
        {
            action -= callback;
        }
        public void SetCallback(Action callback)
        {
            action = callback;
        }
    }

    public class State : IStateControl
    {
        public int UID = -1;
        private Dictionary<StateBehaviorType, IStateAction> stateBehaviors = new Dictionary<StateBehaviorType, IStateAction>();

        public State()
        {
            InitializeBehaviors();
            UID = -1;
        }

        public State(int UID) : this()
        {
            this.UID = UID;
        }

        private void InitializeBehaviors()
        {
            foreach (StateBehaviorType behaviorType in Enum.GetValues(typeof(StateBehaviorType)))
            {
                stateBehaviors[behaviorType] = new StateAction(null);
            }
        }

        public void RunStateCallback(StateBehaviorType stateBehaviorType)
        {
            stateBehaviors[stateBehaviorType].RunStateCallback();
        }

        public void AddCallback(Action callback, StateBehaviorType stateBehaviorType)
        {
            stateBehaviors[stateBehaviorType].AddCallback(callback);
        }

        public void RemoveCallback(Action callback, StateBehaviorType stateBehaviorType)
        {
            stateBehaviors[stateBehaviorType].RemoveCallback(callback);
        }

        public void SetCallback(Action callback, StateBehaviorType stateBehaviorType)
        {
            stateBehaviors[stateBehaviorType].SetCallback(callback);
        }
    }
}