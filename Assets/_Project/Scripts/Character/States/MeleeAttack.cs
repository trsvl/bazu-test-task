using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.States;
using _Project.Scripts.Utils.Classes;

namespace _Project.Scripts.Character.States
{
    public class MeleeAttack : IState
    {
        private readonly int _attackDamage;
        private readonly StopWatchTimer _attackTimer;
        private readonly FindTargetsInArea _findTargetsInArea;


        public MeleeAttack(int attackDamage, StopWatchTimer attackTimer, FindTargetsInArea findTargetsInArea)
        {
            _attackDamage = attackDamage;
            _attackTimer = attackTimer;
            _findTargetsInArea = findTargetsInArea;
        }

        public void OnEnter()
        {
            HitTarget();
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
        }

        private void HitTarget()
        {
            _attackTimer.Reset();

            if (!_findTargetsInArea.ClosestTarget) return;
            
            _findTargetsInArea.ClosestTarget.TakeDamage(_attackDamage);
        }
    }
}