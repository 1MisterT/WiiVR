using UnityEngine;

namespace Menu.Actions
{
    public class TeleportAction : MenuAction
    {
        [SerializeField] private string playerName;
        private GameObject _player;
        [SerializeField] private string targetName;
        private GameObject _target;
    
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void Execute()
        {
            _target ??= GameObject.Find(targetName);
            _player ??= GameObject.Find(playerName);
            var position = _target.transform.position;
            
            OnTeleported?.Invoke();
            
            _player.transform.position = new Vector3(position.x, position.y, position.z);
        }

        public delegate void TeleportedEvent();
        public static event TeleportedEvent OnTeleported;
    }
}
