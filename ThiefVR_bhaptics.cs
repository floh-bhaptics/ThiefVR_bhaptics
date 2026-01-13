using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using Il2CppMazeTheory.Shadow;
using Il2CppHurricaneVR.Framework.Core;
using Il2CppHurricaneVR.Framework.Core.Grabbers;

[assembly: MelonInfo(typeof(ThiefVR_bhaptics.ThiefVR_bhaptics), "ThiefVR_bhaptics", "1.0.0", "Florian Fahrenberger")]
[assembly: MelonGame("MazeTheory", "Shadow")]

namespace ThiefVR_bhaptics
{
    public class ThiefVR_bhaptics: MelonMod
    {
        public static TactsuitVR tactsuitVr = null!;
        public static bool isRightHanded = true;

        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        /*
        [HarmonyPatch(typeof(PlayerEquipmentController), "ToggleBlackjack", new Type[] { typeof(bool) })]
        public class bhaptics_Blackjack
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerEquipmentController __instance, bool isActive)
            {
                tactsuitVr.PlaybackHaptics("HipHolster");
            }
        }
        */

        [HarmonyPatch(typeof(BlackjackGrabbedEvent), "BlackjackGrabbed", new Type[] { typeof(HVRGrabberBase), typeof(HVRGrabbable) })]
        public class bhaptics_BlackjackGrabbed
        {
            [HarmonyPostfix]
            public static void Postfix(BlackjackGrabbedEvent __instance, HVRGrabberBase arg0)
            {
                tactsuitVr.PlaybackHaptics("HipHolster");
            }
        }

        [HarmonyPatch(typeof(MTBowController), "OnBowGrabbed", new Type[] { typeof(HVRHandGrabber), typeof(HVRGrabbable) })]
        public class bhaptics_Bow
        {
            [HarmonyPostfix]
            public static void Postfix(MTBowController __instance, HVRHandGrabber hand)
            {
                if (hand.IsLeftHand) tactsuitVr.PlaybackHaptics("ShoulderHolster_L");
                else tactsuitVr.PlaybackHaptics("ShoulderHolster_R");
            }
        }
        
        [HarmonyPatch(typeof(PlayerHealthManager), "TakeDamage", new Type[] { typeof(PlayerHealthManager.PlayerDamageType), typeof(float) })]
        public class bhaptics_Damage
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHealthManager __instance, PlayerHealthManager.PlayerDamageType type, float amount)
            {
                if (__instance._healthData.currentHealth < 0.25f * __instance._healthData.maxHealth) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
                if (type == PlayerHealthManager.PlayerDamageType.Fall) tactsuitVr.PlaybackHaptics("FallDamage");
                else tactsuitVr.PlaybackHaptics("Impact");
            }
        }

        [HarmonyPatch(typeof(FallDamageController), "HandleFallSfx", new Type[] { typeof(float), typeof(float) })]
        public class bhaptics_Fall
        {
            [HarmonyPostfix]
            public static void Postfix(FallDamageController __instance, float distance, float normalisedDistance)
            {
                if (distance < 0.3f) return;
                if (distance < __instance.minFallDamageDistance) tactsuitVr.PlaybackHaptics("FallLight");
                else tactsuitVr.PlaybackHaptics("FallDamage");
            }
        }

        [HarmonyPatch(typeof(PlayerDeathController), "HandlePlayerDied", new Type[] {  })]
        public class bhaptics_PlayerDied
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerDeathController __instance)
            {
                tactsuitVr.StopThreads();
                tactsuitVr.PlaybackHaptics("PlayerDeath");
            }
        }

        [HarmonyPatch(typeof(Food), "PlayBiteAudio", new Type[] { })]
        public class bhaptics_Eat
        {
            [HarmonyPostfix]
            public static void Postfix(Food __instance)
            {
                tactsuitVr.PlaybackHaptics("Eating");
                tactsuitVr.StopHeartBeat();
            }
        }

        [HarmonyPatch(typeof(MTBowController), "Fire", new Type[] { })]
        public class bhaptics_ShootBow
        {
            [HarmonyPostfix]
            public static void Postfix(MTBowController __instance)
            {
                bool leftHanded = __instance._heldLeft;
                tactsuitVr.ShootBow(!leftHanded);
            }
        }

        [HarmonyPatch(typeof(BirdWhistle), "Trigger", new Type[] { })]
        public class bhaptics_Whistle
        {
            [HarmonyPostfix]
            public static void Postfix(BirdWhistle __instance)
            {
                tactsuitVr.PlaybackHaptics("Whistle");
            }
        }

        [HarmonyPatch(typeof(LootManager), "CollectLoot", new Type[] { typeof(LootDatabase.LootData)})]
        public class bhaptics_CollectLoot
        {
            [HarmonyPostfix]
            public static void Postfix(LootManager __instance)
            {
                tactsuitVr.PlaybackHaptics("CollectLoot");
            }
        }

        [HarmonyPatch(typeof(ThiefVisionToggle), "ThiefVisionActivate", new Type[] { })]
        public class bhaptics_ThiefVisionActivate
        {
            [HarmonyPostfix]
            public static void Postfix(ThiefVisionToggle __instance)
            {
                tactsuitVr.PlaybackHaptics("ThiefVision");
            }
        }

        [HarmonyPatch(typeof(PlayStyleTracker), "PlayerDetected", new Type[] { })]
        public class bhaptics_PlayerDetected
        {
            [HarmonyPostfix]
            public static void Postfix(PlayStyleTracker __instance)
            {
                tactsuitVr.PlaybackHaptics("NeckTingle");
            }
        }

    }
}
