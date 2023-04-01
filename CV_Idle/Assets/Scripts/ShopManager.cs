using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    // UI elements
    public GameObject shopPanel;
    public Button closeButton;
    public Transform heroListContent;
    public GameObject heroItemPrefab;

    // References
    public GameManager gameManager;

    // Initialization
    void Start()
    {
        // Set up button event listeners
        closeButton.onClick.AddListener(() => CloseShop());

        // Populate the shop with available heroes
        PopulateHeroList();
    }

    // Populate the shop with available heroes
    void PopulateHeroList()
    {
        //// Loop through the available heroes from the game manager
        //foreach (HeroData heroData in gameManager.availableHeroes)
        //{
        //    // Instantiate a new hero item prefab and add it to the hero list content
        //    GameObject heroItem = Instantiate(heroItemPrefab, heroListContent);
        //    HeroItem heroItemScript = heroItem.GetComponent<HeroItem>();

        //    // Set up the hero item UI and data
        //    heroItemScript.SetupHeroItem(heroData, gameManager);
        //}
    }

    // Open the shop
    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    // Close the shop
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
