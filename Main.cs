using HarmonyLib;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityModManagerNet;
using UnityStandardAssets.CinematicEffects;
using static System.Net.Mime.MediaTypeNames;
using static UnityModManagerNet.UnityModManager;

// TotalAmmoCount option
// option to show ammo count or not

namespace HideAmmoCounter
{
    
    public static class Main
    {
        public static Settings settings;
        public static UnityModManager.ModEntry ModEntry;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = Settings.Load<Settings>(modEntry);
            settings.OnChange();
            ModEntry = modEntry;
            // Create a Harmony instance
            var harmony = new Harmony("com.example.harmonypatch");
            // Apply the patch
            harmony.PatchAll();
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            return true;
        }

        private static bool OnToggle(ModEntry entry, bool arg2)
        {
            ammoCounter.panel.MagAmmo.gameObject.SetActive(true);
            ammoCounter.panel.TotalAmmo.gameObject.SetActive(true);
            return arg2;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
            settings.OnChange();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        private static void exceptionCheck(bool settingsOn, HUDPanel panel)
        {
            if (settingsOn)
            {
                panel.MagAmmo.gameObject.SetActive(true);
            }
        }
        
        public static void changePanelStates(HUDPanel panel)
        {
            if (panel != null)
            {
                try
                {
                    string weaponID = GameManager.Inst.PlayerControl.SelectedPC.MyReference.CurrentWeapon.GetComponent<Weapon>().WeaponItem.ID;

                    if(weaponID != null) { 
                        switch (weaponID)
                        {
                            case "crossbow": exceptionCheck(settings.WeaponsExceptions.crossbow, panel); break;
                            case "m79": exceptionCheck(settings.WeaponsExceptions.grenadeLauncher, panel); break;
                            case "rpg": exceptionCheck(settings.WeaponsExceptions.rocketLauncher, panel); break;
                            case "doublebarrelshotgun": exceptionCheck(settings.WeaponsExceptions.dualBarrelShotguns, panel); break;
                            case "44magnum": exceptionCheck(settings.WeaponsExceptions.revolvers, panel); break;
                            case "44magnumsilenced": exceptionCheck(settings.WeaponsExceptions.revolvers, panel); break;
                            case "nagantrevolver": exceptionCheck(settings.WeaponsExceptions.revolvers, panel); break;
                            case "ballisticknife": exceptionCheck(settings.WeaponsExceptions.ballisticKnife, panel); break;
                            default:
                                panel.MagAmmo.gameObject.SetActive(!settings.hideAmmoCounter);
                                break;
                        }

                        panel.TotalAmmo.gameObject.SetActive(!settings.hideTotalAmmoCounter);

                        if (settings.hideEmptyText && panel.MagAmmo.text[0] != 'I')
                        {
                            panel.MagAmmo.text = "";
                        }
                    }
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}
