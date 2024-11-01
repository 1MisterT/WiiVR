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
            _player = GameObject.Find(playerName);
            _target = GameObject.Find(targetName);
            Debug.Log($"Player: {_player != null}, Target: {_target != null}");
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void Execute()
        {
            var position = _target.transform.position;
            _player.transform.position = new Vector3(position.x, position.y, position.z);
        }
    }
}
