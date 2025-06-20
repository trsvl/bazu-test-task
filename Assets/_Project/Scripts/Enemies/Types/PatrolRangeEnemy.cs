using _Project.Scripts.Character.States;
using _Project.Scripts.Utils.Classes;
using UnityEngine;

namespace _Project.Scripts.Enemies.Types
{
    public class PatrolRangeEnemy : Enemy
    {
        public Transform[] PatrolPoints { get; set; }
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Projectile _projectile;

        private StopWatchTimer _attackTimer;


        protected override void Start()
        {
            base.Start();

            _attackTimer = new StopWatchTimer(_attackCooldown);

            _stateMachine.AddState(Patrol(), true);
            _stateMachine.AddState(RangeAttack());
        }

        private StateNode Patrol()
        {
            Patrol patrol = new Patrol(_navMeshAgent, _rotationSpeed, EnemySpawner.Instance.SpawnPoints, true);

            bool condition() => !_findTargetsInArea.ClosestTarget;

            StateNode node = new StateNode(patrol, condition);
            return node;
        }

        private StateNode RangeAttack()
        {
            RangeAttack rangeAttack = new RangeAttack(_attackDamage, _attackTimer, transform, _projectile,
                _findTargetsInArea, Team, true, _navMeshAgent);

            bool condition() => _findTargetsInArea.ClosestTarget;

            StateNode node = new StateNode(rangeAttack, condition);
            return node;
        }


        protected override void Update()
        {
            _attackTimer.Update(Time.deltaTime);
            base.Update();
        }
    }
}