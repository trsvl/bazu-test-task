using _Project.Scripts.Utils.Enums;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Utils.Classes
{
    public class ShootManager : NetworkBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;


        [Rpc(SendTo.Server)]
        public void SpawnRpc(NetworkObjectReference projectileSpawnPointRef, Vector3 targetPosition, int attackDamage,
            Team team)
        {
            projectileSpawnPointRef.TryGet(out NetworkObject projectileSpawnPoint);

            Projectile projectile = Instantiate(_projectilePrefab, projectileSpawnPoint.transform.position,
                Quaternion.identity);

            Collider shooterCollider = projectileSpawnPoint.GetComponent<Collider>();
            Collider bulletCollider = projectile?.GetComponent<Collider>();

            if (shooterCollider && bulletCollider)
            {
                Physics.IgnoreCollision(shooterCollider, bulletCollider);
            }

            projectile?.AssignData(targetPosition, attackDamage, team);
            projectile?.GetComponent<NetworkObject>().Spawn();

        }
    }
}