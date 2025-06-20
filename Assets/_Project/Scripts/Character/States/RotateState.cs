using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Character.States
{
    public abstract class RotateState
    {
        protected readonly NavMeshAgent _navMeshAgent;
        protected readonly float _rotationSpeed;

        protected RotateState(NavMeshAgent navMeshAgent, float rotationSpeed)
        {
            _navMeshAgent = navMeshAgent;
            _rotationSpeed = rotationSpeed;
        }

        protected void Rotate()
        {
            Vector3 movementDirection = _navMeshAgent.velocity;

            if (movementDirection != Vector3.zero)
            {
                Quaternion pathRotation = Quaternion.LookRotation(movementDirection.normalized);
                _navMeshAgent.transform.rotation =
                    Quaternion.Slerp(_navMeshAgent.transform.rotation, pathRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}