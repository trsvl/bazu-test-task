using System.Collections.Generic;
using _Project.Scripts.Enemies.States;

namespace _Project.Scripts.Utils.Classes
{
    public class StateMachine
    {
        private readonly List<StateNode> _stateNodes = new();
        private IState _currentState;

        public IState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                _currentState.OnEnter();
            }
        }

        public void AddState(StateNode stateNode, bool isInitialState = false)
        {
            _stateNodes.Add(stateNode);
            if (isInitialState) CurrentState = stateNode.State;
        }

        public void RemoveState(StateNode stateNode)
        {
            _stateNodes.Remove(stateNode);
        }

        public void Update()
        {
            foreach (var localNode in _stateNodes)
            {
                if (_currentState != localNode.State && localNode.Condition())
                {
                    _currentState.OnExit();
                    _currentState = localNode.State;
                    _currentState.OnEnter();
                }
            }

            _currentState?.OnUpdate();
        }
    }
}