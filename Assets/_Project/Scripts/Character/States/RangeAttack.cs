using _Project.Scripts.Enemies.States;
using _Project.Scripts.Utils.Classes;
using _Project.Scripts.Utils.Enums;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Character.States
{
    public class RangeAttack : IState
    {
        private readonly ShootManager _shootManager;
        private readonly int _attackDamage;
        private readonly StopWatchTimer _attackTimer;
        private readonly NetworkObjectReference _projectileSpawnPointRef;
        private readonly FindTargetsInArea _findTargetsInArea;
        private readonly Team _team;
        private readonly bool _isShootInUpdate;
        private readonly NavMeshAgent _navMeshAgent;


        public RangeAttack(ShootManager shootManager, int attackDamage, StopWatchTimer attackTimer,
            NetworkObjectReference projectileSpawnPointRef, FindTargetsInArea findTargetsInArea, Team team,
            bool isShootInUpdate, NavMeshAgent navMeshAgent = null)
        {
            _shootManager = shootManager;
            _attackDamage = attackDamage;
            _attackTimer = attackTimer;
            _projectileSpawnPointRef = projectileSpawnPointRef;
            _findTargetsInArea = findTargetsInArea;
            _team = team;
            _isShootInUpdate = isShootInUpdate;
            _navMeshAgent = navMeshAgent;
        }

        public void OnEnter()
        {
            if (!_isShootInUpdate) SpawnProjectile();
        }

        public void OnUpdate()
        {
            if (_isShootInUpdate) SpawnProjectile();
            Rotate();
        }

        public void OnExit()
        {
        }

        private void SpawnProjectile()
        {
            if (!_attackTimer.IsReady) return;
            if (!_findTargetsInArea.ClosestTarget) return;

            _attackTimer.Reset();

            _shootManager.SpawnRpc(
                _projectileSpawnPointRef,
                _findTargetsInArea.ClosestTarget.transform.position,
                _attackDamage,
                _team
            );
        }

        private void Rotate()
        {
            if (!_findTargetsInArea.ClosestTarget) return;
            if (!_navMeshAgent) return;

            Vector3 directionToTarget =
                _findTargetsInArea.ClosestTarget.transform.position - _navMeshAgent.transform.position;

            if (directionToTarget.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget.normalized);
                _navMeshAgent.transform.rotation =
                    Quaternion.Slerp(_navMeshAgent.transform.rotation, lookRotation, 8f * Time.deltaTime);
            }
        }
    }
}