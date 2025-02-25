using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace HideAmmoCounter
{
    [DrawFields(DrawFieldMask.Public)]
    public class WeaponsExceptions
    {
        public bool crossbow = true;
        public bool grenadeLauncher = true;
        public bool rocketLauncher = true;
        public bool ballisticKnife = true;
        public bool revolvers = false;
        public bool dualBarrelShotguns = false;
    }
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Hide Ammo Counter")] public bool hideAmmoCounter = true;
        [Draw("Hide Total Ammo Counter")] public bool hideTotalAmmoCounter = true;
        [Draw("Show Ammo in inventory overlay panel")] public bool showAmmoInDescription = true;
        [Draw("Remove the 'EMPTY' text")] public bool hideEmptyText = true;
        [Draw("Weapons exceptions", Collapsible = true)] public WeaponsExceptions WeaponsExceptions = new WeaponsExceptions();


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
            Main.changePanelStates(ammoCounter.panel);

        }
    }
}
