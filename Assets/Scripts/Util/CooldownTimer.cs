using System.Diagnostics;
using UnityEngine;

public class CooldownTimer
{
    private float _cooldown;
    private float _lastTime;
    private float _pausedTimeOffset;
    private bool _isPaused = false;

    public CooldownTimer(float cooldown)
    {
        _cooldown = cooldown;
        _lastTime = Time.time - 4;
    }

    public void Pause()
    {
        if (_isPaused) return;
        _pausedTimeOffset = Time.time - _lastTime;
        _isPaused = true;
    }

    public void Resume()
    {
        if (!_isPaused) return;
        _lastTime = Time.time - _pausedTimeOffset;
        _isPaused = false;
    }

    public bool IsReady(float time)
    {
        if (time - _lastTime < _cooldown)
            return false;
        
        _lastTime = time;
        return true;
    }
}