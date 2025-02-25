using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAmmoCounter
{
    [HarmonyPatch(typeof(HUDPanel))]
    [HarmonyPatch("OnUpdateMagAmmo")]
    public class ammoCounter
    {
        public static HUDPanel panel;
        static void Postfix(HUDPanel __instance)
        {
            if (__instance != null)
            {
                Main.ModEntry.Logger.Log(__instance.name);
                panel = __instance;
                Main.changePanelStates(__instance);
            }
        }
    }
    
    [HarmonyPatch(typeof(ItemInfoPanel))]
    [HarmonyPatch("ShowItemInfo")]
    public class inventory
    {
        private static string check(bool settingState, ItemInfoPanel panel, GridItem item)
        {
            string text = "";
            if (!settingState)
            {
                panel.ItemAttributeNames.text += "\n" + GameManager.Inst.UIManager.GetTranslatedMessage("Ammo");
                panel.ItemAttributeValues.text += "\n" + item.Item.GetAttributeByName("_LoadedAmmos").Value.ToString();
            }

            return text;
        }
        public static ItemInfoPanel panel;
        static void Postfix(ItemInfoPanel __instance, GridItem item)
        {
            if (__instance != null && panel == null && //item.Item.Type == ItemType.PrimaryWeapon || item.Item.Type == ItemType.SideArm
                (bool)item.Item.GetAttributeByName("_IsRanged").Value
                && Main.settings.showAmmoInDescription)
            {
                switch (item.Item.ID)
                {
                    case "crossbow": check(Main.settings.WeaponsExceptions.crossbow, __instance, item); break;
                    case "m79": check(Main.settings.WeaponsExceptions.grenadeLauncher, __instance, item); break;
                    case "rpg": check(Main.settings.WeaponsExceptions.rocketLauncher, __instance, item); break;
                    case "doublebarrelshotgun": check(Main.settings.WeaponsExceptions.dualBarrelShotguns, __instance, item); break;
                    case "44magnum": check(Main.settings.WeaponsExceptions.revolvers, __instance, item); break;
                    case "44magnumsilenced": check(Main.settings.WeaponsExceptions.revolvers, __instance, item); break;
                    case "nagantrevolver": check(Main.settings.WeaponsExceptions.revolvers, __instance, item); break;
                    case "ballisticknife": check(Main.settings.WeaponsExceptions.ballisticKnife, __instance, item); break;
                    default:
                        __instance.ItemAttributeNames.text += "\n" + GameManager.Inst.UIManager.GetTranslatedMessage("Ammo");
                        __instance.ItemAttributeValues.text += "\n" + item.Item.GetAttributeByName("_LoadedAmmos").Value.ToString();
                        break;
                }
            }
        }
    }
}
