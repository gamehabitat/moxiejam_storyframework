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
		// Game states.
		List<GameState> m_States;
		public IReadOnlyList<GameState> States => m_States;

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
				Register(in states[i].Identifier, in states[i].Value);
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
				m_States.Clear();
				Game.OnBeginLoadScene -= OnBeginLoadScene;
			}

			m_IsDisposed = true;
		}

		public void Register(in GameStateIdentifier identifier, in GameStateValue value = default)
		{
			int index = FindState(in identifier);
			Assert.IsTrue(index < 0,
				$"A state {identifier} already exist. Please try a different identifier.");

			value.OnValueChanged += OnStateValueChanged;
			m_States.Add(new GameState()
			{
				Identifier = identifier,
				Value = value
			});
		}

		public bool TryGetState(in GameStateIdentifier identifier, out GameState state)
		{
			int index = FindState(in identifier);
			if (index < 0)
			{
				state = default;
				return false;
			}

			state = m_States[index];
			return true;
		}

		public void SetState(in GameStateIdentifier identifier, in GameStateValue value)
		{
			int index = FindState(in identifier);
			if (index < 0)
			{
				Debug.Log($"Adding new state {identifier} of type {value.Type}.");
				Register(in identifier, in value);
				index = m_States.Count - 1;
			}
			else
			{
				Assert.IsTrue(m_States[index].Value.Type == value.Type,
					$"Trying to set state {identifier} with a different type. Please " +
					$"make sure correct type is used or that you are trying to set correct state.");

				m_States[index] = new()
				{
					Identifier = identifier,
					Value = value
				};
			}

			OnStateChanged?.Invoke(m_States[index]);
		}

		public int FindState(in GameStateIdentifier identifier)
		{
			string id = identifier.Identifier;
			string prop = identifier.Property;

			return m_States.FindIndex(x =>
				x.Identifier.Identifier.Equals(id, StringComparison.OrdinalIgnoreCase) &&
				x.Identifier.Property.Equals(prop, StringComparison.OrdinalIgnoreCase));
		}

		void OnStateValueChanged(in GameState state)
		{
			Debug.Log($"State {state.Identifier} of type {state.Value.Type} changed.");
		}

		void OnBeginLoadScene(string sceneName)
		{
		}
	}
}