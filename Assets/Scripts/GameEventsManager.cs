using UnityEngine;

/// <summary>
/// Handles all NON-INPUT events. Classes should subscribe/unsubscribe from events here.
/// 
/// IMPORTANT: this has high priority in script execution order! So you can legally call
/// Instance.someEvents.eventName += Your Delegate in Awake() in any MonoBehaviors!
/// </summary>
public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    public PlayerEvents PlayerEvents;
    public QuestEvents QuestEvents;
    public BedrockPlainsEvents BedrockPlainsEvents;
    public GameStateEvents GameStateEvents;

    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // initialize all events
        QuestEvents = new QuestEvents();
        PlayerEvents = new PlayerEvents();
        BedrockPlainsEvents = new BedrockPlainsEvents();
        GameStateEvents = new GameStateEvents();
    }
}