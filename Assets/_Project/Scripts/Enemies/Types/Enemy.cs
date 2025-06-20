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


        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _findTargetsInArea = GetComponent<FindTargetsInArea>();
            _stateMachine = new StateMachine();
        }

        protected virtual void Update()
        {
            if (!IsServer) return;
            _stateMachine.Update();
        }
    }
}