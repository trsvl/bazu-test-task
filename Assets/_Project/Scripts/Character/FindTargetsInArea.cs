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

        public CharacterBase ClosestTarget => _isDynamicTarget ? _firstClosestTarget : _cachedClosestTarget;


        private void Awake()
        {
            _results = new Collider[_maxResults];
        }

        private void Update()
        {
            int hits = Physics.OverlapSphereNonAlloc(transform.position, AreaRadius, _results, TargetLayer);

            CheckRange();

            if (hits <= 0) return;

            for (int i = 0; i < hits; i++)
            {
                Collider col = _results[i];
                FindClosestTarget(col);
            }
        }

        private void CheckRange()
        {
            if (GetDistanceToTarget() > AreaRadius)
            {
                _firstClosestTarget = null;
            }
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
            if (!_firstClosestTarget) _firstClosestTarget = _cachedClosestTarget;
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