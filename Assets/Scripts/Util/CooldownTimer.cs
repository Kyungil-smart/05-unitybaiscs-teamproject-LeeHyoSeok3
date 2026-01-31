public class CooldownTimer
{
    private float _cooldown;
    private float _lastTime;
    
    public CooldownTimer(float cooldown)
    {
        _cooldown = cooldown;
        _lastTime = Time.time;
    }

    public bool IsReady(float time)
    {
        if (time - _lastTime < _cooldown)
            return false;
        
        _lastTime = time;
        return true;
    }
}