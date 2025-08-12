using GorillaGameModes;
using Elixir.Mods.Categories;
using static Elixir.Menu.ButtonHandler;
using static Elixir.Menu.Main;
using static Elixir.Menu.Optimizations;
using static Elixir.Menu.Optimizations.ResourceLoader;
using static Elixir.Mods.Categories.Fun;
using static Elixir.Mods.Categories.Move;
using static Elixir.Mods.Categories.Playerr;
using static Elixir.Mods.Categories.Room;
using static Elixir.Mods.Categories.Settings;
using static Elixir.Mods.Categories.Visuals;
using static Elixir.Mods.Categories.World;
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
        Creds,
        CS,
        OP,
        IHateMyself
    }
    public class ModButtons
    {
        public static Button[] buttons =
        {
            #region Starting Page
            new Button("Settings", Category.Home, false, false, ()=>ChangePage(Category.Settings), null, "Opens Settings"),
            new Button("Room", Category.Home, false, false, ()=>ChangePage(Category.Room), null, "Opens The Room Category"),
            new Button("Movement", Category.Home, false, false, ()=>ChangePage(Category.Move), null, "Opens The Movement Category"),
            new Button("Player", Category.Home, false, false, ()=>ChangePage(Category.Player), null, "Opens The Player Category"),
            new Button("Visuals", Category.Home, false, false, ()=>ChangePage(Category.Visuals), null, "Opens The Visuals Category"),
            new Button("Fun", Category.Home, false, false, ()=>ChangePage(Category.Fun), null, "Opens The Fun Category"),
            new Button("World", Category.Home, false, false, ()=>ChangePage(Category.World), null, "Opens The World Category"),
           // new Button("IHateMyself", Category.Home, false, false, ()=>Access(), null, "Opens The IHateMyself Category"),
            new Button("Creds", Category.Home, false, false, ()=>ChangePage(Category.Creds), null, "Opens The Room Category"),
            #endregion

            #region Settings
            new Button("Disable All Mods", Category.Settings, false, false, ()=>DisableAllMods(), null, "Disables All Mods"),
            new Button("Right Handed Menu", Category.Settings, true, false, ()=>SwitchHands(true), ()=>SwitchHands(false), "Switches Hand The Menu Is On"),
            new Button("Disconnect Button", Category.Settings, true, true, ()=>ToggleDisconnectButton(true), ()=>ToggleDisconnectButton(false), "Toggles The Disconnect Button"),
            new Button("Toggle Version Counter", Category.Settings, true, true, ()=>ToggleVCounter(true), ()=>ToggleVCounter(false), "Toggles The Version Counter"),
            new Button("Toggle Notifications", Category.Settings, true, true, ()=>ToggleNotifications(true), ()=>ToggleNotifications(false), "Toggles Notifications"),
            new Button("Toggle Tool Tips", Category.Settings, true, true, ()=>ToggleTip(true), ()=>ToggleTip(false), "Toggles Tool Tips"),
            new Button("Clear Notifications", Category.Settings, false, false, ()=>ClearNotifications(), null, "Clears Notifications"),
            new Button("Toggle Array List", Category.Settings, true, true, ()=>Menu.ElixirGUI.ToggleArrayList(true), ()=>Menu.ElixirGUI.ToggleArrayList(false), "Toggles The Array List"),
            new Button("Bark Positioning", Category.Settings, true, false, ()=>Bark(true), ()=>Bark(false), "Toggles Bark Menu Position"),
            new Button("Menu Outline", Category.Settings, true, true, ()=>OLine(true), ()=>OLine(false), "Toggles Menu Outline"),
            new Button("Menu Gravity", Category.Settings, true, true, ()=>Grav(true), ()=>Grav(false), "Toggles Menu Gravity"),
            new Button("Change Layout: Sides", Category.Settings, false, false, ()=> ChangeLayout(), null, "Changes Menu Layout"),
            new Button("Change Theme: Default", Category.Settings, false, false, ()=> ChangeTheme(), null, "Changes Menu Theme"),
            new Button("Change Sound", Category.Settings, false, false, ()=> ChangeSound(), null, "Changes Button Sound"),
            new Button("Visualize Antireport", Category.Settings, true, true, ()=>VisReport(true), ()=>VisReport(false), "Toggles Antireport Vizualizer"),
            new Button("Change ESP Color: Infection", Category.Settings, false, false, ()=>ESPChange(), null, "Changes ESP Color"),
            new Button("Change Fly Speed: Normal", Category.Settings, false, false, ()=>FlySpeed(), null, "Changes Fly Speed"),
            new Button("Change Speed Boost: Normal", Category.Settings, false, false, ()=>SpeedSpeed(), null, "Changes Speed Boost Speed"),
            new Button("Change Gun Type: Ball + Line", Category.Settings, false, false, ()=>GunChange(), null, "Changes Speed Boost Speed"),
            new Button("Refresh Menu", Category.Settings, false, false, ()=> RefreshMenu(), null, "Refreshes The Menu"),
            #endregion

            #region Room
            new Button("Quit Game", Category.Room, true, false, ()=>QuitGTAG(), null, "..."),
            new Button("Join Random", Category.Room, false, false, ()=>JoinRandomPublic(), null, "Joins A Random Room"),
            new Button("Disconnect", Category.Room, false, false, ()=>Disconnect(), null, "Disconnects You From The Lobby"),
            new Button("Primary Disconnect", Category.Room, true, false, ()=>PrimaryDisconnect(), null, "Disconnects You If You Click Your A Button"),
            new Button("Check Master Client", Category.Room, false, false, ()=>IsMasterCheck(), null, "Checks Who Is Master"),
            new Button("Disable Network Triggers", Category.Room, false, false, ()=>DisableNetworkTriggers(), null, "Disables Network Triggers"),
            new Button("Enable Network Triggers", Category.Room, false, false, ()=>EnableNetworkTriggers(), null, "Enables Network Triggers"),
            new Button("Connect To US Servers", Category.Room, false, false, ()=>Servers("us"), null, "Connects To US Servers"),
            new Button("Connect To USW Servers", Category.Room, false, false, ()=>Servers("usw"), null, "Connects To USW Servers"),
            new Button("Connect To EU Servers", Category.Room, false, false, ()=>Servers("eu"), null, "Connects To EU Servers"),
            new Button("Set Mode Paintbrawl", Category.Room, false, false, ()=>SetGamemode(GameModeType.Paintbrawl), null, ""),
            new Button("Set Mode Hunt", Category.Room, false, false, ()=>SetGamemode(GameModeType.HuntDown), null, ""),
            new Button("Set Mode Ambush", Category.Room, false, false, ()=>SetGamemode(GameModeType.Ambush), null, ""),
            new Button("Set Mode Ghost", Category.Room, false, false, ()=>SetGamemode(GameModeType.Ghost), null, ""),
            new Button("Set Mode Error", Category.Room, false, false, ()=>SetGamemode(GameModeType.Count), null, ""),
            new Button("Join Menu Code", Category.Room, false, false, ()=>JoinRoom("$Elixir$"), null, "Joins A Special Code For This Menu"),
            new Button("Join Code MOD", Category.Room, false, false, ()=>JoinRoom("MOD"), null, "Joins Code MOD"),// Poo Poo
            new Button("Join Code PBBV", Category.Room, false, false, ()=>JoinRoom("PBBV"), null, "Joins Code PBBV"),
            new Button("Join Code Daisy09", Category.Room, false, false, ()=>JoinRoom("DAISY09"), null, "Joins Code DAISY09"),
            new Button("Mute Gun", Category.Room, true, false, ()=>MuteGun(), null, "Mutes Who You Shoot"),
            new Button("Mute Everyone", Category.Room, false, false, ()=>MuteAll(), null, "Mutes Everyone"),
            new Button("Report Gun", Category.Room, true, false, ()=>ReportGun(), null, "Reports Who You Shoot"),
            new Button("Report Everyone", Category.Room, false, false, ()=>ReportAll(), null, "Reports Everyone"),
            new Button("Copy Self ID", Category.Room, false, false, ()=> CopySelfID(), null, "Copys Your ID"),
            new Button("Copy ID Gun", Category.Room, true, false, ()=> CopyIDGun(), null, "Copys A Player's ID"),
            new Button("Anti Report", Category.Room, true, true, ()=>AntiReport(), null, "Disconnects You When People Try To Report You"),
            new Button("Strict Anti Report", Category.Room, true, false, ()=>StrictAntiReport(), null, "Anti Report But Bigger"),
            #endregion

            #region Movement
            new Button("Platforms [G]", Category.Move, true, false, ()=>Platforms(false, false), null, "Walk On Air"),
            new Button("Invis Platforms [G]", Category.Move, true, false, ()=>Platforms(true, false), null, "Invisible Platforms"),
            new Button("Plank Platforms", Category.Move, true, false, ()=>Platforms(false, true), null, "Long Platforms"),
            new Button("Force Tag Freeze", Category.Move, false, false, ()=>TagFreeze(), null, "Forces Tag Freeze"),
            new Button("No Tag Freeze", Category.Move, true, false, ()=>NoTagFreeze(), null, "No Tag Freeze"),
            new Button("NoClip [T]", Category.Move, true, false, ()=>Noclip(), null, "Walk Through Walls"),
            new Button("Speed Boost", Category.Move, true, false, ()=>Speedboost(), null, "Makes You Faster"),
            new Button("Dash [P]", Category.Move, true, false, ()=>DashJump(true), null, "Pushes You Forward"),
            new Button("Fling [P]", Category.Move, true, false, ()=>DashJump(false), null, "Flings You"),
            new Button("Velocity Jump", Category.Move, true, false, ()=>Velocity(-1f), ()=> Velocity(default), "Makes You Gain Velocity When Jumping"),
            new Button("Fly [P]", Category.Move, true, false, ()=>Fly(), null, "Fly In The Air"),
            new Button("Trigger Fly [T]", Category.Move, true, false, ()=>TriggerFly(), null, "Fly With Your Trigger"),
            new Button("Super Monke [P]", Category.Move, true, false, ()=>SuperMonke(), ()=>UseGravity(true), "Fly With No Gravity"),
            new Button("Hand Fly [P]", Category.Move, true, false, ()=>HandFly(), null, "Fly With Your Hand"),
            new Button("Slingshot Fly [T]", Category.Move, true, false, ()=>SlingshotFly(), null, "Fly With Velocity"),
            new Button("Iron Monke [G]", Category.Move, true, false, ()=>IronMonkey(), null, "Become Iron Man"),
            new Button("Punch Mod [BUGGY]", Category.Move, true, false, ()=>PunchMod(), null, "Very Buggy Punch Mod"),
            new Button("Forward & Backward [T]", Category.Move, true, false, ()=>carmonkey(), null, "Move Forwards & Backwards"),
            new Button("Up & Down [T]", Category.Move, true, false, ()=>UpAndDown(), null, "Move Up & Down"),
            new Button("Shit Hertz", Category.Move, false, false, ()=>Hertz(10), null, "Makes Your Hertz Shit"),
            new Button("45 Hertz", Category.Move, false, false, ()=>Hertz(45), null, "Sets Your Hertz To 45"),
            new Button("60 Hertz", Category.Move, false, false, ()=>Hertz(60), null, "Sets Your Hertz To 60"),
            new Button("120 Hertz", Category.Move, false, false, ()=>Hertz(120), null, "Sets Your Hertz To 120"),
            new Button("Reset/Unlimit Hertz", Category.Move, false, false, ()=>Hertz(int.MaxValue), null, "Unlimits Hertz"),
            new Button("WASD [PC]", Category.Move, true, false, ()=>WASDFly(), null, "Lets you fly on PC with WASD"),
            new Button("No Gravity", Category.Move, true, false, ()=>Gravity(9.81f), null, "No Gravity"),
            new Button("Moon Walk", Category.Move, true, false, ()=>Gravity(6.66f), null, "Moon Gravity"),
            new Button("Jupiter Walk", Category.Move, true, false, ()=>JupiterWalk(), null, "Jupiter Gravity"),
            new Button("TP Gun", Category.Move, true, false, ()=>TPGun(), null, "TP Where You Shoot"),
            new Button("TP To Player Gun", Category.Move, true, false, ()=>TPPlayerGun(), null, "TP To A Player"),
            new Button("Hover Gun", Category.Move, true, false, ()=>HoverGun(), null, "Hover Over A Player"),
            new Button("Check Point [RG, RT, A]", Category.Move, true, false, ()=>Checkpoint(), null, "Create Checkpoints"),
            #endregion
            
            #region Player
            new Button("Long Arms", Category.Player, true, false, ()=>LongArms(1.2f), ()=>FixArms(), "Makes Your Arms Longer"),
            new Button("Very Long Arms", Category.Player, true, false, ()=>LongArms(2f), ()=>FixArms(), "Very Long Arms"),
            new Button("<color=red>[EXTREME]</color> Long Arms", Category.Player, true, false, ()=>LongArms(6f), ()=>FixArms(), "Very Very Long Arms"),
            new Button("Short Arms", Category.Player, true, false, ()=>LongArms(0.5f), ()=>FixArms(), "Short Arms"),
            new Button("Upsidedown Head", Category.Player, true, false, ()=>UpsidedownHead(), ()=>FixHead(), "Makes Your Head Upsidedown"),
            new Button("Backwards Head", Category.Player, true, false, ()=>BackwardsHead(), ()=>FixHead(), "Makes Your Head Backwards"),
            new Button("Snap Neck", Category.Player, true, false, ()=>SnapNeck(), ()=>FixHead(), "Snaps Your Neck Dummy"),
            new Button("Invis Monke [P]", Category.Player, true, false, ()=>InvisibleMonke(), null, "Go Invisible"),
            new Button("Ghost Monke [P]", Category.Player, true, false, ()=>GhostMonke(), null, "Freeze Your Rig In Place"),
            new Button("Head Spin X", Category.Player, true, false, ()=>HeadSpinx(), ()=>FixHead(), "Spin Head On X Axis"),
            new Button("Head Spin Y", Category.Player, true, false, ()=>HeadSpiny(), ()=>FixHead(), "Spin Head On Y Axis"),
            new Button("Head Spin Z", Category.Player, true, false, ()=>HeadSpinx(), ()=>FixHead(), "Spin Head On Z Axis"),
            new Button("Freeze Rig", Category.Player, true, false, ()=>FreezeRig(), null, "Freeze Your Rig"),
            new Button("Fake Lag [G]", Category.Player, true, false, ()=>FakeLag(), null, "Makes You Look Laggy"),
            new Button("Grab Rig [G]", Category.Player, true, false, ()=>GrabRig(), null, "Grab Your Rig"),
            new Button("Spaz Rig", Category.Player, true, false, ()=>Spaz(), null, "Makes You Spaz Out"),
            new Button("Spaz Hands", Category.Player, true, false, ()=>SpazHands(), null, "Makes Your Hands Spaz Out"),
            new Button("Annoy Player Gun", Category.Player, true, false, ()=>AnnoyPlayerGun(), null, "Annoy A Player"),
            new Button("Annoy Self", Category.Player, true, false, ()=>AnnoySelf(), null, "Annoy Yourself ┇ Similar To Spaz Rig"),
            new Button("Orbit Player Gun", Category.Player, true, false, ()=>OrbitPGun(), null, "Orbit A Player"),
            new Button("Orbit Gun", Category.Player, true, false, ()=>OrbitGun(), null, "Orbit Where You Shoot"),
            new Button("Orbit Self", Category.Player, true, false, ()=>OrbitSelf(), null, "Orbit Yourself"),
            new Button("Bees [G]", Category.Player, true, false, ()=>Bees(), null, "Fly Around The Map"),
            new Button("Fast Bees [G]", Category.Player, true, false, ()=>FBees(), null, "Bees But Faster"),
            new Button("Chase Gun", Category.Player, true, false, ()=>ChaseGun(), null, "Bees But Faster"),
            new Button("Rig Gun", Category.Player, true, false, ()=>RigGun(), null, "Put Your Rig Where You Shoot"),
            new Button("Grab Your Rig Gun", Category.Player, true, false, ()=>GrabGun(), null, "Makes People Grab Your Rig"),
            new Button("Backshots Gun", Category.Player, true, false, ()=>SexGun(), null, "Makes People Grab Your Rig"),
            new Button("Head Gun", Category.Player, true, false, ()=>HeadGun(), null, "Makes People Grab Your Rig"),
            new Button("Get Head Gun", Category.Player, true, false, ()=>GetHeadGun(), null, "Makes People Grab Your Rig"),
            new Button("Tag Gun", Category.Player, true, false, ()=>TagGun(), null, "Tags A Player"),
            new Button("Tag Aura [G]", Category.Player, true, false, ()=>TagAura(), null, "Tag People Near You"),
            new Button("Tag All [T]", Category.Player, true, false, ()=>TagAll(), null, "Tag Everyone"),
            new Button("Tag Self [T]", Category.Player, true, false, ()=>TagSelf(), null, "Tag Yourself"),
            new Button("Max Quest Score", Category.Player, false, false, ()=>QuestScore(99999), null, "Splashes Your Hands"),
            new Button("69420 Quest Score", Category.Player, false, false, ()=>QuestScore(69420), null, "Splashes Where You Shoot"),

            #endregion

            #region Visuals
            //new Button("Menu Tags", Category.Visuals, true, true, ()=>TAGS(), null, "See Who Is Using Menus"),
            new Button("Chams", Category.Visuals, true, false, ()=>ESP(), ()=>DisableESP(), "See People Through Walls"),
            new Button("Tracers", Category.Visuals, true, false, ()=>Tracers(), null, "Creats Tracers To Players"),
            new Button("2D Box ESP", Category.Visuals, true, false, ()=>BoxESP(false), null, "Creates A 2D Box"),
            new Button("3D Box ESP", Category.Visuals, true, false, ()=>BoxESP(true), null, "Creates A 3D Box"),
            new Button("Sphere ESP", Category.Visuals, true, false, ()=>BallESP(), null, "Creates A Sphere"),
            new Button("Bug ESP", Category.Visuals, true, false, ()=>EntityESP(false), null, "Creates A Sphere"),
            new Button("Bat ESP", Category.Visuals, true, false, ()=>EntityESP(true), null, "Creates A Sphere"),
            new Button("Distance ESP", Category.Visuals, true, false, ()=>DistanceESP(), null, "Measures Distance Of Players"),
            new Button("Nametags", Category.Visuals, true, false, ()=>Nametags(), null, "Shows Players Names"),
            new Button("Advanced Nametags", Category.Visuals, true, false, ()=>AdvNametags(), null, "Nametags But More Info"),
            new Button("VR Info Display", Category.Visuals, true, false, ()=>InfoDisplay(), null, "Creates A VR Info Display"),
            new Button("Snake ESP", Category.Visuals, true, false, ()=>SnakeESP(), null, "Creates Snake ESP On Players"),
            new Button("Skeleton ESP", Category.Visuals, true, false, ()=>EnableSkeleton(), ()=> DisableSkeleton(), "View Players Skeletons"),
            new Button("Ignore Gun [CS]", Category.Visuals, true, false, ()=>Ignore(false), null, "Completely Removes Players From Your Game"),
            #endregion

            #region Fun
            new Button("Random CS Mods", Category.Fun, false, false, ()=>ChangePage(Category.CS), null, "Opens CS Category"),
            new Button("Vibrator", Category.Fun, true, false, ()=>Vibrator(), null, "Vibrates"),
            new Button("Splash Hands [G]", Category.Fun, true, false, ()=>SplashHands(), null, "Splashes Your Hands"),
            new Button("Splash Gun", Category.Fun, true, false, ()=>SplashGun(), null, "Splashes Where You Shoot"),
            new Button("Splash Aura [G]", Category.Fun, true, false, ()=>SplashAura(), null, "Splashes Where You Shoot"),
            new Button("Give Splash Gun", Category.Fun, true, false, ()=>GiveSplash(), null, "Makes Others Splash"),
            new Button("Schitzo Gun V1", Category.Fun, true, false, ()=>SchitzoV1(), null, "Makes Others Splash"),
            new Button("Schitzo Gun V2", Category.Fun, true, false, ()=>SchitzoV2(), null, "Makes Others Splash"),
            new Button("Grab Bug [G]", Category.Fun, true, false, ()=> GrabBug(), null, "Grab The Bug"),
            new Button("Bug Gun", Category.Fun, true, false, ()=> BugGun(), null, "Places The Bug Where You Shoot"),
            new Button("Snipe Bug [G]", Category.Fun, true, false, ()=> SnipeBug(), null, "Snipes The Bug"),
            new Button("Bug Halo", Category.Fun, true, false, ()=> BugHalo(), null, "Halos The Bug Above You"),
            new Button("Grab Bat [G]", Category.Fun, true, false, ()=> GrabBat(), null, "Grab The Bat"),
            new Button("Bat Gun", Category.Fun, true, false, ()=> BatGun(), null, "Places The Bat Where You Shoot"),
            new Button("Snipe Bat [G]", Category.Fun, true, false, ()=> SnipeBat(), null, "Snipes The Bat"),
            new Button("Bat Halo", Category.Fun, true, false, ()=> BatHalo(), null, "Halos The Bat Above You"),
            new Button("Grab Soccer Ball [G]", Category.Fun, true, false, ()=> GrabSBall(), null, "Grabs The Soccer Ball"),
            new Button("Soccer Ball Gun", Category.Fun, true, false, ()=> SBallGun(), null, "Places The Ball Where You Shoot"),
            #endregion

            #region CS
            new Button("Draw", Category.CS, true, false, ()=> Fun.Draw(), null, "You Can Draw On Air"),
            new Button("Orb Spam", Category.CS, true, false, ()=> Fun.GravDraw(), null, "Spams Orbs"),
            new Button("Orb Launcher", Category.CS, true, false, ()=> Spam1(), null, "Launches Obrs"),
            new Button("Tracer Orb Launcher", Category.CS, true, false, ()=> Spam2(), null, "Launches Tracer Orbs"),
            new Button("No Grav Orb Launcher", Category.CS, true, false, ()=> Spam3(), null, "Launches No Gravity Orbs"),
            new Button("Big Orb Spam", Category.CS, true, false, ()=> BigSpam(), null, "Spams Big Orbs"),
            new Button("Spaz Orb", Category.CS, true, false, ()=> SpazOrb(), null, "Shoots Orbs Everywhere"),
            new Button("Gun Orb", Category.CS, true, false, ()=> OrbGun(), null, "Shoot Orbs"),
            new Button("Big Gun Orb", Category.CS, true, false, ()=> OrbGun1(), null, "Shoot Big Orbs"),
            new Button("Orb Rain", Category.CS, true, false, ()=> OrbRain(), null, "Rains Orbs"),
            new Button("Orb Rain Trace", Category.CS, true, false, ()=> OrbRain1(), null, "Nicer Looking Orb Rain"),
            #endregion

            #region World
            new Button("Stump Text", Category.World, true, true, ()=>Stumpy(), ()=> STUMPY(), "Makes Text In Stump"),
            new Button("Unlock Comp", Category.World, true, false, ()=>UnlockComp(), null, "Unlocks Comp"),
            new Button("Enable I Lava You Update", Category.World, true, false, ()=>EnableILavaYou(), ()=>DisableILavaYou(), "Toggles I Lava You Update"),
            new Button("Enable Rain", Category.World, true, false, ()=>Rain(), ()=>Rain1(), "Toggles Rain"),
            new Button("Change Time Night", Category.World, false, false, ()=> NightTimeMod(), null, "Makes It Night"),
            new Button("Change Time Day", Category.World, false, false, ()=> idkTimeMod(), null, "Makes It Day"),
            new Button("Enable Shadows", Category.World, true, false, ()=> Shadows(true), ()=> Shadows(false), "Toggles Shadows"),
            #endregion

            /*
            #region IHateMyself
new Button("Slow Gun", Category.IHateMyself, true, false, ()=>SlowGunVP()),
new Button("Slow All", Category.IHateMyself, true, false, ()=>SlowAllVP()),
new Button("Vibrate Gun", Category.IHateMyself, true, false, ()=>VibrateGunVP()),
new Button("Vibrate All", Category.IHateMyself, true, false, ()=>VibrateAllVP()),
new Button("Fling Gun", Category.IHateMyself, true, false, ()=>FlingGunVP()),
new Button("Fling All", Category.IHateMyself, true, false, ()=>FlingAllVP()),
new Button("Fast Gun", Category.IHateMyself, true, false, ()=>FastGunVP()),
new Button("Fast All", Category.IHateMyself, true, false, ()=>FastAllVP()),
//new Button("Force Menu Gun", Category.IHateMyself, true, false, ()=>ForceMenuGunVP()),
//new Button("Force Menu All", Category.IHateMyself, true, false, ()=>ForceMenuAllVP()),
new Button("Grab Gun", Category.IHateMyself, true, false, ()=>GrabGunVP()),
new Button("Grab All", Category.IHateMyself, true, false, ()=>GrabAllVP()),
new Button("Kick Gun", Category.IHateMyself, true, false, ()=>KickGunVP()),
new Button("Kick All", Category.IHateMyself, true, false, ()=>KickAllVP()),
new Button("Black Screen Gun", Category.IHateMyself, true, false, ()=>BlackScreenGunVP()),
new Button("Black Screen All", Category.IHateMyself, true, false, ()=>BlackScreenAllVP()),
//new Button("Nova Steam Crash Gun", Category.IHateMyself, true, false, ()=>NovaGun()),
//new Button("Nova Steam Crash All", Category.IHateMyself, true, false, ()=>NovaAll()),
new Button("App Quit Gun", Category.IHateMyself, true, false, ()=>QuitAppGunVP()),
new Button("App Quit All", Category.IHateMyself, true, false, ()=>QuitAppAllVP()),
//new Button("Code Mist Announcement", Category.IHateMyself, false, false, ()=>AnnouncementCodeMist()),
            #endregion
*/
            #region Credits
            new Button("Menu Credits:", Category.Creds, false, false, ()=>Placeholder(), null, "Credits Of The Menu"),
            new Button("Menker", Category.Creds, false, false, ()=>Placeholder(), null, "Menu Owner"),
            new Button("NxO Template", Category.Creds, false, false, ()=>Placeholder(), null, "Template This Is Based From"),
            new Button("Cha", Category.Creds, false, false, ()=>Placeholder(), null, "I forgot"),
            new Button("Meep", Category.Creds, false, false, ()=>Placeholder(), null, "Set Gamemode Code"),
            new Button("Fwog", Category.Creds, false, false, ()=>Placeholder(), null, "Stuff"),
            new Button("Glxy", Category.Creds, false, false, ()=>Placeholder(), null, "GUI Notifications"),
            new Button("Join The Discord!", Category.Creds, false, false, ()=>Discord(), null, "Join Our Discord Server"),
            #endregion
        };
    }
}
