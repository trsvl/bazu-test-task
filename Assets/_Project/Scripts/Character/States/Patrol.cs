using _Project.Scripts.Enemies.States;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Character.States
{
    public class Patrol : RotateState, IState
    {
        private readonly Transform[] _points;
        private readonly bool _isStoppedOnChangeState;
        private int _currentIndex;

        public Patrol(NavMeshAgent agent, float rotationSpeed, Transform[] points, bool isStoppedOnChangeState)
            : base(agent, rotationSpeed)
        {
            _points = points;
            _isStoppedOnChangeState = isStoppedOnChangeState;

            GetNextPatrolPoint();
        }

        public void OnEnter()
        {
            _navMeshAgent.isStopped = false;
        }

        public void OnUpdate()
        {
            if (_points.Length == 0) return;

            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 1.5f)
            {
                GetNextPatrolPoint();
            }

            Rotate();
        }

        public void OnExit()
        {
            if (_isStoppedOnChangeState) _navMeshAgent.isStopped = true;
        }

        private void GetNextPatrolPoint()
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, _points.Length);
            } while (newIndex == _currentIndex);

            _currentIndex = newIndex;
            _navMeshAgent.SetDestination(_points[_currentIndex].position);
        }
    }
}