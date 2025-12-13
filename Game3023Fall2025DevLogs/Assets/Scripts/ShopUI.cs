using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Settings")]
    public int itemCost = 100;
    public float errorFlashTime = 3f;

    [Header("Button Colors")]
    public Color canBuyColor = Color.green;
    public Color cantBuyColor = Color.red;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip errorSound;
    public AudioClip successSound;

    private GameData data;

    private void Start()
    {
        data = GameData.Instance;

        if (data == null)
        {
            Debug.LogError("GameData Instance not found!");
        }
    }

    // ==========================
    // CORE BUY LOGIC
    // ==========================
    private void TryBuy(System.Action addItemAction, Button button)
    {
        if (data.Gold >= itemCost)
        {
            data.Gold -= itemCost;
            addItemAction.Invoke();

            if (audioSource && successSound)
                audioSource.PlayOneShot(successSound);
        }
        else
        {
            StartCoroutine(ShowError(button));
        }
    }

    private IEnumerator ShowError(Button button)
    {
        if (audioSource && errorSound)
            audioSource.PlayOneShot(errorSound);

        Image img = button.GetComponent<Image>();
        Color originalColor = img.color;

        img.color = cantBuyColor;
        yield return new WaitForSeconds(errorFlashTime);
        img.color = originalColor;
    }

    // ==========================
    // SHOP BUTTONS
    // ==========================

    public void BuyTeleporter(Button button)
    {
        TryBuy(() => data.Teleporter += 1, button);
    }

    public void BuyBeefUp(Button button)
    {
        TryBuy(() => data.BEEFUP += 1, button);
    }

    public void BuyStar(Button button)
    {
        TryBuy(() => data.Star += 1, button);
    }

    public void BuyShovel(Button button)
    {
        TryBuy(() => data.Shovel += 1, button);
    }

    public void BuyLaserCutter(Button button)
    {
        TryBuy(() => data.LaserCutter += 1, button);
    }

    public void BuyWormholeRifle(Button button)
    {
        TryBuy(() => data.WormholeRifle += 1, button);
    }
    

    public void BuyBEEFUP(Button btn)
        => TryBuy(() => data.BEEFUP++, btn);


    public void BuyDisguiseBag(Button btn)
        => TryBuy(() => data.DisguiseBag++, btn);


    public void BuyToppatDiamond(Button btn)
        => TryBuy(() => data.ToppatDiamond++, btn);

    public void BuyNRGDrink(Button btn)
        => TryBuy(() => data.NRGDrink++, btn);

    public void BuyChese(Button btn)
        => TryBuy(() => data.Chese++, btn);

    public void BuyRubiksCube(Button btn)
        => TryBuy(() => data.RubiksCube++, btn);

    public void BuyGatlingGun(Button btn)
        => TryBuy(() => data.GatlingGun++, btn);

    public void BuyDisguise(Button btn)
        => TryBuy(() => data.Disguise++, btn);

    public void BuyStickyHand(Button btn)
        => TryBuy(() => data.StickyHand++, btn);

    public void BuyBananaPeel(Button btn)
        => TryBuy(() => data.BananaPeel++, btn);

    
    public void BuyPoisonDartGun(Button btn)
        => TryBuy(() => data.PoisonDartGun++, btn);

    public void BuyTheForce(Button btn)
        => TryBuy(() => data.TheForce++, btn);

    public void BuyCake(Button btn)
        => TryBuy(() => data.Cake++, btn);

    // ==========================
    // DEBUG / CHEAT BUTTON
    // ==========================
    public void AddGold100()
    {
        data.Gold += 100;
    }
}
