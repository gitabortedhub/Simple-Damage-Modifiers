using SPT.Reflection.Patching;
using System.Collections.Generic;
using System.Reflection;
using EFT.HealthSystem;
using EFT;
using EFT.InventoryLogic;

namespace Simple_Damage_Modifiers
{
    public class InitColliderPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(BodyPartCollider).GetMethod("InitColliderSettings", BindingFlags.Public | BindingFlags.Instance);
        }
        [PatchPostfix]
        private static void PatchPostfix(ref BodyPartCollider __instance)
        {
            if (!Plugin.FixArms.Value) return;
            if (__instance.BodyPartType == EBodyPart.LeftArm || __instance.BodyPartType == EBodyPart.RightArm)
            {
                __instance.penetrationDamageMod = 1f;
                __instance.PenetrationChance = 1f;
                __instance.PenetrationLevel = 0f;
            }
        }
    }
    public class ApplyArmorDamagePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(ArmorComponent).GetMethod("ApplyDurabilityDamage", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static void Prefix(ref ArmorComponent __instance, ref float armorDamage, List<ArmorComponent> armorComponents)
        {
            armorDamage *= Plugin.DamageModifierDurability.Value;
        }
    }
    public class ApplyDamagePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(ActiveHealthController).GetMethod("ApplyDamage", BindingFlags.Public | BindingFlags.Instance);
        }
        [PatchPrefix]
        private static void PatchPrefix(ref ActiveHealthController __instance, EBodyPart bodyPart, ref float damage, DamageInfoStruct damageInfo)
        {
            damage *= Plugin.DamageModifierGeneral.Value;
            if (damageInfo.DamageType == EDamageType.Bullet)
            {
                damage *= Plugin.DamageModifierBallistic.Value;
                if (bodyPart == EBodyPart.Head)
                {
                    damage *= Plugin.DamageModifierHead.Value;
                }
            }
            if (damageInfo.DamageType == EDamageType.Explosion)
            {
                damage *= Plugin.DamageModifierExplosive.Value;
            }
        }
    }
}
