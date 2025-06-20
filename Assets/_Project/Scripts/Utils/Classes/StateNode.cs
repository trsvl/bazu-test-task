using System;
using _Project.Scripts.Enemies.States;

namespace _Project.Scripts.Utils.Classes
{
    public class StateNode
    {
        public IState State { get; private set; }
        public Func<bool> Condition { get; private set; }


        public StateNode(IState state, Func<bool> condition)
        {
            State = state;
            Condition = condition;
        }
    }
}