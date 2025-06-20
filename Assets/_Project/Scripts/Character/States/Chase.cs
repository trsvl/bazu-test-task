using _Project.Scripts.Enemies.States;
using UnityEngine.AI;

namespace _Project.Scripts.Character.States
{
    public class Chase : RotateState, IState
    {
        private readonly FindTargetsInArea _findTargetsInArea;


        public Chase(NavMeshAgent navMeshAgent, float rotationSpeed, FindTargetsInArea findTargetsInArea) : base(
            navMeshAgent, rotationSpeed)
        {
            _findTargetsInArea = findTargetsInArea;
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
            if (!_findTargetsInArea.ClosestTarget) return;

            _navMeshAgent.SetDestination(_findTargetsInArea.ClosestTarget.transform.position);
            Rotate();
        }
    }
}