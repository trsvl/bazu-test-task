using _Project.Scripts.Utils.Enums;

namespace _Project.Scripts.Utils.Interfaces
{
    public interface IDamageable
    {
        public Team Team { get; }
        public void TakeDamage(int damage);
    }
}