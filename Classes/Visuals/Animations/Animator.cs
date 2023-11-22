using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Animator
{
    private List<Animation>_animations;
    private List<Animation>_completedAnimations;

    public Animator()
    {
        _animations = new List<Animation>();
        _completedAnimations = new List<Animation>();
    }

    public void AddAnimation(Animation animation)
    {
        _animations.Add(animation);
        animation.completed+= () =>
        {
            Console.WriteLine("AAAAAAAAAAAAAA");
            if (_animations.Contains(animation))
                _completedAnimations.Add(animation);
        };
    }


    public void Update(GameTime gameTime)
    {
        foreach (Animation animation in _animations)
            animation.Update(gameTime);

        for(int i = _completedAnimations.Count - 1; i >= 0; i--)
        {
            _animations.Remove(_completedAnimations[i]);
            _completedAnimations.RemoveAt(i);
        }
 
    }
}