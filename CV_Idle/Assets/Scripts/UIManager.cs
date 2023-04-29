using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas mainCanvas;
    public TMP_Text stageCounter;
    public TMP_Text virtuePoints;
    public GameObject backgroundImage;
    public HealthBar healthBarPrefab;
    public RectTransform enemySpawn;
    public Transform abilityButtonsContainer;
    public GameObject ShopPanel;
    public GameManager gameManager;
    public GameObject Button;
    public GameObject ButtonPanel;

    public void ToggleButtonPanel()
    {
        if (ButtonPanel.activeSelf)
        {
            ButtonPanel.SetActive(false);
        }
        else
        {
            ButtonPanel.SetActive(true);
        }

        //ButtonPanel.SetActive(!gameObject.activeSelf);

        gameManager.enemyManager.CurrentEnemy.SetActive(!ButtonPanel.activeSelf);
        gameManager.enemyManager.CurrentEnemy.GetComponent<Enemy>().healthBarInstance.gameObject.SetActive(!ButtonPanel.activeSelf);

        foreach (GameObject hero in gameManager.heroManager.heroParty)
        {
            hero.SetActive(!ButtonPanel.activeSelf);
        }
        abilityButtonsContainer.gameObject.SetActive(!ButtonPanel.activeSelf);
        

    }

    public AbilityButton CreateAbilityButton(GameObject abilityButtonPrefab, Hero hero, int heroIndex)
    {
        // Instantiate the ability button prefab and set its parent to the UI canvas
        AbilityButton abilityButtonInstance = Instantiate(abilityButtonPrefab).GetComponent<AbilityButton>();
        abilityButtonInstance.transform.SetParent(abilityButtonsContainer, false);

        // Set the ability button position
        float buttonWidth = abilityButtonInstance.GetComponent<RectTransform>().rect.width;
        float containerWidth = abilityButtonsContainer.GetComponent<RectTransform>().rect.width;

        abilityButtonInstance.transform.localPosition = new Vector3(-containerWidth / 2 + buttonWidth / 2 + heroIndex * buttonWidth, 0, 0);

        // Initialize the ability button with the hero and GameManager references
        abilityButtonInstance.Initialize(hero, gameManager);

        return abilityButtonInstance;
    }

    public void UpdateVirtuePointsText(int vp)
    {
        virtuePoints.text = $"Virtue Points: {vp}";
    }

    public void UpdateStageDisplay(bool isBoss, int stage, int round, int cycle)
    {
        //Adding +1 to displayed because we don't them to start at 0
        stageCounter.text = isBoss
            ? $"Stage: {stage + 1} Boss {ToRoman(cycle)}"
            : $"Stage: {stage + 1}-{round + 1} {ToRoman(cycle)}";
    }

    public void UpdateBackgroundImage(Sprite background)
    {
        backgroundImage.GetComponent<Image>().sprite = background;
    }

    public static string ToRoman(int number)
    {
        switch (number)
        {
            case < 0:
                return string.Empty;
            case > 3999:
                throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            case < 1:
                return string.Empty;
            case >= 1000:
                return "M" + ToRoman(number - 1000);
            case >= 900:
                return "CM" + ToRoman(number - 900);
            case >= 500:
                return "D" + ToRoman(number - 500);
            case >= 400:
                return "CD" + ToRoman(number - 400);
            case >= 100:
                return "C" + ToRoman(number - 100);
            case >= 90:
                return "XC" + ToRoman(number - 90);
            case >= 50:
                return "L" + ToRoman(number - 50);
            case >= 40:
                return "XL" + ToRoman(number - 40);
            case >= 10:
                return "X" + ToRoman(number - 10);
            case >= 9:
                return "IX" + ToRoman(number - 9);
            case >= 5:
                return "V" + ToRoman(number - 5);
            case >= 4:
                return "IV" + ToRoman(number - 4);
            case >= 1:
                return "I" + ToRoman(number - 1);
            default:
                throw new InvalidOperationException("Impossible state reached");
        }
    }
}
