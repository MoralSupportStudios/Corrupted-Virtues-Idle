using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string description;
    public int vpReward;
    public bool isCompleted;

    // Add other properties or methods as needed, such as rewards or quest objectives.
}
