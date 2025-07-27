using TMPro;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class AmmoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _count;

        public void RenderCount(string count)
        {
            _count.text = count;
        }
    }
}