using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;

namespace _project.Scripts.LeoECS.Systems.UI
{
    public class ClickExitEventSystem : EcsUguiCallbackSystem
    {
        [Preserve]
        [EcsUguiClickEvent("Exit")]
        void OnExitButtonClicked(in EcsUguiClickEvent evt)
        {
            Application.Quit();
        }
    }
}