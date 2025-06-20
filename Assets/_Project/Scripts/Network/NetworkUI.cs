using _Project.Scripts.Enemies;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Network
{
    public class NetworkUI : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;

        private bool _isHosted;


        private void Start()
        {
            _hostButton.onClick.AddListener(StartHostClick);
            _clientButton.onClick.AddListener(StartClientClick);
        }

        private void StartHostClick()
        {
            if (_isHosted) return;
            _isHosted = true;
            NetworkManager.Singleton.StartHost();
            EnemySpawner.Instance.StartSpawn();
        }

        private void StartClientClick()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}