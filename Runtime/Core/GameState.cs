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
    /// Game state property identifier and type. 
    /// </summary>
    [Serializable]
    public struct GameStateProperty 
    {
        [SerializeField]
        public string Name;

        [SerializeField]
        public GameStateTypes Type;
        
        public GameStateProperty(string name, GameStateTypes type)
        {
            Name = name;
            Type = type;
        }
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

        [SerializeField]
        public GameStateTypes Type; 

        public GameStateIdentifier(string identifier, string property, GameStateTypes type)
        {
            Identifier = identifier;
            Property = property;
            Type = type;
        }

        public static string MakeIdentifier(string identifier, string property, GameStateTypes type)
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
                Property.Equals(other.Property, StringComparison.OrdinalIgnoreCase) &&
                Type == other.Type;
        }

        public override string ToString()
        {
            return MakeIdentifier(Identifier, Property, Type);
        }
    }
    
    /// <summary>
    /// Value of a game state.
    /// </summary>
    public readonly struct GameStateValue
    {
        public readonly GameStateTypes Type; 
        public readonly bool BooleanValue;
        public readonly int IntegerValue;
        public readonly float FloatValue;
        public readonly string TextValue;

        public GameStateValue(bool value)
        {
            Type = GameStateTypes.BooleanFlag;
            BooleanValue = value;
            IntegerValue = default;
            FloatValue = default;
            TextValue = default;
        }

        public GameStateValue(int value)
        {
            Type = GameStateTypes.IntegerNumber;
            BooleanValue = default;
            IntegerValue = value;
            FloatValue = default;
            TextValue = default;
        }

        public GameStateValue(float value)
        {
            Type = GameStateTypes.FloatNumber;
            BooleanValue = default;
            IntegerValue = default;
            FloatValue = value;
            TextValue = default;
        }

        public GameStateValue(string value)
        {
            Type = GameStateTypes.Text;
            BooleanValue = default;
            IntegerValue = default;
            FloatValue = default;
            TextValue = value;
        }

        public static implicit operator bool(in GameStateValue state) => state.BooleanValue;
        public static implicit operator GameStateValue(in bool value) => new(value);

        public static implicit operator int(in GameStateValue state) => state.IntegerValue;
        public static implicit operator GameStateValue(in int value) => new(value);

        public static implicit operator float(in GameStateValue state) => state.FloatValue;
        public static implicit operator GameStateValue(in float value) => new(value);

        public static implicit operator string(in GameStateValue state) => state.TextValue;
        public static implicit operator GameStateValue(in string value) => new(value);

        void AssertType(in GameState owner, GameStateTypes type)
        {
            Assert.IsTrue(Type == GameStateTypes.BooleanFlag,
                $"Tried accessing the value of game state {owner.Identifier} as a {type} when type is " +
                $"{Type}. Please make sure the state type is correct and you are trying to access it correctly.");
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
        public string Owner;

        [SerializeField]
        public GameStateProperty Property;

        [SerializeField]
        bool m_BooleanValue;

        [SerializeField]
        int m_IntegerValue;

        [SerializeField]
        float m_FloatValue;

        [SerializeField]
        string m_TextValue;

        public bool BooleanValue => GetValueBoolean();//Value.GetBooleanValue(in this);
        public int IntegerValue => GetValueInteger();//Value.GetIntegerValue(in this);
        public float FloatValue => GetValueFloat();//Value.GetFloatValue(in this);
        public string TextValue => GetValueText();//Value.GetTextValue(in this);

        public bool BooleanValue2
        {
            get => GetValueBoolean();
            set => SetValue(value);
        }
        public string TextValue2
        {
            get => GetValueText();
            set => SetValue(value);
        }

        public GameState(in GameStateIdentifier identifier)
        {
            Identifier = identifier;
            Owner = identifier.Identifier;
            Property = new GameStateProperty(identifier.Property, identifier.Type);
            m_BooleanValue = default;
            m_IntegerValue = default;
            m_FloatValue = default;
            m_TextValue = default;
        }

        public GameState(in GameStateIdentifier identifier, in GameStateValue value) : this(in identifier)
        {
            AssertType(value.Type);
            switch (value.Type)
            {
            case GameStateTypes.BooleanFlag:
                m_BooleanValue = value.BooleanValue;
                break;
            case GameStateTypes.IntegerNumber:
                m_IntegerValue = value.IntegerValue;
                break;
            case GameStateTypes.FloatNumber:
                m_FloatValue = value.FloatValue;
                break;
            case GameStateTypes.Text:
                m_TextValue = value.TextValue;
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }
        
        public GameState(in GameStateIdentifier identifier, bool value) : this(in identifier)
        {
            AssertType(GameStateTypes.BooleanFlag);
            m_BooleanValue = value;
        }

        public GameState(in GameStateIdentifier identifier, int value) :this(in identifier)
        {
            AssertType(GameStateTypes.IntegerNumber);
            m_IntegerValue = value;
        }

        public GameState(in GameStateIdentifier identifier, float value) :this(in identifier)
        {
            AssertType(GameStateTypes.FloatNumber);
            m_FloatValue = value;
        }

        public GameState(in GameStateIdentifier identifier, string value) :this(in identifier)
        {
            AssertType(GameStateTypes.Text);
            m_TextValue = value;
        }

        public bool GetValueBoolean() => StateManager.Global.GetOrCreate(Identifier, default(bool));
        public int GetValueInteger() => StateManager.Global.GetOrCreate(Identifier, default(int));
        public float GetValueFloat() => StateManager.Global.GetOrCreate(Identifier, default(float));
        public string GetValueText() => StateManager.Global.GetOrCreate(Identifier, default(string));

        public void SetValue(bool value) => StateManager.Global.SetState(Identifier, value);
        public void SetValue(int value) => StateManager.Global.SetState(Identifier, value);
        public void SetValue(float value) => StateManager.Global.SetState(Identifier, value);
        public void SetValue(string value) => StateManager.Global.SetState(Identifier, value);

        public static implicit operator bool(in GameState state) => state.BooleanValue;
        public static implicit operator int(in GameState state) => state.IntegerValue;
        public static implicit operator float(in GameState state) => state.FloatValue;
        public static implicit operator string(in GameState state) => state.TextValue;

        void AssertType(GameStateTypes type)
        {
            Assert.IsTrue(Identifier.Type == type,
                $"Tried accessing the value of game state {Identifier} as a {type} when type is " +
                $"{Identifier.Type}. Please make sure the state type is correct and you are trying to access it " +
                $"correctly.");
        }
    }
}