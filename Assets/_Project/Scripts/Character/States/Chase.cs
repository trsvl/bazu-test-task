using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.States;
using UnityEngine.AI;

namespace _Project.Scripts.Character.States
{
    public class Chase : RotateState, IState
    {
        private readonly FindTargetsInArea _findTargetsInArea;
        private readonly bool _isDynamicTarget;


        public Chase(NavMeshAgent navMeshAgent, float rotationSpeed, FindTargetsInArea findTargetsInArea,
            bool isDynamicTarget) : base(
            navMeshAgent, rotationSpeed)
        {
            _findTargetsInArea = findTargetsInArea;
            _isDynamicTarget = isDynamicTarget;
        }

        public void OnEnter()
        {
        }

        public void OnUpdate()
        {
            OnChase();
        }

        public void OnExit()
        {
        }

        private void OnChase()
        {
            if (_isDynamicTarget ? !_findTargetsInArea.ClosestTarget : !_findTargetsInArea.FirstClosestTarget) return;

            _navMeshAgent.SetDestination(_isDynamicTarget
                ? _findTargetsInArea.ClosestTarget.transform.position
                : _findTargetsInArea.FirstClosestTarget.transform.position);

            Rotate();
        }
    }
}