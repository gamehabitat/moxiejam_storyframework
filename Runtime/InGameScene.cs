using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace StoryFramework
{
    public class InGameScene : MonoBehaviour
    {
        [SerializeField]
        IngameUI inGameUI;

        void Awake()
        {
            Game.Instance.IngameUi = inGameUI;
        }

        void Start()
        {
        }
    }
}