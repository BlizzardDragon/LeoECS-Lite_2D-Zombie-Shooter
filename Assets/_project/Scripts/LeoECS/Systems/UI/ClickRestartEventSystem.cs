using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace _project.Scripts.LeoECS.Systems.UI
{
    public class ClickRestartEventSystem : EcsUguiCallbackSystem
    {
        [Preserve]
        [EcsUguiClickEvent("Restart")]
        void OnRestartButtonClicked(in EcsUguiClickEvent evt)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}