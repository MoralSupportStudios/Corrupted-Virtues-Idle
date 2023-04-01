using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroData", menuName = "ScriptableObjects/HeroData", order = 1)]
public class HeroData : ScriptableObject
{
    public string heroName;
    public int attackDamage;
    public float attackSpeed;
    public int abilityDamage;
    public Sprite heroSprite;
}
