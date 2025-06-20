using _Project.Scripts.Character;
using _Project.Scripts.Utils.Classes;
using _Project.Scripts.Utils.Enums;
using _Project.Scripts.Utils.Interfaces;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.Types
{
    public abstract class Enemy : CharacterBase, IDamageable
    {
        public override Team Team => Team.Enemy;

        protected NavMeshAgent _navMeshAgent;
        protected StateMachine _stateMachine;
        protected FindTargetsInArea _findTargetsInArea;


        protected virtual void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _findTargetsInArea = GetComponent<FindTargetsInArea>();
        }

        protected override void Start()
        {
            base.Start();
            _stateMachine = new StateMachine();
        }

        protected virtual void Update()
        {
            _stateMachine.Update();
        }
    }
}