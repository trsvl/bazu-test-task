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

        private int _damage;
        private Team _shooterTeam;
        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void AssignData(Vector3 targetPosition, int damage, Team shooterTeam)
        {
            _damage = damage;
            _shooterTeam = shooterTeam;

            var bulletDirection = (targetPosition - transform.position).normalized;
            _rigidbody.linearVelocity = bulletDirection * _speed;

            if (!IsServer) return;

            DestroyRpc(_lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null && damageable.Team != _shooterTeam)
            {
                damageable.TakeDamage(_damage);
            }

            DestroyRpc();
        }

        [Rpc(SendTo.Server)]
        private void DestroyRpc(float lifetime = 0f)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject, lifetime);
        }
    }
}