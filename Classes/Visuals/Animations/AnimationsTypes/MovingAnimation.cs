using System;
using Microsoft.Xna.Framework;

public class MovingAnimation : Animation
{
    private const float ACCURACY = 2.2f;
    private VisualObject _target;
    private Vector2 _toPos;
    public MovingAnimation(VisualObject target, Vector2 toPos,double timeMiliseconds): base(timeMiliseconds)
    {
        _target = target;
        _toPos = toPos;

        completed+=()=>
        {
            _target.MoveTo(toPos);
        };
    }
    public override bool Animate(GameTime gameTime)
    {
        Vector2 moveVector = _toPos-_target.Position;

        if (Vector2.Distance(_toPos,_target.Position)<=ACCURACY)
            return true;

        Vector2 newPos = _target.Position+moveVector*(float)(currentTime/time);

        _target.MoveTo(newPos);

        return false;
    }
}