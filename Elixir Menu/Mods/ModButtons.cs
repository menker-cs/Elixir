using GorillaGameModes;
using Elixir.Mods.Categories;
using static Elixir.Menu.ButtonHandler;
using static Elixir.Menu.Main;
using static Elixir.Menu.Optimizations;
using static Elixir.Mods.Categories.Fun;
using static Elixir.Mods.Categories.Move;
using static Elixir.Mods.Categories.Playerr;
using static Elixir.Mods.Categories.Room;
using static Elixir.Mods.Categories.Settings;
using static Elixir.Mods.Categories.Visuals;
using static Elixir.Mods.Categories.World;
using static Elixir.Mods.Categories.SherbertClass;
using static Elixir.Utilities.ColorLib;
using static Elixir.Utilities.GunTemplate;
using static Elixir.Utilities.Variables;

namespace Elixir.Mods
{
    public enum Category
    {
        Home,
        Settings,
        Room,
        Player,
        Move,
        World,
        Fun,
        Visuals,
        CS,
        Splash,

        Creds,
        MenuCreds,
        ModCreds,
    }
    public class ModButtons
    {
        public static Button[] buttons =
        {
            #region Starting Page
            new Button("Settings", Category.Home, false, false, ()=>ChangePage(Category.Settings), null),
            new Button("Room", Category.Home, false, false, ()=>ChangePage(Category.Room), null),
            new Button("Movement", Category.Home, false, false, ()=>ChangePage(Category.Move), null),
            new Button("Player", Category.Home, false, false, ()=>ChangePage(Category.Player), null),
            new Button("Visuals", Category.Home, false, false, ()=>ChangePage(Category.Visuals), null),
            new Button("Fun", Category.Home, false, false, ()=>ChangePage(Category.Fun), null),
            new Button("World", Category.Home, false, false, ()=>ChangePage(Category.World), null),
            new Button("Credits", Category.Home, false, false, ()=>ChangePage(Category.Creds), null),
            #endregion

            #region Settings
            new Button("Disable All Mods", Category.Settings, false, false, ()=>DisableAllMods(), null),
            new Button("Right Handed Menu", Category.Settings, true, false, ()=>SwitchHands(true), ()=>SwitchHands(false)),
            new Button("Disconnect Button", Category.Settings, true, true, ()=>ToggleDisconnectButton(true), ()=>ToggleDisconnectButton(false)),
            new Button("Toggle Version Counter", Category.Settings, true, true, ()=>ToggleVCounter(true), ()=>ToggleVCounter(false)),
            new Button("Toggle Notifications", Category.Settings, true, true, ()=>ToggleNotifications(true), ()=>ToggleNotifications(false)),
            new Button("Toggle Tool Tips", Category.Settings, true, true, ()=>ToggleTip(true), ()=>ToggleTip(false)),
            new Button("Clear Notifications", Category.Settings, false, false, ()=>ClearNotifications(), null),
            new Button("Bark Positioning", Category.Settings, true, false, ()=>Bark(true), ()=>Bark(false)),
            new Button("Menu Outline", Category.Settings, true, true, ()=>OLine(true), ()=>OLine(false)),
            new Button("Menu Gravity", Category.Settings, true, true, ()=>Grav(true), ()=>Grav(false)),
            new Button("Change Layout: Sides", Category.Settings, false, false, ()=> ChangeLayout(), null),
            new Button("Change Theme: Default", Category.Settings, false, false, ()=> ChangeTheme(), null),
            new Button("Change Sound", Category.Settings, false, false, ()=> ChangeSound(), null),
            new Button("Visualize Antireport", Category.Settings, true, true, ()=>VisReport(true), ()=>VisReport(false)),
            new Button("Change ESP Color: Infection", Category.Settings, false, false, ()=>ESPChange(), null),
            new Button("Change Tracer Position: Right", Category.Settings, false, false, ()=>TracerPos(), null),
            new Button("Change Fly Speed: Normal", Category.Settings, false, false, ()=>FlySpeed(), null),
            new Button("Change Speed Boost: Normal", Category.Settings, false, false, ()=>SpeedSpeed(), null),
            new Button("Change Gun Type: Ball + Line", Category.Settings, false, false, ()=>GunChange(), null),
            new Button("Refresh Menu", Category.Settings, false, false, ()=> RefreshMenu(), null),
            #endregion

            #region Room
            new Button("Quit Game", Category.Room, true, false, ()=>QuitGTAG(), null),
            new Button("Join Random", Category.Room, false, false, ()=>JoinRandomPublic(), null),
            new Button("Disconnect", Category.Room, false, false, ()=>Disconnect(), null),
            new Button("Primary Disconnect", Category.Room, true, false, ()=>PrimaryDisconnect(), null),
            new Button("Check Master Client", Category.Room, false, false, ()=>IsMasterCheck(), null),
            new Button("Disable Network Triggers", Category.Room, false, false, ()=>DisableNetworkTriggers(), null),
            new Button("Enable Network Triggers", Category.Room, false, false, ()=>EnableNetworkTriggers(), null),
            new Button("Connect To US Servers", Category.Room, false, false, ()=>Servers("us"), null),
            new Button("Connect To USW Servers", Category.Room, false, false, ()=>Servers("usw"), null),
            new Button("Connect To EU Servers", Category.Room, false, false, ()=>Servers("eu"), null),
            new Button("Set Mode Paintbrawl", Category.Room, false, false, ()=>SetGamemode(GameModeType.Paintbrawl), null),
            new Button("Set Mode Hunt", Category.Room, false, false, ()=>SetGamemode(GameModeType.HuntDown), null),
            new Button("Set Mode Ambush", Category.Room, false, false, ()=>SetGamemode(GameModeType.Ambush), null),
            new Button("Set Mode Ghost", Category.Room, false, false, ()=>SetGamemode(GameModeType.Ghost), null),
            new Button("Set Mode Error", Category.Room, false, false, ()=>SetGamemode(GameModeType.Count), null),
            new Button("Join Menu Code", Category.Room, false, false, ()=>JoinRoom("$Elixir$"), null),
            new Button("Join Code MOD", Category.Room, false, false, ()=>JoinRoom("MOD"), null),// Poo Poo
            new Button("Join Code PBBV", Category.Room, false, false, ()=>JoinRoom("PBBV"), null),
            new Button("Join Code Daisy09", Category.Room, false, false, ()=>JoinRoom("DAISY09"), null),
            new Button("Mute Gun", Category.Room, true, false, ()=>MuteGun(), null),
            new Button("Mute Everyone", Category.Room, false, false, ()=>MuteAll(), null),
            new Button("Report Gun", Category.Room, true, false, ()=>ReportGun(), null),
            new Button("Report Everyone", Category.Room, false, false, ()=>ReportAll(), null),
            new Button("Copy Self ID", Category.Room, false, false, ()=> CopySelfID(), null),
            new Button("Copy ID Gun", Category.Room, true, false, ()=> CopyIDGun(), null),
            new Button("Anti Report", Category.Room, true, true, ()=>AntiReport(), null),
            new Button("Strict Anti Report", Category.Room, true, false, ()=>StrictAntiReport(), null),
            #endregion

            #region Movement
            new Button("Platforms [G]", Category.Move, true, false, ()=>Platforms(false, false), null),
            new Button("Invis Platforms [G]", Category.Move, true, false, ()=>Platforms(true, false), null),
            new Button("Plank Platforms", Category.Move, true, false, ()=>Platforms(false, true), null),
            new Button("Force Tag Freeze", Category.Move, false, false, ()=>TagFreeze1(), null),
            new Button("No Tag Freeze", Category.Move, true, false, ()=>TagFreeze(true), null),
            new Button("NoClip [T]", Category.Move, true, false, ()=>Noclip(), null),
            new Button("Speed Boost", Category.Move, true, false, ()=>Speedboost(), null),
            new Button("Grip Speed Boost [G]", Category.Move, true, false, ()=>GSpeedboost(), null),
            new Button("Dash [P]", Category.Move, true, false, ()=>DashJump(true), null),
            new Button("Fling [P]", Category.Move, true, false, ()=>DashJump(false), null),
            new Button("Velocity Jump", Category.Move, true, false, ()=>Velocity(-1.7f), ()=> Velocity(default)),
            new Button("Fly [P]", Category.Move, true, false, ()=>Fly(), null),
            new Button("Trigger Fly [T]", Category.Move, true, false, ()=>TriggerFly(), null),
            new Button("Super Monke [P]", Category.Move, true, false, ()=>SuperMonke()),
            new Button("Hand Fly [P]", Category.Move, true, false, ()=>HandFly(), null),
            new Button("Slingshot Fly [T]", Category.Move, true, false, ()=>SlingshotFly(), null),
            new Button("Iron Monke [G]", Category.Move, true, false, ()=>IronMonkey(), null),
            new Button("Punch Mod [BUGGY]", Category.Move, true, false, ()=>PunchMod(), null),
            new Button("Forward & Backward [T]", Category.Move, true, false, ()=>Carmonkey(), null),
            new Button("Up & Down [T]", Category.Move, true, false, ()=>UpAndDown(), null),
            //new Button("Shit Hertz", Category.Move, false, false, ()=>Hertz(10), null),
            //new Button("45 Hertz", Category.Move, false, false, ()=>Hertz(45), null),
            //new Button("60 Hertz", Category.Move, false, false, ()=>Hertz(60), null),
            //new Button("120 Hertz", Category.Move, false, false, ()=>Hertz(120), null),
            //new Button("Reset/Unlimit Hertz", Category.Move, false, false, ()=>Hertz(int.MaxValue), null),
            new Button("WASD [PC]", Category.Move, true, false, ()=>WASDFly(), null),
            new Button("Joystick Fly", Category.Move, true, false, ()=>DroneFly(), null),
            new Button("No Gravity", Category.Move, true, false, ()=>Gravity(9.81f), null),
            new Button("Moon Walk", Category.Move, true, false, ()=>Gravity(6.66f), null),
            new Button("Jupiter Walk", Category.Move, true, false, ()=>JupiterWalk(), null),
            new Button("TP Gun", Category.Move, true, false, ()=>TPGun(), null),
            new Button("TP To Player Gun", Category.Move, true, false, ()=>TPPlayerGun(), null),
            new Button("Hover Gun", Category.Move, true, false, ()=>HoverGun(), null),
            new Button("Check Point [RG, RT, A]", Category.Move, true, false, ()=>Checkpoint(), null),
            #endregion
            
            #region Player
            new Button("Long Arms", Category.Player, true, false, ()=>LongArms(1.2f), ()=>FixArms()),
            new Button("Very Long Arms", Category.Player, true, false, ()=>LongArms(2f), ()=>FixArms()),
            new Button("<color=red>[EXTREME]</color> Long Arms", Category.Player, true, false, ()=>LongArms(6f), ()=>FixArms()),
            new Button("Short Arms", Category.Player, true, false, ()=>LongArms(0.5f), ()=>FixArms()),
            new Button("Upsidedown Head", Category.Player, true, false, ()=>UpsidedownHead(), ()=>FixHead()),
            new Button("Backwards Head", Category.Player, true, false, ()=>BackwardsHead(), ()=>FixHead()),
            new Button("Snap Neck", Category.Player, true, false, ()=>SnapNeck(), ()=>FixHead()),
            new Button("Invis Monke [P]", Category.Player, true, false, ()=>InvisibleMonke(), null),
            new Button("Ghost Monke [P]", Category.Player, true, false, ()=>GhostMonke(), null),
            new Button("Head Spin X", Category.Player, true, false, ()=>HeadSpinx(), ()=>FixHead()),
            new Button("Head Spin Y", Category.Player, true, false, ()=>HeadSpiny(), ()=>FixHead()),
            new Button("Head Spin Z", Category.Player, true, false, ()=>HeadSpinx(), ()=>FixHead()),
            new Button("Freeze Rig", Category.Player, true, false, ()=>FreezeRig(), null),
            new Button("Spin", Category.Player, true, false, ()=>Spin(), null),
            new Button("Ascend", Category.Player, true, false, ()=>Ascend(), null),
            new Button("T Pose", Category.Player, true, false, ()=>Tpose(), null),
            new Button("Helicopter", Category.Player, true, false, ()=>Helicopter(), null),
            new Button("Fake Lag [G]", Category.Player, true, false, ()=>FakeLag(), null),
            new Button("Grab Rig [G]", Category.Player, true, false, ()=>GrabRig(), null),
            new Button("Spaz Rig", Category.Player, true, false, ()=>Spaz(), null),
            new Button("Spaz Hands", Category.Player, true, false, ()=>SpazHands(), null),
            new Button("Annoy Player Gun", Category.Player, true, false, ()=>AnnoyPlayerGun(), null),
            new Button("Annoy Self", Category.Player, true, false, ()=>AnnoySelf(), null),
            new Button("Orbit Player Gun", Category.Player, true, false, ()=>OrbitPGun(), null),
            new Button("Orbit Gun", Category.Player, true, false, ()=>OrbitGun(), null),
            new Button("Orbit Self", Category.Player, true, false, ()=>OrbitSelf(), null),
            new Button("Bees [G]", Category.Player, true, false, ()=>Bees(), null),
            new Button("Fast Bees [G]", Category.Player, true, false, ()=>FBees(), null),
            new Button("Chase Gun", Category.Player, true, false, ()=>ChaseGun(), null),
            new Button("Rig Gun", Category.Player, true, false, ()=>RigGun(), null),
            new Button("Grab Your Rig Gun", Category.Player, true, false, ()=>GrabGun(), null),
            new Button("Backshots Gun", Category.Player, true, false, ()=>SexGun(), null),
            new Button("Head Gun", Category.Player, true, false, ()=>HeadGun(), null),
            new Button("Get Head Gun", Category.Player, true, false, ()=>GetHeadGun(), null),
            new Button("Tag Gun", Category.Player, true, false, ()=>TagGun(), null),
            new Button("Tag Aura [G]", Category.Player, true, false, ()=>TagAura(), null),
            new Button("Tag All [T]", Category.Player, true, false, ()=>TagAll(), null),
            new Button("Tag Self [T]", Category.Player, true, false, ()=>TagSelf(), null),
            new Button("Max Quest Score", Category.Player, false, false, ()=>QuestScore(99999), null),
            new Button("69420 Quest Score", Category.Player, false, false, ()=>QuestScore(69420), null),

            #endregion

            #region Visuals
            new Button("Chams", Category.Visuals, true, false, ()=>ESP(), ()=>DisableESP()),
            new Button("Tracers", Category.Visuals, true, false, ()=>Tracers(), null),
            new Button("Beacons", Category.Visuals, true, false, ()=>Beacons(), null),
            new Button("2D Box ESP", Category.Visuals, true, false, ()=>BoxESP(false), null),
            new Button("3D Box ESP", Category.Visuals, true, false, ()=>BoxESP(true), null),
            new Button("3D Wireframe ESP", Category.Visuals, true, false, ()=>Wireframe(false), null),
            new Button("3D Wireframe ESP", Category.Visuals, true, false, ()=>Wireframe(true), null),
            new Button("CSGO ESP", Category.Visuals, true, false, ()=>CSGO(), null),
            new Button("Sphere ESP", Category.Visuals, true, false, ()=>BallESP(), null),
            new Button("Bug ESP", Category.Visuals, true, false, ()=>EntityESP(false), null),
            new Button("Bat ESP", Category.Visuals, true, false, ()=>EntityESP(true), null),
            new Button("Distance ESP", Category.Visuals, true, false, ()=>DistanceESP(), null),
            new Button("Nametags", Category.Visuals, true, false, ()=>Nametags(), null),
            new Button("Advanced Nametags", Category.Visuals, true, false, ()=>AdvNametags(), null),
            new Button("VR Info Display", Category.Visuals, true, false, ()=>InfoDisplay(), null),
            new Button("Snake ESP", Category.Visuals, true, false, ()=>SnakeESP(), null),
            new Button("Skeleton ESP", Category.Visuals, true, false, ()=>EnableSkeleton(), ()=> DisableSkeleton()),
            new Button("Ignore Gun [CS]", Category.Visuals, true, false, ()=>Ignore(), null),
            #endregion

            #region Fun
            new Button("Random CS Mods", Category.Fun, false, false, ()=>ChangePage(Category.CS), null),
            new Button("Splash Mods", Category.Fun, false, false, ()=>ChangePage(Category.Splash), null),

                #region Splash Mods
            new Button("Back", Category.Splash, false, false, ()=>ChangePage(Category.Fun), null),
            new Button("Splash Hands [G]", Category.Splash, true, false, ()=>SplashHands(), null),
            new Button("Splash Gun", Category.Splash, true, false, ()=>SplashGun(), null),
            new Button("Splash Aura [G]", Category.Splash, true, false, ()=>SplashAura(), null),
            new Button("Give Splash Gun", Category.Splash, true, false, ()=>GiveSplash(), null),
            new Button("Schitzo Gun V1", Category.Splash, true, false, ()=>SchitzoV1(), null),
            new Button("Schitzo Gun V2", Category.Splash, true, false, ()=>SchitzoV2(), null),
            #endregion

                #region CS
            new Button("Back", Category.CS, false, false, ()=>ChangePage(Category.Fun), null),
            new Button("Draw", Category.CS, true, false, ()=> Fun.Draw(), null),
            new Button("Orb Spam", Category.CS, true, false, ()=> Fun.GravDraw(), null),
            new Button("Orb Launcher", Category.CS, true, false, ()=> Spam1(), null),
            new Button("Tracer Orb Launcher", Category.CS, true, false, ()=> Spam2(), null),
            new Button("No Grav Orb Launcher", Category.CS, true, false, ()=> Spam3(), null),
            new Button("Big Orb Spam", Category.CS, true, false, ()=> BigSpam(), null),
            new Button("Spaz Orb", Category.CS, true, false, ()=> SpazOrb(), null),
            new Button("Gun Orb", Category.CS, true, false, ()=> OrbGun(), null),
            new Button("Big Gun Orb", Category.CS, true, false, ()=> OrbGun1(), null),
            new Button("Orb Rain", Category.CS, true, false, ()=> OrbRain(), null),
            new Button("Orb Rain Trace", Category.CS, true, false, ()=> OrbRain1(), null),
            #endregion

            new Button("Sherbert", Category.Fun, true, false, ()=>Sherbert(), ()=>KillSherbert()),
            new Button("Vibrator", Category.Fun, true, false, ()=>Vibrator(), null),
            new Button("Grab Bug [G]", Category.Fun, true, false, ()=> GrabBug(), null),
            new Button("Bug Gun", Category.Fun, true, false, ()=> BugGun(), null),
            new Button("Snipe Bug [G]", Category.Fun, true, false, ()=> SnipeBug(), null),
            new Button("Bug Halo", Category.Fun, true, false, ()=> BugHalo(), null),
            new Button("Grab Bat [G]", Category.Fun, true, false, ()=> GrabBat(), null),
            new Button("Bat Gun", Category.Fun, true, false, ()=> BatGun(), null),
            new Button("Snipe Bat [G]", Category.Fun, true, false, ()=> SnipeBat(), null),
            new Button("Bat Halo", Category.Fun, true, false, ()=> BatHalo(), null),
            new Button("Grab Soccer Ball [G]", Category.Fun, true, false, ()=> GrabSBall(), null),
            new Button("Soccer Ball Gun", Category.Fun, true, false, ()=> SBallGun(), null),
            #endregion

            #region World
            new Button("Stump Text", Category.World, true, true, ()=>Stumpy(), ()=> STUMPY()),
            new Button("Unlock Comp", Category.World, true, false, ()=>UnlockComp(), null),
            new Button("Enable I Lava You Update", Category.World, true, false, ()=>EnableILavaYou(), ()=>DisableILavaYou()),
            new Button("Enable Rain", Category.World, true, false, ()=>Rain(), ()=>Rain1()),
            new Button("Change Time Night", Category.World, false, false, ()=> NightTimeMod(), null),
            new Button("Change Time Day", Category.World, false, false, ()=> idkTimeMod(), null),
            new Button("Enable Shadows", Category.World, true, false, ()=> Shadows(true), ()=> Shadows(false)),
            #endregion

            #region Credits
            new Button("Menu Credits", Category.Creds, false, false, ()=>ChangePage(Category.MenuCreds), null),
            new Button("Mod Credits", Category.Creds, false, false, ()=>ChangePage(Category.ModCreds), null),
            new Button("Join The Discord!", Category.Creds, false, false, ()=>Discord(), null),

            new Button("Menu Credits:", Category.MenuCreds, false, false, ()=>Placeholder(), null),
            new Button("Owners", Category.MenuCreds, false, false, ()=>Placeholder(), null),
            new Button("NxO Template", Category.MenuCreds, false, false, ()=>Placeholder(), null),
            new Button("Back", Category.MenuCreds, false, false, ()=>ChangePage(Category.Creds), null),

            new Button("Mod Credits:", Category.ModCreds, false, false, ()=>Placeholder(), null),
            new Button("Cha", Category.ModCreds, false, false, ()=>Placeholder(), null),
            new Button("Glxy", Category.ModCreds, false, false, ()=>Placeholder(), null),
            new Button("Back", Category.ModCreds, false, false, ()=>ChangePage(Category.Creds), null),
            #endregion
        };
    }
}
