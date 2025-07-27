using UnityEngine;

namespace _project.Scripts.UI
{
    public interface IScreenView
    {
        void Enable(bool value);
    }
    
    public class GameOverScreenView : MonoBehaviour, IScreenView
    {
        public void Enable(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}