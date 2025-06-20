namespace _Project.Scripts.Utils.Classes
{
    public class StopWatchTimer
    {
        private readonly float _cooldown;
        private float _timer;
        private bool _isReady;
        private bool _isStopped;

        public bool IsReady => _isReady;


        public StopWatchTimer(float cooldown)
        {
            _cooldown = cooldown;
            _isReady = true;
        }

        public void Update(float time)
        {
            if (_isReady || _isStopped) return;

            if (_cooldown > _timer)
            {
                _timer += time;
            }
            else
            {
                _isReady = true;
            }
        }

        public void Reset()
        {
            _isReady = false;
            _isStopped = false;
            _timer = 0f;
        }

        public void Stop()
        {
            _isStopped = true;
        }

        public void Continue()
        {
            _isStopped = false;
        }
    }
}