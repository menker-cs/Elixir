using System.Collections.Generic;
using static Elixir.Management.Menu;
using static Elixir.Utilities.Variables;
using static Elixir.Mods.Categories.Room;
using static Elixir.Mods.Categories.Playerr;
using Elixir.Management;

namespace Elixir.Utilities
{
    internal class ButtonManager
    {
        private static VRRig targetedPlayer = null;
        static Module[] GetPlayers()
        {
            var mods = new List<Module>();
            var modsHash = new HashSet<string>();

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == GorillaTagger.Instance.offlineVRRig)
                    continue;

                string modName = vrrig.OwningNetPlayer.NickName;
                if (modsHash.Contains(modName))
                    continue;

                modsHash.Add(modName);

                var rig = vrrig;

                mods.Add(new Module()
                {
                    title = rig.OwningNetPlayer.NickName,
                    tooltip = "Click to view player options",
                    isToggleable = false,
                    toggled = false,
                    action = () =>
                    {
                        targetedPlayer = rig;
                    },
                });
            }

            if (mods.Count == 0)
            {
                mods.Add(new Module()
                {
                    title = "Not Connected To A Room",
                    tooltip = "",
                    isToggleable = false,
                    action = () => { }
                });
            }
            return mods.ToArray();
        }

        static Module[] CreatePlayerButtons(VRRig rig)
        {
            var mods = new List<Module>();
            var modsHash = new HashSet<string>();
            string modName = rig.OwningNetPlayer.NickName;
            if (!modsHash.Contains(modName))
            {
                modsHash.Add(modName);

                mods.Add(new Module()
                {
                    title = "< Back To Players",
                    tooltip = "Return to the players list",
                    isToggleable = false,
                    action = () =>
                    {
                        targetedPlayer = null;
                    },
                });

                mods.Add(new Module()
                {
                    title = "Targeted Player: " + rig.OwningNetPlayer.NickName,
                    tooltip = "",
                    isToggleable = false,
                    action = () => { },
                });

                mods.Add(new Module()
                {
                    title = "Is Master: " + rig.OwningNetPlayer.IsMasterClient,
                    tooltip = "",
                    isToggleable = false,
                    action = () => { },
                });

                mods.Add(new Module()
                {
                    title = "Tag " + rig.OwningNetPlayer.NickName,
                    tooltip = "Currently Tagged: " + RigIsInfected(rig),
                    isToggleable = false,
                    action = () => { TagPlayer(rig); },
                });

                mods.Add(new Module()
                {
                    title = "Mute " + rig.OwningNetPlayer.NickName,
                    tooltip = "Mute this player",
                    isToggleable = false,
                    action = () => { MutePlayer(rig); },
                });

                mods.Add(new Module()
                {
                    title = "Report " + rig.OwningNetPlayer.NickName,
                    tooltip = "Report this player",
                    isToggleable = false,
                    action = () => { ReportPlayer(rig); },
                });
            }
            return mods.ToArray();
        }

        static Module[] GetEnabledMods()
        {
            var mods = new List<Module>();
            var modsHash = new HashSet<string>();

            foreach (var category in categories)
            {
                if (category == null || category.buttons == null || category.name == "Enabled Mods") continue;

                foreach (var mod in category.buttons)
                {
                    if (mod == null || !mod.isToggleable || !mod.toggled) continue;

                    string modName = mod.title;

                    if (!modsHash.Contains(modName))
                    {
                        modsHash.Add(modName);

                        var ogMod = mod;
                        var ogCategory = category.name;

                        mods.Add(new Module()
                        {
                            title = mod.title,
                            tooltip = $"Located in [{ogCategory}]",
                            isToggleable = true,
                            toggled = true,
                            action = () => { ogMod.toggled = true; ogMod.action?.Invoke(); },
                            disableAction = () => { ogMod.toggled = false; ogMod.disableAction?.Invoke(); }
                        });
                    }
                }
            }

            if (mods.Count == 0)
            {
                mods.Add(new Module()
                {
                    title = "No Mods Enabled",
                    tooltip = "",
                    isToggleable = false,
                    action = () => { }
                });
            }

            return mods.ToArray();
        }

        public static Module GetButton(string title)
        {
            foreach (var category in categories)
            {
                foreach (var button in category.buttons)
                {
                    if (button.title.Contains(title))
                    {
                        return button;
                    }
                }
            }
            return null;
        }

        public static Module GetAllButtons()
        {
            foreach (var category in categories)
            {
                foreach (var button in category.buttons)
                {
                    return button;
                }
            }
            return null;
        }

        public static void RefreshCategory()
        {
            var enabledMods = categories.Find(c => c.name == "Enabled Mods");
            var players = categories.Find(c => c.name == "Players");

            if (enabledMods != null)
            {
                enabledMods.buttons = GetEnabledMods();
            }
            if (players != null)
            {
                if (targetedPlayer != null)
                {
                    players.buttons = CreatePlayerButtons(targetedPlayer);
                }
                else
                {
                    players.buttons = GetPlayers();
                }
            }
        }

        public static void DisableAllMods()
        {
            foreach (var category in categories)
            {
                foreach (var mod in category.buttons)
                {
                    if (mod == null || !mod.isToggleable || !mod.toggled) continue;

                    mod.toggled = false;
                    mod.disableAction?.Invoke();
                }
            }
        }

    }
}