using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private LayerMask _groundLayer;
        private Camera _camera;
        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Quaternion _targetRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _movement = new Vector3(horizontal, 0, vertical);

            RotatePlayer();
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _movement * _moveSpeed;
            _rigidbody.rotation = Quaternion.Euler(0f, _targetRotation.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Rotates the player character around its Y-axis based on the mouse's world position on the ground.
        /// </summary>
        private void RotatePlayer()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _groundLayer))
            {
                Vector3 targetPosition = hit.point;

                Vector3 direction = targetPosition - transform.position;

                if (direction != Vector3.zero)
                {
                    _targetRotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }
}