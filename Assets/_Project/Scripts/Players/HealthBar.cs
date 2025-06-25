using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Players
{
    public class HealthBar : NetworkBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthText;

        private int _maxHealth;
        private Camera _camera;


        private void Start()
        {
            _camera = Camera.main;
            _canvas.transform.rotation = _camera.transform.rotation;
        }

        public void OnSpawn(int maxHealth)
        {
            _maxHealth = maxHealth;
            _healthBar.fillAmount = 1f;
            UpdateHealthRpc(_maxHealth);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void UpdateHealthRpc(int currentHealth)
        {
            _healthBar.fillAmount = Mathf.Clamp01((float)currentHealth / _maxHealth);
            _healthText.text = $"{currentHealth} / {_maxHealth}";
        }

        public void LateUpdate()
        {
            _canvas.transform.rotation = _camera.transform.rotation;
        }
    }
}