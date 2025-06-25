using _Project.Scripts.Utils.Enums;
using _Project.Scripts.Utils.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Character
{
    public abstract class CharacterBase : NetworkBehaviour, IDamageable
    {
        public abstract Team Team { get; }

        [SerializeField] protected int _health;

        protected FindTargetsInArea _findTargetsInArea;
        private const float _onHitColorDuration = 0.1f;
        private Renderer _renderer;
        private Color _originalColor;


        protected virtual void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
            _findTargetsInArea = GetComponent<FindTargetsInArea>();
        }

        protected virtual void Update()
        {
            _findTargetsInArea.OnUpdate();
        }

        public virtual void TakeDamage(int damage)
        {
            _health -= damage;

            ChangeColorRpc();

            if (_health <= 0)
            {
                DestroyCharacter();
            }
        }

        private void DestroyCharacter()
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ChangeColorRpc()
        {
            _renderer.material.color = Color.red;
            Invoke(nameof(ResetColor), _onHitColorDuration);
        }

        private void ResetColor()
        {
            _renderer.material.color = _originalColor;
        }
    }
}