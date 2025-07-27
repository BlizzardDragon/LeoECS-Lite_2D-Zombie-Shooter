using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.View
{
    public interface IHealthBarView
    {
        float FillAmount { get; }

        void RenderFillAmount(float fillAmount);
    }

    public class HealthBarView : MonoBehaviour, IHealthBarView
    {
        [SerializeField] private Image _image;
        
        public float FillAmount => _image.fillAmount;

        public void RenderFillAmount(float fillAmount)
        {
            _image.fillAmount = fillAmount;
        }
    }
}