﻿using System;
using _Project.Scripts.Character;
using _Project.Scripts.Character.States;
using _Project.Scripts.Utils.Classes;
using _Project.Scripts.Utils.Enums;
using _Project.Scripts.Utils.Interfaces;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Players
{
    public class Player : CharacterBase, IDamageable
    {
        public override Team Team => Team.Player;

        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private LayerMask _groundLayer;

        private HealthBar _healthBar;
        private int _maxHealth;
        private Camera _camera;
        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Quaternion _targetRotation;
        private StateMachine _stateMachine;
        private StopWatchTimer _attackTimer;


        protected override void Awake()
        {
            base.Awake();
            _healthBar = GetComponent<HealthBar>();
            _maxHealth = _health;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
                transform.position = randomDirection;
                _rigidbody = GetComponent<Rigidbody>();
                _camera = Camera.main;
                _findTargetsInArea = GetComponent<FindTargetsInArea>();

                _attackTimer = new StopWatchTimer(_attackCooldown);
                _stateMachine = new StateMachine();
                _stateMachine.AddState(Idle(), true);
                _stateMachine.AddState(RangeAttack());
            }

            _healthBar.OnSpawn(_maxHealth);

            if (IsServer)
            {
            }
        }

        protected override void Update()
        {
            if (!IsOwner) return;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            _movement = new Vector3(horizontal, 0, vertical).normalized * _moveSpeed;

            RotatePlayer();
            base.Update();
            _attackTimer.Update(Time.deltaTime);
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            _rigidbody.linearVelocity = new Vector3(_movement.x, _rigidbody.linearVelocity.y, _movement.z);
            _rigidbody.rotation = Quaternion.Euler(0f, _targetRotation.eulerAngles.y, 0f);
        }

        public override void TakeDamage(int damage)
        {
            _health -= damage;

            ChangeColorRpc();
            _healthBar.UpdateHealthRpc(_health);

            if (_health <= 0)
            {
                DestroyCharacter();
            }
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
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.01f)
                {
                    _targetRotation = Quaternion.LookRotation(direction);
                }
            }
        }

        private StateNode RangeAttack()
        {
            ShootManager shootManager = FindAnyObjectByType<ShootManager>();

            RangeAttack rangeAttack = new RangeAttack(shootManager, _attackDamage, _attackTimer,
                GetComponent<NetworkObject>(), _findTargetsInArea, Team, false);

            bool condition() => _attackTimer.IsReady && _findTargetsInArea.ClosestTarget;

            StateNode node = new StateNode(rangeAttack, condition);
            return node;
        }

        private StateNode Idle()
        {
            Idle idle = new Idle();

            bool condition() => true;

            StateNode node = new StateNode(idle, condition);
            return node;
        }
    }
}