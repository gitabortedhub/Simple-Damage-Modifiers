using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace Simple_Damage_Modifiers
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        internal static ConfigEntry<bool> FixArms { get; set; }
        internal static ConfigEntry<float> DamageModifierGeneral { get; set; }
        internal static ConfigEntry<float> DamageModifierBallistic { get; set; }
        internal static ConfigEntry<float> DamageModifierHead { get; set; }
        internal static ConfigEntry<float> DamageModifierExplosive { get; set; }
        internal static ConfigEntry<float> DamageModifierDurability { get; set; }

        private void Awake()
        {
            FixArms = Config.Bind<bool>("General",
                                         "Fix Arms Decreasing Damage",
                                         true,
                                         "Prevents arms from protecting the thorax from bullet damage");
            DamageModifierGeneral = Config.Bind<float>("General",
                                         "Global Damage Modifier",
                                         1f,
                                         "All damage sources");
            DamageModifierBallistic = Config.Bind<float>("General",
                                         "Ballistic Damage Modifier",
                                         1f,
                                         "Bullet damage sources");
            DamageModifierHead = Config.Bind<float>("General",
                                         "Ballistic Damage Modifier (Head)",
                                         1f,
                                         "Extra ballistic damage modifier applied to head");
            DamageModifierExplosive = Config.Bind<float>("General",
                                         "Explosive Damage Modifier",
                                         1f,
                                         "Explosive damage sources");
            DamageModifierDurability = Config.Bind<float>("General",
                                         "Armor Durability Damage Modifier",
                                         1f,
                                         "Durability damage received by armor");
            // Plugin startup logic
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            new InitColliderPatch().Enable();
            new ApplyDamagePatch().Enable();
            new ApplyArmorDamagePatch().Enable();
        }
    }
}
