using System;
using _Project.Scripts.Utils.Enums;
using _Project.Scripts.Utils.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Character
{
    public abstract class CharacterBase : MonoBehaviour, IDamageable
    {
        public abstract Team Team { get; }

        [SerializeField] protected int _health;

        private const float _onHitColorDuration = 0.1f;
        private Renderer _renderer;
        private Color _originalColor;


        protected virtual void Start()
        {
            _renderer = GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;

            ChangeColor();

            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void ChangeColor()
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