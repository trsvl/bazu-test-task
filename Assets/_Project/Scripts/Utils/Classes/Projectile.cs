using _Project.Scripts.Utils.Enums;
using _Project.Scripts.Utils.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Utils.Classes
{
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _lifetime = 5f;
        [SerializeField] private Collider _collider;

        private int _damage;
        private Team _shooterTeam;
        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider.enabled = false;
        }

        public void AssignData(Vector3 targetPosition, int damage, Team shooterTeam)
        {
            _damage = damage;
            _shooterTeam = shooterTeam;
            _collider.enabled = true;
            var bulletDirection = (targetPosition - transform.position).normalized;
            _rigidbody.linearVelocity = bulletDirection * _speed;

            Destroy(_lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null && damageable.Team != _shooterTeam)
            {
                damageable.TakeDamage(_damage);
            }

            Destroy();
        }

        private void Destroy(float lifetime = 0f)
        {
            if (!IsServer) return;

            if (NetworkObject && NetworkObject.IsSpawned)
            {
                gameObject.GetComponent<NetworkObject>().Despawn();
            }

            Destroy(gameObject, lifetime);
        }
    }
}