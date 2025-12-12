using UnityEngine;
using System.Collections.Generic;

public enum ItemName
{
    Teleporter,
    BEEFUP,
    Star,
    DisguiseBag,
    Shovel,
    ToppatDiamond,
    NRGDrink,
    Chese,
    RubiksCube,
    GatlingGun,
    Disguise,
    StickyHand,
    BananaPeel,
    LaserCutter,
    WormholeRifle,
    PoisonDartGun,
    TheForce,
    ShrinkRay,
    Cake
}

[CreateAssetMenu(fileName = "NewItem", menuName = "HenryStickmin/Item")]
public class ItemData : ScriptableObject
{
    public Sprite image;

    [Header("Item Settings")]
    public ItemName itemName;

    [TextArea(3, 6)]
    public string itemDescription;

    public int amount = 1;

    [Range(0, 100)]
    public int chanceOfWorking = 100;

    private void OnValidate()
    {
        if (descriptionDictionary.ContainsKey(itemName))
        {
            itemDescription = descriptionDictionary[itemName];
        }
    }

   
    private static readonly Dictionary<ItemName, string> descriptionDictionary =
        new Dictionary<ItemName, string>()
    {
        { ItemName.Teleporter, "A highly experimental teleporter. It usually *kinda* works… sometimes." },
        { ItemName.BEEFUP, "Become buff instantly. Warning: results may vary wildly." },
        { ItemName.Star, "Summons a powerful ally… or an angry plumber." },
        { ItemName.DisguiseBag, "Classic trick. Hide in a bag and hope nobody checks." },
        { ItemName.Shovel, "When in doubt, dig. Works surprisingly often." },
        { ItemName.ToppatDiamond, "A priceless diamond… if you survive taking it." },
        { ItemName.NRGDrink, "Gives extreme speed. Time may or may not freeze." },
        { ItemName.Chese, "Cheese. Could be useful? Maybe?" },
        { ItemName.RubiksCube, "A puzzle cube. Solving it might unlock… something?" },
        { ItemName.GatlingGun, "Fires very fast. Sometimes too fast." },
        { ItemName.Disguise, "Blend into your surroundings. Results depend on acting skill." },
        { ItemName.StickyHand, "A stretchy hand used for grabbing things. Accuracy questionable." },
        { ItemName.BananaPeel, "Classic slip trick. Works 100% of the time… on *you*." },
        { ItemName.LaserCutter, "Cuts through almost anything… except thick plot armor." },
        { ItemName.WormholeRifle, "Shoots portals. Definitely not stolen tech." },
        { ItemName.PoisonDartGun, "A silent takedown tool. Watch the wind direction." },
        { ItemName.TheForce, "Use the mystical power. Totally legit and not copyrighted." },
        { ItemName.ShrinkRay, "Shrinks targets. Just avoid shrinking *yourself*." },
        { ItemName.Cake, "Sometimes hides tools inside. Sometimes just cake." },
    };
}
