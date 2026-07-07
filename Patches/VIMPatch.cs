using GorillaTagScripts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elixir.Patches
{
    public class VIMPatch
    {
        public static bool enabled;

        [HarmonyPatch(typeof(SubscriptionManager), nameof(SubscriptionManager.LocalSubscriptionStatus))]
        public class LocalSubscriptionStatus
        {
            private static bool Prefix(ref SubscriptionManager.SubscriptionStatus __result)
            {
                if (!enabled)
                    return true;

                __result = SubscriptionManager.SubscriptionStatus.Active;
                return false;
            }
        }

        [HarmonyPatch(typeof(SubscriptionManager), nameof(SubscriptionManager.IsLocalSubscribed))]
        public class IsLocalSubscribed
        {
            private static bool Prefix(ref bool __result)
            {
                if (!enabled)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(SubscriptionManager), nameof(SubscriptionManager.LocalSubscriptionDetails))]
        public class LocalSubscriptionDetails
        {
            private static bool Prefix(ref SubscriptionManager.SubscriptionDetails __result)
            {
                if (!enabled)
                    return true;

                __result = new SubscriptionManager.SubscriptionDetails
                {
                    daysAccrued = int.MaxValue,
                    active = true,
                    subscriptionFeatureSettings = new[] { true, true },
                    tier = int.MaxValue,
                    autoRenew = true,
                    subscriptionActiveUntilDate = DateTime.MaxValue,
                    autoRenewMonths = int.MaxValue
                };
                return false;
            }
        }
    }
}
