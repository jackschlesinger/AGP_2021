using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiniteStateMachine<TContext>
{
	private readonly TContext _context;
	private readonly Dictionary<System.Type, State> _stateCache = new Dictionary<System.Type, State>();
	public State CurrentState { get; private set; }
	public State PendingState { get; private set; }
	
	public FiniteStateMachine(TContext context)
	{
		_context = context;
	}
	
	public void Update()
	{
		PerformPendingTransition();

		Debug.Assert(CurrentState != null,
			"Updating FiniteStateMachine with null current state. Did you forget to transition to a starting state?");

		CurrentState.Update();

		PerformPendingTransition();
	}

	public void TransitionTo<TState>() where TState : State
	{
		PendingState = GetOrCreateState<TState>();
	}

	private void PerformPendingTransition()
	{
		if (PendingState == null) return;
		
		CurrentState?.OnExit();

		CurrentState = PendingState;

		CurrentState.OnEnter();

		PendingState = null;
	}

	public void ResetCurrentState()
	{
		CurrentState.OnEnter();
	}

	private TState GetOrCreateState<TState>() where TState : State
	{
		if (_stateCache.TryGetValue(typeof(TState), out var state))
		{
			return (TState) state;
		}
		
		var newState = System.Activator.CreateInstance<TState>();

		newState.Parent = this;

		newState.Initialize();

		_stateCache[typeof(TState)] = newState;

		return newState;
	}

	public void Destroy()
	{
		var states = _stateCache.Values;

		foreach (var state in states)
		{
			state.CleanUp();
			_stateCache.Remove(state.GetType());
		}
	}

	public void EndState<TState>()
	{
		if (!_stateCache.TryGetValue(typeof(TState), out var state)) return;
		
		state.CleanUp();
		_stateCache.Remove(typeof(TState));
	}

	public void EndAllButCurrentState()
	{
		var  states = _stateCache.Values;

		foreach (var state in states.Where(state => state != CurrentState))
		{
			state.CleanUp();
			_stateCache.Remove(state.GetType());
		}
	}

	public abstract class State
	{
		internal FiniteStateMachine<TContext> Parent { get; set; }

		protected TContext Context => Parent._context;

		protected void TransitionTo<TState>() where TState : State
		{
			Parent.TransitionTo<TState>();
		}

		public virtual void Initialize() { }

		public virtual void OnEnter() { }

		public virtual void OnExit() { }

		public virtual void Update() { }

		public virtual void CleanUp() { }
		
		
	}
	
	

}
