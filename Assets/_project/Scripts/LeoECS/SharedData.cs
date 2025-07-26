using System;
using _project.Scripts.Configs;
using UnityEngine;

namespace _project.Scripts.LeoECS
{
    [Serializable]
    public class SharedData
    {
        [field: SerializeField] public Transform BulletContainer { get; private set; }
        [field: SerializeField] public PlayerConfig PlayerConfig { get; private set; }
        [field: SerializeField] public BulletConfig BulletConfig { get; private set; }
    }
}