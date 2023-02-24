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
    public struct GameStateIdentifier : IEquatable<GameStateIdentifier>
    {
        [SerializeField]
        public string Identifier;

        [SerializeField]
        public string Property;

        public GameStateIdentifier(string identifier, string property)
        {
            Identifier = identifier;
            Property = property;
        }

        public static string MakeIdentifier(string identifier, string property)
        {
            return $"{identifier}[{property}]";
        }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Identifier)) && (!string.IsNullOrEmpty(Property));
        }
        
        public bool Equals(GameStateIdentifier other)
        {
            return
                Identifier.Equals(other.Identifier, StringComparison.OrdinalIgnoreCase) &&
                Property.Equals(other.Property, StringComparison.OrdinalIgnoreCase);
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

        public GameStateValue(bool value)
        {
            Type = GameStateTypes.BooleanFlag;
            m_BooleanValue = value;
            m_IntegerValue = default;
            m_FloatValue = default;
            m_TextValue = default;
        }

        public GameStateValue(int value)
        {
            Type = GameStateTypes.BooleanFlag;
            m_BooleanValue = default;
            m_IntegerValue = value;
            m_FloatValue = default;
            m_TextValue = default;
        }

        public GameStateValue(float value)
        {
            Type = GameStateTypes.BooleanFlag;
            m_BooleanValue = default;
            m_IntegerValue = default;
            m_FloatValue = value;
            m_TextValue = default;
        }

        public GameStateValue(string value)
        {
            Type = GameStateTypes.BooleanFlag;
            m_BooleanValue = default;
            m_IntegerValue = default;
            m_FloatValue = default;
            m_TextValue = value;
        }

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
        }

        public void SetValue(in GameState owner, int value)
        {
            AssertType(in owner, GameStateTypes.IntegerNumber);
            m_IntegerValue = value;
        }

        public void SetValue(in GameState owner, float value)
        {
            AssertType(in owner, GameStateTypes.FloatNumber);
            m_FloatValue = value;
        }

        public void SetValue(in GameState owner, string value)
        {
            AssertType(in owner, GameStateTypes.Text);
            m_TextValue = value;
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

        public bool BooleanValue => Value.GetBooleanValue(in this);
        public int IntegerValue => Value.GetIntegerValue(in this);
        public float FloatValue => Value.GetFloatValue(in this);
        public string TextValue => Value.GetTextValue(in this);
        
        public static implicit operator bool(in GameState state) => state.BooleanValue;
        public static implicit operator int(in GameState state) => state.IntegerValue;
        public static implicit operator float(in GameState state) => state.FloatValue;
        public static implicit operator string(in GameState state) => state.TextValue;
    }
}