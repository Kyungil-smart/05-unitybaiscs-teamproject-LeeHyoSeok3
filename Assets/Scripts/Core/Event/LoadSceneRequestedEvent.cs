public class LoadSceneRequestedEvent : IGameEvent
{
    public SceneType SceneType { get; private set; }
    public LoadSceneRequestedEvent(SceneType sceneType) => SceneType = sceneType;
}