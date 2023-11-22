using System;
using Microsoft.Xna.Framework;

public abstract class Animation
{
    public event Action completed;

    protected double time;
    protected double currentTime;
    private bool _playing;

    public Animation(double timeSeconds)
    {
        time = timeSeconds;
    }

    public void Play()
    {
        currentTime = 0;
        _playing = true;
    }

    public void Stop()
    {
        _playing = false;
        currentTime = time;
        completed.Invoke();
    }

    public void Update(GameTime gameTime)
    {
        if (!_playing)
            return;

        if (currentTime>=time)
        {
            Stop();
            return;
        }

        currentTime+=(float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Animate(gameTime))
            Stop();
    }

    public abstract bool Animate(GameTime gameTime);
}