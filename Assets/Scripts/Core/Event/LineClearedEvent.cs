public class LineClearedEvent : IGameEvent
{
    public int Count { get; }
    public LineClearedEvent(int count) => Count = count;
}