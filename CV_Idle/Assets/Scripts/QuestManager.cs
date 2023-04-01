using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public int requiredStage;
    public bool isCompleted;
}

public class QuestManager : MonoBehaviour
{
    // Quests
    public List<Quest> quests;

    // Initialization
    void Start()
    {
        // Set initial quest states (you can customize this to load saved quest states)
        InitializeQuests();
    }

    // Initialize quests
    void InitializeQuests()
    {
        foreach (Quest quest in quests)
        {
            quest.isCompleted = false;
        }
    }

    // Check if a quest is completed
    public void CheckQuestCompletion(int currentStage)
    {
        foreach (Quest quest in quests)
        {
            if (!quest.isCompleted && currentStage >= quest.requiredStage)
            {
                quest.isCompleted = true;
                Debug.Log($"Quest '{quest.questName}' completed!");
                // Perform quest reward or action here
            }
        }
    }

    // Other quest management functions
    // ...
}
