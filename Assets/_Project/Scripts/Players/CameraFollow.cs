using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Players
{
    public class CameraFollow : NetworkBehaviour
    {
        private Vector3 _offset;
        private Player _player;
        private Camera _camera;


        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            _camera = Camera.main;
            _player = GetComponent<Player>();
            _offset = _player.transform.position - _camera.transform.position;
        }

        private void LateUpdate()
        {
            if (!IsOwner) return;

            Vector3 targetPosition = _player.transform.position - _offset;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, 5f * Time.deltaTime);
        }
    }
}