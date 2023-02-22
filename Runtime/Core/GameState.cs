using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace StoryFramework
{
    /// <summary>
    /// Supported game state value types.
    /// </summary>
    public enum GameStateTypes
    {
        BooleanFlag,
        IntegerNumber,
        FloatNumber,
        Text,
    }
    
    /// <summary>
    /// Identifier used by game states.
    /// </summary>
    [Serializable]
    public struct GameStateIdentifier
    {
        [SerializeField]
        public string Identifier;

        [SerializeField]
        public string Property;

        public static string MakeIdentifier(string identifier, string property)
        {
            return $"{identifier}.{property}";
        }

        public override string ToString()
        {
            return MakeIdentifier(Identifier, Property);
        }
    }
    
    /// <summary>
    /// Value of a game state.
    /// </summary>
    [Serializable]
    public struct GameStateValue
    {
        public delegate void ValueChangedEvent(in GameState state);
        public event ValueChangedEvent OnValueChanged;

        [SerializeField]
        public GameStateTypes Type; 

        [SerializeField]
        bool m_BooleanValue;

        [SerializeField]
        int m_IntegerValue;

        [SerializeField]
        float m_FloatValue;

        [SerializeField]
        string m_TextValue;

        void AssertType(in GameState owner, GameStateTypes type)
        {
            Assert.IsTrue(Type == GameStateTypes.BooleanFlag,
                $"Tried accessing the value of game state {owner.Identifier} as a {type} when type is " +
                $"{Type}. Please make sure the state type is correct and you are trying to access it correctly.");
        }

        public bool GetBooleanValue(in GameState owner)
        {
            AssertType(in owner, GameStateTypes.BooleanFlag);
            return m_BooleanValue;
        }

        public int GetIntegerValue(in GameState owner)
        {
            AssertType(in owner, GameStateTypes.IntegerNumber);
            return m_IntegerValue;
        }

        public float GetFloatValue(in GameState owner)
        {
            AssertType(in owner, GameStateTypes.FloatNumber);
            return m_FloatValue;
        }

        public string GetTextValue(in GameState owner)
        {
            AssertType(in owner, GameStateTypes.Text);
            return m_TextValue;
        }

        public void SetValue(in GameState owner, bool value)
        {
            AssertType(in owner, GameStateTypes.BooleanFlag);
            m_BooleanValue = value;
            OnValueChanged?.Invoke(in owner);
        }

        public void SetValue(in GameState owner, int value)
        {
            AssertType(in owner, GameStateTypes.IntegerNumber);
            m_IntegerValue = value;
            OnValueChanged?.Invoke(in owner);
        }

        public void SetValue(in GameState owner, float value)
        {
            AssertType(in owner, GameStateTypes.FloatNumber);
            m_FloatValue = value;
            OnValueChanged?.Invoke(in owner);
        }

        public void SetValue(in GameState owner, string value)
        {
            AssertType(in owner, GameStateTypes.Text);
            m_TextValue = value;
            OnValueChanged?.Invoke(in owner);
        }
    }
    
    /// <summary>
    /// A complete game state.
    /// </summary>
    [Serializable]
    public struct GameState
    {
        [SerializeField]
        public GameStateIdentifier Identifier;

        [SerializeField]
        public GameStateValue Value;
    }
}