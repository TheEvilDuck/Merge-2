using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class StateMachine
{
    private State _currentState;
    private Dictionary<Type,State> _states;
    
    public StateMachine()
    {
        _states = new Dictionary<Type, State>();
    }
    public void AddState(State state)
    {
        var type = state.GetType();
        if (_states.ContainsKey(type))
            return;
        _states.Add(type,state);
        if (_currentState==null)
        {
            _currentState = state;
            _currentState?.Enter();
        }
        
    }
    public void ChangeState<T>() where T: State
    {
        var type = typeof(T);
        if (!_states.ContainsKey(type))
            return;
        if (_currentState==_states[type])
            return;
        _currentState?.Exit();
        _currentState = _states[type];
        _currentState?.Enter();
    }

    public void Update(GameTime gameTime)
    {
        _currentState?.Update(gameTime);
    }
    public void Draw()
    {
        _currentState?.Draw();
    }
}