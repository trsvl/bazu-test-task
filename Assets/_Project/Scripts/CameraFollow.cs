using UnityEngine;

namespace _Project.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 _offset;
        private Player _player;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _offset = _player.transform.position - _camera.transform.position;
        }

        private void LateUpdate()
        {
            _camera.transform.position = _player.transform.position - _offset;
        }
    }
}