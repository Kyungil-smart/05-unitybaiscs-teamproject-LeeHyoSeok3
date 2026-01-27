using System;
using System.Collections.Generic;

public static class GameEventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _subscribers
        = new Dictionary<Type, List<Delegate>>();

    // 구독
    public static void Subscribe<T>(Action<T> callback)
        where T : IGameEvent
    {
        var type = typeof(T);

        if (!_subscribers.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            _subscribers[type] = list;
        }

        if (!list.Contains(callback))
            list.Add(callback);
    }

    // 구독 해제
    public static void Unsubscribe<T>(Action<T> callback)
        where T : IGameEvent
    {
        var type = typeof(T);

        if (_subscribers.TryGetValue(type, out var list))
        {
            list.Remove(callback);
        }
    }

    // 이벤트 발행
    public static void Raise<T>(T gameEvent)
        where T : IGameEvent
    {
        var type = typeof(T);

        if (!_subscribers.TryGetValue(type, out var list))
            return;

        // 복사본으로 순회 (구독 중 변경 방지)
        var snapshot = list.ToArray();
        foreach (var subscriber in snapshot)
        {
            ((Action<T>)subscriber)?.Invoke(gameEvent);
        }
    }

    // 전체 초기화 (씬 전환 시)
    public static void Clear()
    {
        _subscribers.Clear();
    }
}