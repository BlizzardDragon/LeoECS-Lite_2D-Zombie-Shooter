using UnityEngine;

namespace _project.Scripts.LeoECS.Mono.Player
{
    public class PlayerAnimationListener : MonoBehaviour
    {
        [SerializeField] private PlayerMonoObject _playerMono;
        
        public void OnShootAnimation()
        {
            _playerMono.CreateShootAnimationEvent();
        }
    }
}