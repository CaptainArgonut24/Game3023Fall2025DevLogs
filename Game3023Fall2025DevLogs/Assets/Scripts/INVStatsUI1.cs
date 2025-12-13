using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using TMPro;

using UnityEngine;
using TMPro;

public class INVStatsUI : MonoBehaviour
{
    [Header("Inventory UI Text References")]
    public TextMeshProUGUI teleporterText;
    public TextMeshProUGUI beefUpText;
    public TextMeshProUGUI starText;
    public TextMeshProUGUI disguiseBagText;
    public TextMeshProUGUI shovelText;
    public TextMeshProUGUI toppatDiamondText;
    public TextMeshProUGUI nrgDrinkText;
    public TextMeshProUGUI cheseText;
    public TextMeshProUGUI rubiksCubeText;
    public TextMeshProUGUI gatlingGunText;
    public TextMeshProUGUI disguiseText;
    public TextMeshProUGUI stickyHandText;
    public TextMeshProUGUI bananaPeelText;
    public TextMeshProUGUI laserCutterText;
    public TextMeshProUGUI wormholeRifleText;
    public TextMeshProUGUI poisonDartGunText;
    public TextMeshProUGUI theForceText;
    public TextMeshProUGUI cakeText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI CheseText;

    private GameData data;

    private void Start()
    {
        data = GameData.Instance;

        if (data == null)
        {
            Debug.LogError("GameData Instance not found!");
        }
    }

    private void Update()
    {
        if (data == null) return;

        teleporterText.text = $"Teleporter: {data.Teleporter}";
        beefUpText.text = $"BEEF UP: {data.BEEFUP}";
        starText.text = $"Star: {data.Star}";
        disguiseBagText.text = $"Disguise Bag: {data.DisguiseBag}";
        shovelText.text = $"Shovel: {data.Shovel}";
        toppatDiamondText.text = $"Toppat Diamond: {data.ToppatDiamond}";
        nrgDrinkText.text = $"NRG Drink: {data.NRGDrink}";
        cheseText.text = $"Cheese: {data.Chese}";
        rubiksCubeText.text = $"Rubik's Cube: {data.RubiksCube}";
        gatlingGunText.text = $"Gatling Gun: {data.GatlingGun}";
        disguiseText.text = $"Disguise: {data.Disguise}";
        stickyHandText.text = $"Sticky Hand: {data.StickyHand}";
        bananaPeelText.text = $"Banana Peel: {data.BananaPeel}";
        laserCutterText.text = $"Laser Cutter: {data.LaserCutter}";
        wormholeRifleText.text = $"Wormhole Rifle: {data.WormholeRifle}";
        poisonDartGunText.text = $"Poison Dart Gun: {data.PoisonDartGun}";
        theForceText.text = $"The Force: {data.TheForce}";
        cakeText.text = $"Cake: {data.Cake}";
        goldText.text = $"Gold: {data.Gold}";
        CheseText.text = $"Chese: {data.Chese}";
    }
}
