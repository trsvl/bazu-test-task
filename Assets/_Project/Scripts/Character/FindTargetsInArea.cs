using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Character
{
    public class FindTargetsInArea : NetworkBehaviour
    {
        [SerializeField] private int _maxResults = 4;
        [SerializeField] private LayerMask TargetLayer;
        [SerializeField] private bool _isDynamicTarget = true;
        private Collider[] _results;
        public float AreaRadius = 5f;

        private CharacterBase _firstClosestTarget;
        private CharacterBase _cachedClosestTarget;

        public CharacterBase ClosestTarget => _isDynamicTarget ? _cachedClosestTarget : _firstClosestTarget;


        private void Awake()
        {
            _results = new Collider[_maxResults];
        }

        public void OnUpdate()
        {
            int hits = Physics.OverlapSphereNonAlloc(transform.position, AreaRadius, _results, TargetLayer);
            if (hits <= 0)
            {
                _cachedClosestTarget = null;
                return;
            }

            CharacterBase closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;

            for (int i = 0; i < hits; i++)
            {
                var col = _results[i];
                var target = col.GetComponent<CharacterBase>();
                if (!target) continue;

                float distSqr = (target.transform.position - transform.position).sqrMagnitude;

                if (distSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distSqr;
                    closestTarget = target;
                }
            }

            _cachedClosestTarget = closestTarget;

            if (!_firstClosestTarget && closestTarget) _firstClosestTarget = closestTarget;
        }

        private void FindClosestTarget(Collider potentialTarget)
        {
            const float closestDistance = Mathf.Infinity;

            Transform closestTarget = null;

            Vector2 directionToTarget = potentialTarget.transform.localPosition - transform.localPosition;
            float directionSqr = directionToTarget.sqrMagnitude;

            if (directionSqr < closestDistance)
            {
                closestTarget = potentialTarget.transform;
            }

            if (!closestTarget) return;
            CharacterBase closestTypeTarget = closestTarget.GetComponent<CharacterBase>();
            if (!closestTypeTarget) return;

            _cachedClosestTarget = closestTypeTarget;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AreaRadius);
        }

        public float GetDistanceToTarget()
        {
            if (!ClosestTarget) return 0;

            float distance = Vector3.Distance(transform.position, ClosestTarget.transform.position);
            return distance;
        }
    }
}