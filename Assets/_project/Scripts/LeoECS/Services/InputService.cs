using UnityEngine;

namespace _project.Scripts.LeoECS.Services
{
    public interface IInputService
    {
        bool IsHoldLMB { get; }
        int HorizontalAxisRaw { get; }
    }

    public class InputService : IInputService
    {
        public bool IsHoldLMB => Input.GetMouseButton(0);
        public int HorizontalAxisRaw => (int) Input.GetAxisRaw("Horizontal");
    }
}