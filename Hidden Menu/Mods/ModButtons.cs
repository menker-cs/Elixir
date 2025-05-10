using static Hidden.Utilities.GunTemplate;
using static Hidden.Utilities.Variables;
using static Hidden.Utilities.ColorLib;
using static Hidden.Mods.Categories.Move;
using static Hidden.Mods.Categories.Playerr;
using static Hidden.Mods.Categories.Visuals;
using static Hidden.Mods.Categories.Room;
using static Hidden.Mods.Categories.Settings;
using static Hidden.Mods.Categories.Fun;
using static Hidden.Mods.Categories.World;
using static Hidden.Menu.ButtonHandler;
using static Hidden.Menu.Optimizations;
using static Hidden.Menu.Optimizations.ResourceLoader;
using static Hidden.Menu.Main;
using UnityEngine;
using Fusion;
using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using Hidden.Utilities;
using Hidden.Mods.Categories;
using static Hidden.Menu.ButtonHandler;
using Unity.Mathematics;

namespace Hidden.Mods
{
    public enum Category
    {
        // Starting Page
        Home,

        // Mod Categories
        Settings,
        Room,
        Player,
        Move,
        World,
        Fun,
        Visuals,
        Creds,
        CS,
    }
    public class ModButtons
    {
        public static Button[] buttons =
        {
            #region Starting Page
            new Button("Settings", Category.Home, false, false, ()=>ChangePage(Category.Settings)),
            new Button("Room", Category.Home, false, false, ()=>ChangePage(Category.Room)),
            new Button("Movement", Category.Home, false, false, ()=>ChangePage(Category.Move)),
            new Button("Player", Category.Home, false, false, ()=>ChangePage(Category.Player)),
            new Button("Visual", Category.Home, false, false, ()=>ChangePage(Category.Visuals)),
            new Button("Fun", Category.Home, false, false, ()=>ChangePage(Category.Fun)),
            new Button("World", Category.Home, false, false, ()=>ChangePage(Category.World)),
            new Button("Creds", Category.Home, false, false, ()=>ChangePage(Category.Creds)),
            #endregion

            #region Settings
            new Button("Disable All Mods", Category.Settings, false, false, ()=>DisableAllMods()),
            new Button("Right Handed Menu", Category.Settings, true, false, ()=>SwitchHands(true), ()=>SwitchHands(false)),
            new Button("Disconnect Button", Category.Settings, true, true, ()=>ToggleDisconnectButton(true), ()=>ToggleDisconnectButton(false)),
            new Button("Toggle Notifications", Category.Settings, true, true, ()=>ToggleNotifications(true), ()=>ToggleNotifications(false)),
            new Button("Clear Notifications", Category.Settings, false, false, ()=>ClearNotifications()),
            new Button("Bark Positioning", Category.Settings, true, false, ()=>Bark(true), ()=>Bark(false)),
            new Button("Menu Outline", Category.Settings, true, true, ()=>OLine(true), ()=>OLine(false)),
            new Button("Change Layout", Category.Settings, false, false, ()=> ChangeLayout()),
            new Button("Change Theme", Category.Settings, false, false, ()=> ChangeTheme()),
            new Button("Change Sound", Category.Settings, false, false, ()=> ChangeSound()),
            new Button("Change ESP Color", Category.Settings, false, false, ()=>ESPChange()),
            new Button("Refresh Menu", Category.Settings, false, false, ()=> RefreshMenu()),
            #endregion

            #region Room
            new Button("Quit Game", Category.Room, true, false, ()=>QuitGTAG()),
            new Button("Join Random", Category.Room, false, false, ()=>JoinRandomPublic()),
            new Button("Disconnect", Category.Room, false, false, ()=>Disconnect()),
            new Button("Primary Disconnect", Category.Room, true, false, ()=>PrimaryDisconnect()),
            new Button("Check If Master", Category.Room, false, false, ()=>IsMasterCheck()),
            new Button("Disable Network Triggers", Category.Room, false, false, ()=>DisableNetworkTriggers()),
            new Button("Enable Network Triggers", Category.Room, false, false, ()=>EnableNetworkTriggers()),
            new Button("Join Code Mods", Category.Room, false, false, ()=>JoinRoom("MODS")),
            new Button("Join Code HIDDENMENU", Category.Room, false, false, ()=>JoinRoom("HIDDENMENU")),
            new Button("Join Code PBBV", Category.Room, false, false, ()=>JoinRoom("PBBV")),
            new Button("Join Code Daisy09", Category.Room, false, false, ()=>JoinRoom("DAISY09")),
            new Button("Mute Everyone", Category.Room, false, false, ()=>MuteAll()),
            new Button("Report Everyone", Category.Room, false, false, ()=>ReportAll()),
            new Button("Copy Self ID", Category.Room, false, false, ()=> CopySelfID()),
            new Button("Copy ID Gun", Category.Room, true, false, ()=> CopyIDGun()),
            new Button("Anti Report", Category.Room, true, true, ()=>AntiReport()),
            new Button("Strict Anti Report", Category.Room, true, false, ()=>StrictAntiReport()),
            #endregion

            #region Movement
            new Button("Platforms [G]", Category.Move, true, false, ()=>Platforms()),
            new Button("Sticky Platforms [G]", Category.Move, true, false, ()=>StickyPlatforms()),
            new Button("Force Tag Freeze", Category.Move, false, false, ()=>TagFreeze()),
            new Button("No Tag Freeze", Category.Move, true, false, ()=>NoTagFreeze()),
            new Button("NoClip [T]", Category.Move, true, false, ()=>Noclip()),
            new Button("Speed Boost", Category.Move, true, false, ()=>Speedboost()),
            new Button("Velocity Jump", Category.Move, true, false, ()=>Velocity(-1f), ()=> Velocity(default)),
            new Button("Fly [P]", Category.Move, true, false, ()=>Fly()),
            new Button("Trigger Fly [T]", Category.Move, true, false, ()=>TriggerFly()),
            new Button("Hand Fly [P]", Category.Move, true, false, ()=>HandFly()),
            new Button("Slingshot Fly [T]", Category.Move, true, false, ()=>SlingshotFly()),
            new Button("Iron Monke [G]", Category.Move, true, false, ()=>IronMonkey()),
            new Button("Punch Mod [BUGGY]", Category.Move, true, false, ()=>SlaveWhipMod()),
            new Button("Car Monke [T]", Category.Move, true, false, ()=>carmonkey()),
            new Button("Up & Down [T]", Category.Move, true, false, ()=>UpAndDown()),
            new Button("Shit Hertz", Category.Move, false, false, ()=>Hertz(10)),
            new Button("45 Hertz", Category.Move, false, false, ()=>Hertz(45)),
            new Button("60 Hertz", Category.Move, false, false, ()=>Hertz(60)),
            new Button("120 Hertz", Category.Move, false, false, ()=>Hertz(120)),
            new Button("Unlimit Hertz", Category.Move, false, false, ()=>Hertz(int.MaxValue)),
            new Button("Reset Hertz", Category.Move, false, false, ()=>ResetHz()),
            new Button("WASD [PC]", Category.Move, true, false, ()=>WASDFly()),
            new Button("No Gravity", Category.Move, true, false, ()=>Gravity(9.81f)),
            new Button("Moon Walk", Category.Move, true, false, ()=>Gravity(6.66f)),
            new Button("Jupiter Walk", Category.Move, true, false, ()=>Gravity(7.77f)),
            new Button("TP Gun", Category.Move, true, false, ()=>TPGun()),
            new Button("TP To Player Gun", Category.Move, true, false, ()=>TPPlayerGun()),
            new Button("Hover Gun", Category.Move, true, false, ()=>HoverGun()),
            new Button("Check Point [RG, RT, A]", Category.Move, true, false, ()=>Checkpoint()),
            #endregion

            #region Player
            new Button("Long Arms", Category.Player, true, false, ()=>LongArms(), ()=>FixArms()),
            new Button("Very Long Arms", Category.Player, true, false, ()=>VeryLongArms(), ()=>FixArms()),
            new Button("<color=red>[EXTREME]</color> Long Arms", Category.Player, true, false, ()=>VeryLongArmsX(), ()=>FixArms()),
            new Button("Short Arms", Category.Player, true, false, ()=>FlatMonk(), ()=>FixArms()),
            new Button("Upsidedown Head", Category.Player, true, false, ()=>UpsidedownHead(), ()=>FixHead()),
            new Button("Backwards Head", Category.Player, true, false, ()=>BackwardsHead(), ()=>FixHead()),
            new Button("Invis Monke [P]", Category.Player, true, false, ()=>InvisibleMonke()),
            new Button("Ghost Monke [P]", Category.Player, true, false, ()=>GhostMonke()),
            new Button("Head Spin X", Category.Player, true, false, ()=>HeadSpinx(), ()=>FixHead()),
            new Button("Head Spin Y", Category.Player, true, false, ()=>HeadSpiny(), ()=>FixHead()),
            new Button("Head Spin Z", Category.Player, true, false, ()=>HeadSpinx(), ()=>FixHead()),
            new Button("Freeze Rig", Category.Player, true, false, ()=>FreezeRig()),
            new Button("Fake Lag [G]", Category.Player, true, false, ()=>FakeLag()),
            new Button("Grab Rig [G]", Category.Player, true, false, ()=>GrabRig()),
            new Button("Spaz Rig", Category.Player, true, false, ()=>Spaz()),
            new Button("Annoy Player Gun", Category.Player, true, false, ()=>AnnoyPlayerGun()),
            new Button("Orbit Player Gun", Category.Player, true, false, ()=>OrbitPGun()),
            new Button("Orbit Gun", Category.Player, true, false, ()=>OrbitGun()),
            new Button("Bees [G]", Category.Player, true, false, ()=>Bees()),
            new Button("Fast Bees [G]", Category.Player, true, false, ()=>FBees()),
            new Button("Rig Gun", Category.Player, true, false, ()=>RigGun()),
            new Button("Grab Rig Gun", Category.Player, true, false, ()=>GrabGun()),
            new Button("Tag Gun", Category.Player, true, false, ()=>TagGun()),
            new Button("Tag Aura [G]", Category.Player, true, false, ()=>TagAura()),
            new Button("Tag All [T]", Category.Player, true, false, ()=>TagAll()),
            new Button("Tag Self [T]", Category.Player, true, false, ()=>TagSelf()),
            #endregion

            #region Visuals
            new Button("Hidden User ESP", Category.Visuals, true, false, ()=>HiddenESP(), ()=>DisableESP()),
            new Button("Chams", Category.Visuals, true, false, ()=>ESP(), ()=>DisableESP()),
            new Button("Tracers", Category.Visuals, true, false, ()=>Tracers()),
            new Button("2D Box ESP", Category.Visuals, true, false, ()=>BoxESP(false)),
            new Button("3D Box ESP", Category.Visuals, true, false, ()=>BoxESP(true)),
            new Button("Sphere ESP", Category.Visuals, true, false, ()=>BallESP()),
            new Button("Distance ESP", Category.Visuals, true, false, ()=>DistanceESP()),
            new Button("Nametags", Category.Visuals, true, false, ()=>Nametags()),
            new Button("Advanced Nametags", Category.Visuals, true, false, ()=>AdvNametags()),
            new Button("VR Info Display", Category.Visuals, true, false, ()=>InfoDisplay()),
            new Button("Snake ESP", Category.Visuals, true, false, ()=>SnakeESP()),
            #endregion

            new Button("Random CS Mods", Category.Fun, false, false, ()=>ChangePage(Category.CS)),
            new Button("Vibrator", Category.Fun, false, false, ()=>Vibrator()),
            new Button("Grab Bug [G]", Category.Fun, true, false, ()=> GrabBug()),
            new Button("Bug Gun", Category.Fun, true, false, ()=> BugGun()),
            new Button("Snipe Bug [G]", Category.Fun, true, false, ()=> SnipeBug()),
            new Button("Bug Halo", Category.Fun, true, false, ()=> BugHalo()),
            new Button("Grab Bat [G]", Category.Fun, true, false, ()=> GrabBat()),
            new Button("Bat Gun", Category.Fun, true, false, ()=> BatGun()),
            new Button("Snipe Bat [G]", Category.Fun, true, false, ()=> SnipeBat()),
            new Button("Bat Halo", Category.Fun, true, false, ()=> BatHalo()),
            new Button("Grab Soccer Ball [G]", Category.Fun, true, false, ()=> GrabSBall()),
            new Button("Soccer Ball Gun", Category.Fun, true, false, ()=> SBallGun()),

            new Button("Draw", Category.CS, true, false, ()=> Fun.Draw()),
            new Button("Orb Spam", Category.CS, true, false, ()=> Fun.GravDraw()),
            new Button("Orb Launcher", Category.CS, true, false, ()=> Spam1()),
            new Button("Tracer Orb Launcher", Category.CS, true, false, ()=> Spam2()),
            new Button("No Grav Orb Launcher", Category.CS, true, false, ()=> Spam3()),
            new Button("Big Orb Spam", Category.CS, true, false, ()=> BigSpam()),
            new Button("Spaz Orb", Category.CS, true, false, ()=> SpazOrb()),
            new Button("Gun Orb", Category.CS, true, false, ()=> OrbGun()),
            new Button("Big Gun Orb", Category.CS, true, false, ()=> OrbGun1()),
            new Button("Orb Rain", Category.CS, true, false, ()=> OrbRain()),
            new Button("Orb Rain Trace", Category.CS, true, false, ()=> OrbRain1()),

            new Button("Stump Text", Category.World, true, true, ()=>Stumpy(), ()=> STUMPY()),
            new Button("Unlock Comp", Category.World, true, false, ()=>UnlockComp()),
            new Button("Enable I Lava You Update", Category.World, true, false, ()=>EnableILavaYou(), ()=>DisableILavaYou()),
            new Button("Enable Rain", Category.World, true, false, ()=>Rain(), ()=>Rain1()),
            new Button("Change Time Night", Category.World, false, false, ()=> NightTimeMod()),
            new Button("Change Time Day", Category.World, false, false, ()=> idkTimeMod()),
            new Button("Enable Shadows", Category.World, true, false, ()=> Shadows(true), ()=> Shadows(false)),

            #region Credits
            new Button("Menu Credits:", Category.Creds, false, false, ()=>Placeholder()),
            new Button("Menker", Category.Creds, false, false, ()=>Placeholder()),
            new Button("NxO Template", Category.Creds, false, false, ()=>Placeholder()),
            new Button("Revanent GUI", Category.Creds, false, false, ()=>Placeholder()),
            new Button("Join The Discord!", Category.Creds, false, false, ()=>Discord()),
            #endregion
        };
    }
}
