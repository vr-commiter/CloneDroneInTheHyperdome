using Autohand;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using MyTrueGear;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VR.Enemies.EnemyAbilities.Subclasses;
using VR.HapticFeedback;
using VR.PowerGloves;
using VR.PowerGloves.UI;
using VR.PowerGloves.Upgrades.UpgradeUI;
using VR.UI;
using Weapons;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace CloneDroneInTheHyperdome_TrueGear
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        private static TrueGearMod _TrueGear = null;

        private static bool canPlayerFire = true;

        private static bool canTwoHandMeleeEnvironment = true;
        private static bool canLeftHandMeleeEnvironment = true;
        private static bool canRightHandMeleeEnvironment = true;

        private static bool canTwoHandMeleeWeapon = true;
        private static bool canLeftHandMeleeWeapon = true;
        private static bool canRightHandMeleeWeapon = true;

        private static bool canTwoHandMeleeImpact = true;
        private static bool canLeftHandMeleeImpact = true;
        private static bool canRightHandMeleeImpact = true;

        private static bool canLeftHandShock = false;
        private static bool canRightHandShock = false;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(Plugin));

            _TrueGear = new TrueGearMod();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRWeapon), "ProcessEnvironmentCollision")]
        private static void VRWeapon_ProcessEnvironmentCollision_Postfix(VRWeapon __instance,GameObject envObject, bool __result)
        {
            if (__result)
            {
                if (__instance._characterHoldingWeapon.IsMainPlayer())
                {
                    if (__instance.IsHeldByTwoHands())
                    {
                        if (!canTwoHandMeleeEnvironment)
                        {
                            return;
                        }
                        canTwoHandMeleeEnvironment = false;
                        new Timer(TwoHandMeleeEnvironmentTimerCallBack,null,80,Timeout.Infinite);
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("LeftHandMeleeHitEnvironment");
                        Logger.LogInfo("RightHandMeleeHitEnvironment");
                        _TrueGear.Play("LeftHandMeleeHitEnvironment");
                        _TrueGear.Play("RightHandMeleeHitEnvironment");
                    }
                    else if (__instance.IsHeldByLeftHand())
                    {
                        if (!canLeftHandMeleeEnvironment)
                        {
                            return;
                        }
                        canLeftHandMeleeEnvironment = false;
                        new Timer(LeftHandMeleeEnvironmentTimerCallBack, null, 80, Timeout.Infinite);
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("LeftHandMeleeHitEnvironment");
                        _TrueGear.Play("LeftHandMeleeHitEnvironment");
                    }
                    else if(!__instance.IsHeldByLeftHand())
                    {
                        if (!canRightHandMeleeEnvironment)
                        {
                            return;
                        }
                        canRightHandMeleeEnvironment = false;
                        new Timer(RightHandMeleeEnvironmentTimerCallBack, null, 80, Timeout.Infinite);
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("RightHandMeleeHitEnvironment");
                        _TrueGear.Play("RightHandMeleeHitEnvironment");
                    }
                }
            }         
        }

        private static void TwoHandMeleeEnvironmentTimerCallBack(object o)
        {
            canTwoHandMeleeEnvironment = true;
        }
        private static void LeftHandMeleeEnvironmentTimerCallBack(object o)
        {
            canLeftHandMeleeEnvironment = true;
        }
        private static void RightHandMeleeEnvironmentTimerCallBack(object o)
        {
            canRightHandMeleeEnvironment = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRWeapon), "ProcessWeapon")]
        private static void VRWeapon_ProcessWeapon_Postfix(VRWeapon __instance, UnityEngine.Collision collision)
        {
            Rigidbody rigidbody1 = collision.rigidbody;
            if (rigidbody1 == null || rigidbody1 == __instance.body)
            {
                return;
            }
            if (rigidbody1.gameObject.layer != __instance.gameObject.layer)
            {
                return;
            }
            VRWeapon componentInParent1 = rigidbody1.GetComponentInParent<VRWeapon>();
            if (componentInParent1 == null || componentInParent1 == __instance || !componentInParent1.HasCharacterHoldingWeapon())
            {
                return;
            }
            if (__instance._characterHoldingWeapon.IsMainPlayer())
            {
                
                if (__instance.IsHeldByTwoHands())
                {
                    if (!canTwoHandMeleeWeapon)
                    {
                        return;
                    }
                    canTwoHandMeleeWeapon = false;
                    new Timer(TwoHandMeleeWeaponTimerCallBack, null, 80, Timeout.Infinite);
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("LeftHandMeleeHitWeapon");
                    Logger.LogInfo("RightHandMeleeHitWeapon");
                    _TrueGear.Play("LeftHandMeleeHitWeapon");
                    _TrueGear.Play("RightHandMeleeHitWeapon");
                }
                else if (__instance.IsHeldByLeftHand())
                {
                    if (!canLeftHandMeleeWeapon)
                    {
                        return;
                    }
                    canLeftHandMeleeWeapon = false;
                    new Timer(LeftHandMeleeWeaponTimerCallBack, null, 80, Timeout.Infinite);
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("LeftHandMeleeHitWeapon");
                    _TrueGear.Play("LeftHandMeleeHitWeapon");
                }
                else if (!__instance.IsHeldByLeftHand())
                {
                    if (!canRightHandMeleeWeapon)
                    {
                        return;
                    }
                    canRightHandMeleeWeapon = false;
                    new Timer(RightHandMeleeWeaponTimerCallBack, null, 80, Timeout.Infinite);
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("RightHandMeleeHitWeapon");
                    _TrueGear.Play("RightHandMeleeHitWeapon");
                }
            }
        }

        private static void TwoHandMeleeWeaponTimerCallBack(object o)
        {
            canTwoHandMeleeWeapon = true;
        }
        private static void LeftHandMeleeWeaponTimerCallBack(object o)
        {
            canLeftHandMeleeWeapon = true;
        }
        private static void RightHandMeleeWeaponTimerCallBack(object o)
        {
            canRightHandMeleeWeapon = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRPlayerCharacter), "onWeaponGrabbed")]
        private static void VRPlayerCharacter_onWeaponGrabbed_Postfix(VRPlayerCharacter __instance, VRWeapon weapon)
        {
            if (weapon.IsHeldByLeftHand())
            {
                Logger.LogInfo("-------------------------------");
                Logger.LogInfo("LeftHandPickupItem");
                _TrueGear.Play("LeftHandPickupItem");
            }
            else
            {
                Logger.LogInfo("-------------------------------");
                Logger.LogInfo("RightHandPickupItem");
                _TrueGear.Play("RightHandPickupItem");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRPlayerCharacter), "Teleport")]
        private static void VRPlayerCharacter_Teleport_Postfix(VRPlayerCharacter __instance)
        {
            Logger.LogInfo("-------------------------------");
            Logger.LogInfo("Teleport");
            _TrueGear.Play("Teleport");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRPlayerCharacter), "OnBodyPartsDamaged")]
        private static void VRPlayerCharacter_OnBodyPartsDamaged_Postfix(VRPlayerCharacter __instance, CharacterDamageRecord damageRecord)
        {
            if (__instance.IsAlive())
            {
                Logger.LogInfo("-------------------------------");
                switch (damageRecord.damagedBodyPart.PartType)
                {
                    case MechBodyPartType.HandL:
                    case MechBodyPartType.LeftArm:
                    case MechBodyPartType.WristL:
                        Logger.LogInfo("LeftArmDamage");
                        _TrueGear.StopLeftHandLoadingPowerGlove();
                        _TrueGear.StopLeftHandFlamethrower();
                        _TrueGear.StopGrabString();
                        _TrueGear.Play("LeftArmDamage");
                        break;
                    case MechBodyPartType.HandR:
                    case MechBodyPartType.RightArm:
                    case MechBodyPartType.WristR:
                        Logger.LogInfo("RightArmDamage");
                        _TrueGear.StopRightHandLoadingPowerGlove();
                        _TrueGear.StopRightHandFlamethrower();
                        _TrueGear.StopGrabString();
                        _TrueGear.Play("RightArmDamage");
                        break;
                    case MechBodyPartType.LeftLeg:
                    case MechBodyPartType.FootL:
                    case MechBodyPartType.BackLegL:
                        Logger.LogInfo("LeftLegDamage");
                        _TrueGear.Play("LeftLegDamage");
                        break;
                    case MechBodyPartType.RightLeg:
                    case MechBodyPartType.FootR:
                    case MechBodyPartType.BackLegR:
                        Logger.LogInfo("RightLegDamage");
                        _TrueGear.Play("RightLegDamage");
                        break;
                    default:
                        Logger.LogInfo("TorsoDamage");
                        _TrueGear.Play("TorsoDamage");
                        break;
                }
                //Logger.LogInfo($"BodyPart :{damageRecord.damagedBodyPart.PartType}");
                //Logger.LogInfo($"impactDirection | X:{damageRecord.impactDirection.x} , Y:{damageRecord.impactDirection.y} , Z:{damageRecord.impactDirection.z}");
                //Logger.LogInfo($"impactPosition | X:{damageRecord.impactPosition.x} , Y:{damageRecord.impactPosition.y} , Z:{damageRecord.impactPosition.z}");
                //Logger.LogInfo($"transform | X:{__instance.transform.position.x} , Y:{__instance.transform.position.y} , Z:{__instance.transform.position.z}");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRPlayerCharacter), "onDeath")]
        private static void VRPlayerCharacter_onDeath_Postfix(VRPlayerCharacter __instance)
        {
            Logger.LogInfo("-------------------------------");
            Logger.LogInfo("PlayerDeath");
            _TrueGear.Play("PlayerDeath");
            _TrueGear.StopLeftHandLoadingPowerGlove();
            _TrueGear.StopRightHandLoadingPowerGlove();
            _TrueGear.StopLeftHandFlamethrower();
            _TrueGear.StopRightHandFlamethrower();
            _TrueGear.StopGrabString();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRBowWeapon), "OnStringGrabbed")]
        private static void VRBowWeapon_OnStringGrabbed_Postfix(VRBowWeapon __instance)
        {
            if (__instance._characterHoldingWeapon.IsMainPlayer())
            {
                Logger.LogInfo("-------------------------------");
                Logger.LogInfo("BowStringPull");
                _TrueGear.StartGrabString();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRBowWeapon), "OnStringReleased")]
        private static void VRBowWeapon_OnStringReleased_Postfix(VRBowWeapon __instance)
        {
            if (__instance._characterHoldingWeapon.IsMainPlayer())
            {
                _TrueGear.StopGrabString();
                if (__instance._isHeldByLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("LeftHandArrowShoot");
                    _TrueGear.Play("LeftHandArrowShoot");
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("RightHandArrowShoot");
                    _TrueGear.Play("RightHandArrowShoot");
                }
            }
        }



        [HarmonyPostfix, HarmonyPatch(typeof(VRHapticFeedbackManager), "PlayMeleeHitMeleeVibrationOnHand")]
        private static void VRHapticFeedbackManager_PlayMeleeHitMeleeVibrationOnHand_Postfix(VRHapticFeedbackManager __instance, Hand hand)
        {
            //Logger.LogInfo("-------------------------------");
            if (hand.left)
            {
                //Logger.LogInfo("LeftHandShock");
                canLeftHandShock = true;
            }
            else
            {
                //Logger.LogInfo("RightHandShock");
                canRightHandShock = true;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PowerGloveAttachPoint), "FinishPickingUp")]
        private static void PowerGloveAttachPoint_FinishPickingUp_Postfix(PowerGloveAttachPoint __instance)
        {
            if (__instance._hand.left)
            {
                Logger.LogInfo("-------------------------------");
                Logger.LogInfo("LeftHandEquipedPowerGlove");
                _TrueGear.StopLeftHandLoadingPowerGlove();
                _TrueGear.Play("LeftHandEquipedPowerGlove");
            }
            else
            {
                Logger.LogInfo("-------------------------------");
                Logger.LogInfo("RightHandEquipedPowerGlove");
                _TrueGear.StopRightHandLoadingPowerGlove();
                _TrueGear.Play("RightHandEquipedPowerGlove");
            }
        }


        [HarmonyPostfix, HarmonyPatch(typeof(ActivePeriodPowerGlove), "CanUse_Internal")]
        private static void ActivePeriodPowerGlove_CanUse_Internal_Postfix(ActivePeriodPowerGlove __instance, bool __result)
        {
            if (__instance._owner.IsMainPlayer())
            {
                if (__result)
                {
                    if (__instance._isLeftHand)
                    {
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("LeftHandUsedPowerGlove");
                        _TrueGear.Play("LeftHandUsedPowerGlove");
                    }
                    else
                    {
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("RightHandUsedPowerGlove");
                        _TrueGear.Play("RightHandUsedPowerGlove");
                    }
                }
            }
        }




        [HarmonyPostfix, HarmonyPatch(typeof(MeleeImpactArea), "OnImpactFX")]
        private static void MeleeImpactArea_OnImpactFX_Postfix(MeleeImpactArea __instance)
        {
            if (__instance._parentWeapon != null)
            {
                if (__instance._parentWeapon.HasCharacterHoldingWeapon())
                {
                    if (__instance._parentWeapon._characterHoldingWeapon.IsMainPlayer())
                    {
                        if (__instance._parentWeapon.IsHeldByTwoHands())
                        {
                            if (!canTwoHandMeleeImpact)
                            {
                                return;
                            }
                            canTwoHandMeleeImpact = false;
                            new Timer(TwoHandMeleeImpactnTimerCallBack, null, 190, Timeout.Infinite);
                            Logger.LogInfo("-------------------------------");
                            Logger.LogInfo("LeftHandMeleeImpact");
                            Logger.LogInfo("RightHandMeleeImpact");
                            _TrueGear.Play("LeftHandMeleeImpact");
                            _TrueGear.Play("RightHandMeleeImpact");
                        }
                        else if (__instance._parentWeapon.IsHeldByLeftHand())
                        {
                            if (!canLeftHandMeleeImpact)
                            {
                                return;
                            }
                            canLeftHandMeleeImpact = false;
                            new Timer(LeftHandMeleeImpactTimerCallBack, null, 190, Timeout.Infinite);
                            Logger.LogInfo("-------------------------------");
                            Logger.LogInfo("LeftHandMeleeImpact");
                            _TrueGear.Play("LeftHandMeleeImpact");
                        }
                        else
                        {
                            if (!canRightHandMeleeImpact)
                            {
                                return;
                            }
                            canRightHandMeleeImpact = false;
                            new Timer(RightHandMeleeImpactTimerCallBack, null, 190, Timeout.Infinite);
                            Logger.LogInfo("-------------------------------");
                            Logger.LogInfo("RightHandMeleeImpact");
                            _TrueGear.Play("RightHandMeleeImpact");
                        }
                    }
                }
            }                      
        }

        private static void TwoHandMeleeImpactnTimerCallBack(object o)
        {
            canTwoHandMeleeImpact = true;
        }
        private static void LeftHandMeleeImpactTimerCallBack(object o)
        {
            canLeftHandMeleeImpact = true;
        }
        private static void RightHandMeleeImpactTimerCallBack(object o)
        {
            canRightHandMeleeImpact = true;
        }


        [HarmonyPrefix, HarmonyPatch(typeof(HandProximityPanel), "SetHandInRange")]
        private static void HandProximityPanel_SetHandInRange_Prefix(HandProximityPanel __instance, bool isInRange)
        {
            if (isInRange)
            {
                if (__instance._isLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StartLeftHandLoadingPowerGlove");
                    _TrueGear.StartLeftHandLoadingPowerGlove();
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StartRightHandLoadingPowerGlove");
                    _TrueGear.StartRightHandLoadingPowerGlove();
                }
            }
            else
            {
                if (__instance._isLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StopLeftHandLoadingPowerGlove");
                    _TrueGear.StopLeftHandLoadingPowerGlove();
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StopRightHandLoadingPowerGlove");
                    _TrueGear.StopRightHandLoadingPowerGlove();
                }
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(VRPlayerCharacter), "OnArmorImpact")]
        private static void VRPlayerCharacter_OnArmorImpact_Prefix(VRPlayerCharacter __instance)
        {
            Logger.LogInfo("-------------------------------");
            Logger.LogInfo("ArmorImpact");
            _TrueGear.Play("ArmorImpact");
        }


        [HarmonyPrefix, HarmonyPatch(typeof(VRPlayerCharacter), "OnDamagedFromBeingOnFire")]
        private static void VRPlayerCharacter_OnDamagedFromBeingOnFire_Prefix(VRPlayerCharacter __instance)
        {
            if (canPlayerFire)
            {
                canPlayerFire = false;
                Logger.LogInfo("-------------------------------");
                Logger.LogInfo("PlayerBurn");
                _TrueGear.Play("PlayerBurn");
                new Timer(PlayerBrunTimerCallBack,null,150,Timeout.Infinite);
            }    
        }
        private static void PlayerBrunTimerCallBack(object o)
        {
            canPlayerFire = true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ShurikenThrowerGlove), "CanUse_Internal")]
        private static void ShurikenThrowerGlove_CanUse_Internal_Prefix(ShurikenThrowerGlove __instance,bool __result)
        {
            if (__instance._owner.IsMainPlayer())
            {
                if (__result)
                {
                    if (__instance._isLeftHand)
                    {
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("LeftHandUsedPowerGlove");
                        _TrueGear.Play("LeftHandUsedPowerGlove");
                    }
                    else
                    {
                        Logger.LogInfo("-------------------------------");
                        Logger.LogInfo("RightHandUsedPowerGlove");
                        _TrueGear.Play("RightHandUsedPowerGlove");
                    }
                }
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ShurikenThrowerGlove), "onFire")]
        private static void ShurikenThrowerGlove_onFire_Prefix(ShurikenThrowerGlove __instance)
        {
            if (__instance._shurikenProjectile == null)
            {
                return;
            }
            if (__instance._owner.IsMainPlayer())
            {
                if (__instance._isLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("LeftHandShurikenThrow");
                    _TrueGear.Play("LeftHandShurikenThrow");
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("RightHandShurikenThrow");
                    _TrueGear.Play("RightHandShurikenThrow");
                }
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(FlamethrowerGlove), "startActivePeriod")]
        private static void FlamethrowerGlove_startActivePeriod_Prefix(FlamethrowerGlove __instance)
        {
            if (__instance._owner.IsMainPlayer())
            {
                if (__instance._isLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StartLeftHandFlamethrower");
                    _TrueGear.StartLeftHandFlamethrower();
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StartRightHandFlamethrower");
                    _TrueGear.StartRightHandFlamethrower();
                }
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(FlamethrowerGlove), "stopActivePeriod")]
        private static void FlamethrowerGlove_stopActivePeriod_Prefix(FlamethrowerGlove __instance)
        {
            if (__instance._owner.IsMainPlayer())
            {
                if (__instance._isLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StopLeftHandFlamethrower");
                    _TrueGear.StopLeftHandFlamethrower();
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("StopRightHandFlamethrower");
                    _TrueGear.StopRightHandFlamethrower();
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(JetpackGlove), "setIsJetpacking")]
        private static void JetpackGlove_setIsJetpacking_Prefix(JetpackGlove __instance, bool value)
        {
            Logger.LogInfo("-------------------------------");
            Logger.LogInfo("setIsJetpacking");
            Logger.LogInfo(__instance._owner.IsMainPlayer());
            Logger.LogInfo(value);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MagnetPulseGlove), "playFireSequence")]
        private static void MagnetPulseGlove_playFireSequence_Prefix(MagnetPulseGlove __instance)
        {
            if (__instance.cooldown.IsActive)
            {
                return;
            }
            if (__instance._owner == null)
            {
                return;
            }
            if (__instance._owner.IsMainPlayer())
            {
                if (__instance._isLeftHand)
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("LeftHandUsedPowerGlove");
                    _TrueGear.Play("LeftHandUsedPowerGlove");
                }
                else
                {
                    Logger.LogInfo("-------------------------------");
                    Logger.LogInfo("RightHandUsedPowerGlove");
                    _TrueGear.Play("RightHandUsedPowerGlove");
                }
            }
        }

        //[HarmonyPostfix, HarmonyPatch(typeof(MagnetPulseGlove), "spawnShrapnelFragment")]
        //private static void MagnetPulseGlove_spawnShrapnelFragment_Prefix(MagnetPulseGlove __instance)
        //{
        //    Logger.LogInfo("-------------------------------");
        //    Logger.LogInfo("spawnShrapnelFragment");
        //    Logger.LogInfo(__instance._owner.IsMainPlayer());
        //    Logger.LogInfo(__instance._isLeftHand);
        //}



    }
}
