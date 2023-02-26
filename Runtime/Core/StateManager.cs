using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StoryFramework
{
	/// <summary>
	/// Manages game states through out the game.
	/// </summary>
	public class StateManager
	{
		public static StateManager Global = new();
		
		// Game states.
		List<GameStateIdentifier> m_stateIdentifiers = new();
		List<GameState> m_stateValues = new();
		public IReadOnlyList<GameStateIdentifier> StatesIdentifiers => m_stateIdentifiers;
		public IReadOnlyList<GameState> StateValues => m_stateValues;

		// Dispose flag.
		bool m_IsDisposed = false;

		public delegate void StateChangedEvent(in GameState state);
		public event StateChangedEvent OnStateChanged;

		/// <summary>
		/// Constructs a new state manager.
		/// </summary>
		public StateManager()
		{
			Game.OnBeginLoadScene += OnBeginLoadScene;
		}

		public void Load(GameState[] states)
		{
			for (int i = 0; i < states.Length; i++)
			{
				Register(in states[i]);
			}
		}
		
		/// <summary>
		/// Cleans up any resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (m_IsDisposed)
			{
				return;
			}

			if (disposing)
			{
				m_stateIdentifiers.Clear();
				m_stateValues.Clear();
				Game.OnBeginLoadScene -= OnBeginLoadScene;
			}

			m_IsDisposed = true;
		}

		public void Register(in GameState gameState)
		{
			int index = FindState(in gameState.Identifier);
			Assert.IsTrue(index < 0,
				$"A state {gameState.Identifier} already exist. Please try a different identifier.");

			m_stateIdentifiers.Add(gameState.Identifier);
			m_stateValues.Add(gameState);
		}

		// TODO: Other types.
		public GameState GetOrCreate(in GameStateIdentifier identifier, in GameStateValue initialValue)
		{
			if (TryGetState(in identifier, out var state))
			{
				return state;
			}
			
			Register(new GameState(in identifier, initialValue));

			int index = m_stateIdentifiers.Count - 1;
			return m_stateValues[index];
		}

		public bool TryGetState(in GameStateIdentifier identifier, out GameState state)
		{
			int index = FindState(in identifier);
			if (index < 0)
			{
				state = default;
				return false;
			}

			state = m_stateValues[index];
			return true;
		}

		public void SetState(in GameStateIdentifier identifier, in GameStateValue value)
		{
			int index = FindState(in identifier);
			if (index < 0)
			{
				Debug.Log($"Adding new state {identifier} of type {identifier.Type}.");
				Register(new GameState(in identifier, in value));
				index = m_stateIdentifiers.Count - 1;
			}
			else
			{
				Assert.IsTrue(m_stateIdentifiers[index].Type == value.Type,
					$"Trying to set state {identifier} with a different type. Please " +
					$"make sure correct type is used or that you are trying to set correct state.");

				m_stateIdentifiers[index] = identifier;
				m_stateValues[index] = new GameState(in identifier, in value);
			}

			OnStateChanged?.Invoke(m_stateValues[index]);
		}

		public int FindState(in GameStateIdentifier identifier)
		{
			string id = identifier.Identifier;
			string prop = identifier.Property;
			var type = identifier.Type;

			return m_stateIdentifiers.FindIndex(x =>
				x.Identifier.Equals(id, StringComparison.OrdinalIgnoreCase) &&
				x.Property.Equals(prop, StringComparison.OrdinalIgnoreCase) &&
				x.Type == type);
		}

		public bool Exists(in GameStateIdentifier identifier)
		{
			return FindState(in identifier) >= 0;
		}

		void OnStateValueChanged(in GameState state)
		{
			Debug.Log($"State {state.Identifier} of type {state.Identifier.Type} changed.");
		}

		void OnBeginLoadScene(string sceneName)
		{
		}
	}
}