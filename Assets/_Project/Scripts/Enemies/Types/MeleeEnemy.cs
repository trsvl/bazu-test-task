using _Project.Scripts.Character.States;
using _Project.Scripts.Enemies.States;
using _Project.Scripts.Utils.Classes;
using UnityEngine;

namespace _Project.Scripts.Enemies.Types
{
    public class MeleeEnemy : Enemy
    {
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _rotationSpeed;

        private StopWatchTimer _attackTimer;


        protected override void Start()
        {
            base.Start();

            _attackTimer = new StopWatchTimer(_attackCooldown);

            _stateMachine.AddState(Attack());
            _stateMachine.AddState(Chase(), true);
        }

        private StateNode Attack()
        {
            MeleeAttack meleeAttackState = new MeleeAttack(_attackDamage, _attackTimer, _findTargetsInArea);

            bool condition() => _attackTimer.IsReady && _findTargetsInArea.ClosestTarget &&
                                _findTargetsInArea.GetDistanceToTarget() <= _attackRadius;

            StateNode node = new StateNode(meleeAttackState, condition);
            return node;
        }

        private StateNode Chase()
        {
            Chase chaseState = new Chase(_navMeshAgent, _rotationSpeed, _findTargetsInArea, false);

            bool condition() => _findTargetsInArea.ClosestTarget;

            StateNode node = new StateNode(chaseState, condition);
            return node;
        }

        protected override void Update()
        {
            _attackTimer.Update(Time.deltaTime);
            base.Update();
        }
    }
}