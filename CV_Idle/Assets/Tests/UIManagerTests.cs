using NUnit.Framework;
using TMPro;
using UnityEngine;

[TestFixture]
public class UIManagerTests
{
    private UIManager uiManager;
    private GameObject uiManagerObject;
    private TMP_Text virtuePoints;

    [SetUp]
    public void Setup()
    {
        uiManagerObject = new GameObject();
        uiManager = uiManagerObject.AddComponent<UIManager>();

        virtuePoints = new GameObject().AddComponent<TextMeshProUGUI>();

        uiManager.virtuePoints = virtuePoints;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(uiManagerObject);
        Object.DestroyImmediate(virtuePoints.gameObject);
    }

    [Test]
    public void TestUpdateVirtuePointsText()
    {
        int testVP = 42;
        uiManager.UpdateVirtuePointsText(testVP);

        Assert.AreEqual($"Virtue Points: {testVP}", virtuePoints.text, "Virtue Points text should be updated correctly.");
    }
}
