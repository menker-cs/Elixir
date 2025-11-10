using GorillaGameModes;
using static Elixir.Management.Menu;
using static Elixir.Mods.Categories.Fun;
using static Elixir.Mods.Categories.Move;
using static Elixir.Mods.Categories.Playerr;
using static Elixir.Mods.Categories.Room;
using static Elixir.Mods.Categories.Settings;
using static Elixir.Mods.Categories.Visuals;
using static Elixir.Mods.Categories.World;
using static Elixir.Mods.Categories.SherbertClass;
using static Elixir.Utilities.Variables;
using Elixir.Mods.Categories;

namespace Elixir.Management
{
    public class Buttons
    {
        public static void CreateButtons()
        {
            categories.Add(new Category("Settings", Category.IconType.Setting, new Module[] {
            //new Module() { title = "Change Sound", tooltip = "Cycles through available menu sound profiles.", isToggleable = false, action = () => ChangeSound() },
            new Module() { title = "Toggle Tooltips", tooltip = "Toggles when tooltips should show.", isToggleable = true, toggled = true, action = () => Settings.ToggleTips(true), disableAction = () => ToggleTips(false) },
            new Module() { title = "Toggle Disconnect Button", tooltip = "Shows or hides the disconnect button.", isToggleable = true, toggled = true, action = () => ToggleDisconnect(true), disableAction = () => ToggleDisconnect(false) },
            new Module() { title = "Toggle Version Counter", tooltip = "Shows or hides the on-screen version counter.", isToggleable = true, toggled = true, action = () => ToggleVCounter(true), disableAction = () => ToggleVCounter(false) },
            new Module() { title = "Sort Buttons Alphabetically", tooltip = "Sorts buttons in alphabetical order when enabled.", isToggleable = true, toggled = false, action = () => Alphabet(true), disableAction = () => Alphabet(false) },
            new Module() { title = "Visualize Antireport", tooltip = "Displays a visual indicator for anti-report features.", isToggleable = true, toggled = false, action = () => VisReport(true), disableAction = () => VisReport(false) },
            new Module() { title = "Change ESP Color", tooltip = "Opens color selector for 'Infection' ESP elements.", isToggleable = false, action = () => ESPChange() },
            new Module() { title = "Change Tracer Position", tooltip = "Switches tracer visuals to the right-side attachment.", isToggleable = false, action = () => TracerPos() },
            new Module() { title = "Change Fly Speed", tooltip = "Cycles through fly speed presets (current: Normal).", isToggleable = false, action = () => FlySpeed() },
            new Module() { title = "Change Speed Boost", tooltip = "Cycles speed boost presets (current: Normal).", isToggleable = false, action = () => SpeedSpeed() },
            new Module() { title = "Change Gun Type", tooltip = "Cycles gun visualization type to 'Ball + Line'.", isToggleable = false, action = () => GunChange() },
            }));

            categories.Add(new Category("Room", Category.IconType.Room, new Module[] {
            new Module() { title = "Quit Game", tooltip = "Exits the current game and returns to the main menu.", isToggleable = true, action = () => QuitGTAG() },
            new Module() { title = "Join Random", tooltip = "Attempts to join a random public room.", isToggleable = false, action = () => JoinRandomPublic() },
            new Module() { title = "Disconnect", tooltip = "Disconnects you from the current server session.", isToggleable = false, action = () => Disconnect() },
            new Module() { title = "Primary Disconnect", tooltip = "Performs the primary disconnect routine.", isToggleable = true, action = () => PrimaryDisconnect() },
            new Module() { title = "Disable Network Triggers", tooltip = "Turns off network-triggered behaviors.", isToggleable = false, action = () => DisableNetworkTriggers() },
            new Module() { title = "Enable Network Triggers", tooltip = "Turns on network-triggered behaviors.", isToggleable = false, action = () => EnableNetworkTriggers() },
            new Module() { title = "Connect To US Servers", tooltip = "Forces connection to the US server cluster.", isToggleable = false, action = () => Servers("us") },
            new Module() { title = "Connect To USW Servers", tooltip = "Forces connection to the US West server cluster.", isToggleable = false, action = () => Servers("usw") },
            new Module() { title = "Connect To EU Servers", tooltip = "Forces connection to the EU server cluster.", isToggleable = false, action = () => Servers("eu") },
            new Module() { title = "Set Mode Paintbrawl", tooltip = "Sets the current game mode to Paintbrawl.", isToggleable = false, action = () => SetGamemode(GameModeType.Paintbrawl) },
            new Module() { title = "Set Mode Hunt", tooltip = "Sets the current game mode to Hunt.", isToggleable = false, action = () => SetGamemode(GameModeType.HuntDown) },
            new Module() { title = "Set Mode Ambush", tooltip = "Sets the current game mode to Ambush.", isToggleable = false, action = () => SetGamemode(GameModeType.Ambush) },
            new Module() { title = "Set Mode Ghost", tooltip = "Sets the current game mode to Ghost.", isToggleable = false, action = () => SetGamemode(GameModeType.Ghost) },
            new Module() { title = "Set Mode Error", tooltip = "Sets the game mode to an 'Error' / test mode.", isToggleable = false, action = () => SetGamemode(GameModeType.Count) },
            new Module() { title = "Join Menu Code", tooltip = "Joins the room with the menu's preset code ($Elixir$).", isToggleable = false, action = () => JoinRoom("$Elixir$") },
            new Module() { title = "Join Code MOD", tooltip = "Joins the room using code 'MOD'.", isToggleable = false, action = () => JoinRoom("MOD") },
            new Module() { title = "Join Code PBBV", tooltip = "Joins the room using code 'PBBV'.", isToggleable = false, action = () => JoinRoom("PBBV") },
            new Module() { title = "Join Code Daisy09", tooltip = "Joins the room using code 'DAISY09'.", isToggleable = false, action = () => JoinRoom("DAISY09") },
            new Module() { title = "Mute Gun", tooltip = "Mutes sounds originating from guns.", isToggleable = true, action = () => MuteGun() },
            new Module() { title = "Mute Everyone", tooltip = "Mutes all other players in the room.", isToggleable = false, action = () => MuteAll() },
            new Module() { title = "Report Gun", tooltip = "Sends a report for the current gun (if applicable).", isToggleable = true, action = () => ReportGun() },
            new Module() { title = "Report Everyone", tooltip = "Sends a report for all players in the room.", isToggleable = false, action = () => ReportAll() },
            new Module() { title = "Copy Self ID", tooltip = "Copies your player ID to the clipboard.", isToggleable = false, action = () => CopySelfID() },
            new Module() { title = "Copy ID Gun", tooltip = "Copies the current gun's ID to the clipboard.", isToggleable = true, action = () => CopyIDGun() },
            new Module() { title = "Anti Report", tooltip = "Activates basic anti-report protections.", isToggleable = true,toggled = true, action = () => AntiReport() },
            new Module() { title = "Strict Anti Report", tooltip = "Enables stricter anti-report behavior.", isToggleable = true, action = () => StrictAntiReport() },
            }));

            categories.Add(new Category("Movement", Category.IconType.Moving, new Module[] {
            new Module() { title = "Platforms [G]", tooltip = "Creates solid platforms beneath your hands while gripping.", isToggleable = true, action = () => Platforms(false, false) },
            new Module() { title = "Invis Platforms [G]", tooltip = "Creates invisible grip platforms for stealth movement.", isToggleable = true, action = () => Platforms(true, false) },
            new Module() { title = "Plank Platforms", tooltip = "Spawns wide plank-like platforms to stand or walk on.", isToggleable = true, action = () => Platforms(false, true) },
            new Module() { title = "Force Tag Freeze", tooltip = "Forces tag freeze effects on nearby targets.", isToggleable = false, action = () => TagFreeze(true) },
            new Module() { title = "No Tag Freeze", tooltip = "Prevents your character from being frozen when tagged.", isToggleable = true, action = () => TagFreeze(false) },
            new Module() { title = "NoClip [T]", tooltip = "Lets you move through objects freely while active.", isToggleable = true, action = () => Noclip() },
            new Module() { title = "Speed Boost", tooltip = "Boosts your movement speed for faster traversal.", isToggleable = true, action = () => Speedboost() },
            new Module() { title = "Grip Speed Boost [G]", tooltip = "Hold grip to activate a short speed boost.", isToggleable = true, action = () => GSpeedboost() },
            new Module() { title = "Dash [P]", tooltip = "Performs a short forward dash when triggered.", isToggleable = true, action = () => DashJump(true) },
            new Module() { title = "Fling [P]", tooltip = "Launches you forward with a powerful fling motion.", isToggleable = true, action = () => DashJump(false) },
            new Module() { title = "Velocity Jump", tooltip = "Increases jump velocity for higher leaps.", isToggleable = true, action = () => Velocity(-1.7f), disableAction = () => Velocity(default) },
            new Module() { title = "Fly [P]", tooltip = "Lets you fly freely around the map using trigger.", isToggleable = true, action = () => Fly() },
            new Module() { title = "Trigger Fly [T]", tooltip = "Fly using trigger activation only.", isToggleable = true, action = () => TriggerFly() },
            new Module() { title = "Super Monke [P]", tooltip = "Enables powerful super-jump and enhanced agility.", isToggleable = true, action = () => SuperMonke() },
            new Module() { title = "Hand Fly [P]", tooltip = "Control flight by moving your hands through the air.", isToggleable = true, action = () => HandFly() },
            new Module() { title = "Slingshot Fly [T]", tooltip = "Fly with slingshot-style physics using trigger.", isToggleable = true, action = () => SlingshotFly() },
            new Module() { title = "Iron Monke [G]", tooltip = "Gain extra weight and strong gravity when gripping.", isToggleable = true, action = () => IronMonkey() },
            new Module() { title = "Punch Mod [BUGGY]", tooltip = "Enables experimental punch interactions.", isToggleable = true, action = () => PunchMod() },
            new Module() { title = "Forward & Backward [T]", tooltip = "Allows directional control forward/backward with trigger.", isToggleable = true, action = () => Carmonkey() },
            new Module() { title = "Up & Down [T]", tooltip = "Move vertically using trigger input.", isToggleable = true, action = () => UpAndDown() },
            new Module() { title = "WASD [PC]", tooltip = "Move your character using keyboard WASD controls.", isToggleable = true, action = () => WASDFly() },
            new Module() { title = "Joystick Fly", tooltip = "Fly using joystick movement controls.", isToggleable = true, action = () => DroneFly() },
            new Module() { title = "No Gravity", tooltip = "Removes gravity effect from your player.", isToggleable = true, action = () => Gravity(9.81f) },
            new Module() { title = "Moon Walk", tooltip = "Simulates moon-like low gravity movement.", isToggleable = true, action = () => Gravity(6.66f) },
            new Module() { title = "Jupiter Walk", tooltip = "Simulates heavy gravity like on Jupiter.", isToggleable = true, action = () => JupiterWalk() },
            new Module() { title = "TP Gun", tooltip = "Teleports you to where your gun projectile lands.", isToggleable = true, action = () => TPGun() },
            new Module() { title = "TP To Player Gun", tooltip = "Teleports you directly to another player using gun aim.", isToggleable = true, action = () => TPPlayerGun() },
            new Module() { title = "Hover Gun", tooltip = "Allows your gun to hover in midair.", isToggleable = true, action = () => HoverGun() },
            new Module() { title = "Check Point [RG, RT, A]", tooltip = "Sets or returns to a saved checkpoint position.", isToggleable = true, action = () => Checkpoint() },
            }));

            categories.Add(new Category("Player", Category.IconType.Person, new Module[] {
            new Module() { title = "Long Arms", tooltip = "Extends your arms slightly for extra reach.", isToggleable = true, action = () => LongArms(1.2f), disableAction = () => FixArms() },
            new Module() { title = "Very Long Arms", tooltip = "Greatly extends arm length for exaggerated reach.", isToggleable = true, action = () => LongArms(2f), disableAction = () => FixArms() },
            new Module() { title = "[EXTREME] Long Arms", tooltip = "Extremely long arms for extreme reach visuals.", isToggleable = true, action = () => LongArms(6f), disableAction = () => FixArms() },
            new Module() { title = "Short Arms", tooltip = "Shrinks your arms to a smaller size.", isToggleable = true, action = () => LongArms(0.5f), disableAction = () => FixArms() },
            new Module() { title = "Upsidedown Head", tooltip = "Flips your head model upside down.", isToggleable = true, action = () => UpsidedownHead(), disableAction = () => FixHead() },
            new Module() { title = "Backwards Head", tooltip = "Rotates your head to face behind you.", isToggleable = true, action = () => BackwardsHead(), disableAction = () => FixHead() },
            new Module() { title = "Snap Neck", tooltip = "Applies a neck-snapping animation effect.", isToggleable = true, action = () => SnapNeck(), disableAction = () => FixHead() },
            new Module() { title = "Invis Monke [P]", tooltip = "Makes your player invisible to others.", isToggleable = true, action = () => InvisibleMonke() },
            new Module() { title = "Ghost Monke [P]", tooltip = "Gives your player a ghost-like appearance.", isToggleable = true, action = () => GhostMonke() },
            new Module() { title = "Head Spin X", tooltip = "Rotates your head continuously around the X axis.", isToggleable = true, action = () => HeadSpinx(), disableAction = () => FixHead() },
            new Module() { title = "Head Spin Y", tooltip = "Rotates your head continuously around the Y axis.", isToggleable = true, action = () => HeadSpiny(), disableAction = () => FixHead() },
            new Module() { title = "Head Spin Z", tooltip = "Rotates your head continuously around the Z axis.", isToggleable = true, action = () => HeadSpinx(), disableAction = () => FixHead() },
            new Module() { title = "Freeze Rig", tooltip = "Freezes your body rig in its current position.", isToggleable = true, action = () => FreezeRig() },
            new Module() { title = "Spin", tooltip = "Spins your character model around continuously.", isToggleable = true, action = () => Spin() },
            new Module() { title = "Ascend", tooltip = "Slowly raises your player upward through the air.", isToggleable = true, action = () => Ascend() },
            new Module() { title = "T Pose", tooltip = "Puts your avatar into a static T-pose.", isToggleable = true, action = () => Tpose() },
            new Module() { title = "Helicopter", tooltip = "Spins your body rapidly like a helicopter.", isToggleable = true, action = () => Helicopter() },
            new Module() { title = "Fake Lag [G]", tooltip = "Simulates lag by freezing and resuming motion randomly.", isToggleable = true, action = () => FakeLag() },
            new Module() { title = "Grab Rig [G]", tooltip = "Allows grabbing and pulling on your rig manually.", isToggleable = true, action = () => GrabRig() },
            new Module() { title = "Spaz Rig", tooltip = "Applies random spaz-like movements to your rig.", isToggleable = true, action = () => Spaz() },
            new Module() { title = "Spaz Hands", tooltip = "Causes your hands to twitch erratically.", isToggleable = true, action = () => SpazHands() },
            new Module() { title = "Annoy Player Gun", tooltip = "Targets another player with an annoying effect gun.", isToggleable = true, action = () => AnnoyPlayerGun() },
            new Module() { title = "Annoy Self", tooltip = "Applies random annoying effects to yourself.", isToggleable = true, action = () => AnnoySelf() },
            new Module() { title = "Orbit Player Gun", tooltip = "Makes a gun orbit around another player.", isToggleable = true, action = () => OrbitPGun() },
            new Module() { title = "Orbit Gun", tooltip = "Makes your gun spin in an orbital path.", isToggleable = true, action = () => OrbitGun() },
            new Module() { title = "Orbit Self", tooltip = "Rotates your player around an invisible axis.", isToggleable = true, action = () => OrbitSelf() },
            new Module() { title = "Bees [G]", tooltip = "Spawns bees that follow or interact with players.", isToggleable = true, action = () => Bees() },
            new Module() { title = "Fast Bees [G]", tooltip = "Spawns faster-moving bees for chaos.", isToggleable = true, action = () => FBees() },
            new Module() { title = "Chase Gun", tooltip = "Makes a gun chase players automatically.", isToggleable = true, action = () => ChaseGun() },
            new Module() { title = "Rig Gun", tooltip = "Attaches a gun to the player's rig structure.", isToggleable = true, action = () => RigGun() },
            new Module() { title = "Grab Your Rig Gun", tooltip = "Lets you grab your rig's attached gun.", isToggleable = true, action = () => GrabGun() },
            new Module() { title = "Backshots Gun", tooltip = "Special gun effect that fires backward shots.", isToggleable = true, action = () => SexGun() },
            new Module() { title = "Head Gun", tooltip = "Attaches a gun to your head model.", isToggleable = true, action = () => HeadGun() },
            new Module() { title = "Get Head Gun", tooltip = "Gives you a gun that attaches to your head.", isToggleable = true, action = () => GetHeadGun() },
            new Module() { title = "Tag Gun", tooltip = "Shoots tagging projectiles instead of normal shots.", isToggleable = true, action = () => TagGun() },
            new Module() { title = "Tag Aura [G]", tooltip = "Creates an aura that tags nearby players when gripping.", isToggleable = true, action = () => TagAura() },
            new Module() { title = "Tag All [T]", tooltip = "Tags all players instantly when triggered.", isToggleable = true, action = () => TagAll() },
            new Module() { title = "Tag Self [T]", tooltip = "Tags yourself for quick testing.", isToggleable = true, action = () => TagSelf() },
            new Module() { title = "Max Quest Score", tooltip = "Sets your quest score to a maxed-out value.", isToggleable = false, action = () => QuestScore(99999) },
            new Module() { title = "69420 Quest Score", tooltip = "Sets your quest score to a funny custom value.", isToggleable = false, action = () => QuestScore(69420) },
            }));

            categories.Add(new Category("Visuals", Category.IconType.Visual, new Module[] {
            new Module() { title = "Chams", tooltip = "Highlights players and objects with colored materials.", isToggleable = true, action = () => ESP(), disableAction = () => DisableESP() },
            new Module() { title = "Tracers", tooltip = "Draws visible lines between you and other players.", isToggleable = true, action = () => Tracers() },
            new Module() { title = "Beacons", tooltip = "Displays beacon markers over key targets.", isToggleable = true, action = () => Beacons() },
            new Module() { title = "2D Box ESP", tooltip = "Shows 2D boxes around players for easy identification.", isToggleable = true, action = () => BoxESP(false) },
            new Module() { title = "3D Box ESP", tooltip = "Shows 3D bounding boxes around players.", isToggleable = true, action = () => BoxESP(true) },
            new Module() { title = "3D Wireframe ESP (Alt 1)", tooltip = "Displays entities as 3D wireframes for visibility.", isToggleable = true, action = () => Wireframe(false) },
            new Module() { title = "3D Wireframe ESP (Alt 2)", tooltip = "Alternative wireframe visualization mode.", isToggleable = true, action = () => Wireframe(true) },
            new Module() { title = "CSGO ESP", tooltip = "Activates a CSGO-style enemy highlight system.", isToggleable = true, action = () => CSGO() },
            new Module() { title = "Sphere ESP", tooltip = "Draws spheres around visible entities.", isToggleable = true, action = () => BallESP() },
            new Module() { title = "Bug ESP", tooltip = "Shows ESP outlines for bug entities.", isToggleable = true, action = () => EntityESP(false) },
            new Module() { title = "Bat ESP", tooltip = "Shows ESP outlines for bat entities.", isToggleable = true, action = () => EntityESP(true) },
            new Module() { title = "Distance ESP", tooltip = "Displays distance information beside each tracked target.", isToggleable = true, action = () => DistanceESP() },
            new Module() { title = "Nametags", tooltip = "Shows floating nametags above players’ heads.", isToggleable = true, action = () => Nametags() },
            new Module() { title = "Advanced Nametags", tooltip = "Adds extra information (like ping) to nametags.", isToggleable = true, action = () => AdvNametags() },
            new Module() { title = "VR Info Display", tooltip = "Displays VR performance or device info overlay.", isToggleable = true, action = () => InfoDisplay() },
            new Module() { title = "Snake ESP", tooltip = "Renders a moving snake trail behind players.", isToggleable = true, action = () => SnakeESP() },
            new Module() { title = "Skeleton ESP", tooltip = "Draws skeletal outlines for player avatars.", isToggleable = true, action = () => EnableSkeleton(), disableAction = () => DisableSkeleton() },
            new Module() { title = "Ignore Gun [CS]", tooltip = "Prevents ESP from highlighting held guns.", isToggleable = true, action = () => Ignore() },
            }));

            categories.Add(new Category("Fun", Category.IconType.Fun, new Module[] {
            new Module() { title = "Vibrator", tooltip = "Applies continuous vibration to your hands.", isToggleable = true, action = () => Vibrator() },
            new Module() { title = "Sherbert", tooltip = "Summons or dismisses the Sherbert companion.", isToggleable = true, action = () => Sherbert(), disableAction = () => KillSherbert() },
            new Module() { title = "Grab Bug [G]", tooltip = "Allows grabbing and tossing bug entities.", isToggleable = true, action = () => GrabBug() },
            new Module() { title = "Bug Gun", tooltip = "Spawns a gun that fires bug projectiles.", isToggleable = true, action = () => BugGun() },
            new Module() { title = "Snipe Bug [G]", tooltip = "Lets you aim and shoot long-range bug attacks.", isToggleable = true, action = () => SnipeBug() },
            new Module() { title = "Bug Halo", tooltip = "Creates a rotating halo of bugs around you.", isToggleable = true, action = () => BugHalo() },
            new Module() { title = "Grab Bat [G]", tooltip = "Lets you grab and throw bat entities.", isToggleable = true, action = () => GrabBat() },
            new Module() { title = "Bat Gun", tooltip = "Spawns a gun that fires bats instead of bullets.", isToggleable = true, action = () => BatGun() },
            new Module() { title = "Snipe Bat [G]", tooltip = "Enables long-range bat attacks using your gun.", isToggleable = true, action = () => SnipeBat() },
            new Module() { title = "Bat Halo", tooltip = "Creates a halo of bats orbiting your player.", isToggleable = true, action = () => BatHalo() },
            new Module() { title = "Grab Soccer Ball [G]", tooltip = "Lets you grab and kick a soccer ball in the world.", isToggleable = true, action = () => GrabSBall() },
            new Module() { title = "Soccer Ball Gun", tooltip = "Shoots soccer balls from your gun.", isToggleable = true, action = () => SBallGun() },
        }));

            // --- SPLASH MODS ---
            categories.Add(new Category("Splash Mods", Category.IconType.Splash, new Module[] {
            new Module() { title = "Splash Hands [G]", tooltip = "Creates water splash effects from your hands.", isToggleable = true, action = () => SplashHands() },
            new Module() { title = "Splash Gun", tooltip = "Spawns a gun that shoots splash effects.", isToggleable = true, action = () => SplashGun() },
            new Module() { title = "Splash Aura [G]", tooltip = "Creates a watery aura effect around you.", isToggleable = true, action = () => SplashAura() },
            new Module() { title = "Give Splash Gun", tooltip = "Gives you a splash gun to use.", isToggleable = true, action = () => GiveSplash() },
            new Module() { title = "Schitzo Gun V1", tooltip = "Activates Schitzo Gun variant 1 with chaotic visuals.", isToggleable = true, action = () => SchitzoV1() },
            new Module() { title = "Schitzo Gun V2", tooltip = "Activates Schitzo Gun variant 2 (enhanced version).", isToggleable = true, action = () => SchitzoV2() },
        }));

            // --- CS MODS ---
            categories.Add(new Category("CS Mods", Category.IconType.Person, new Module[] {
            new Module() { title = "Draw (CS)", tooltip = "Draws orb patterns around you.", isToggleable = true, action = () => Draw() },
            new Module() { title = "Orb Spam", tooltip = "Rapidly spawns multiple orbs in the air.", isToggleable = true, action = () => GravDraw() },
            new Module() { title = "Orb Launcher", tooltip = "Launches orbs repeatedly from your gun.", isToggleable = true, action = () => Spam1() },
            new Module() { title = "Tracer Orb Launcher", tooltip = "Fires tracer-style orbs that leave glowing trails.", isToggleable = true, action = () => Spam2() },
            new Module() { title = "No Grav Orb Launcher", tooltip = "Spawns orbs that float weightlessly.", isToggleable = true, action = () => Spam3() },
            new Module() { title = "Big Orb Spam", tooltip = "Creates oversized orbs that move slowly.", isToggleable = true, action = () => BigSpam() },
            new Module() { title = "Spaz Orb", tooltip = "Spawns orbs with erratic spaz-like motion.", isToggleable = true, action = () => SpazOrb() },
            new Module() { title = "Gun Orb", tooltip = "Shoots orbs directly from your gun.", isToggleable = true, action = () => OrbGun() },
            new Module() { title = "Big Gun Orb", tooltip = "Shoots large orbs from your gun.", isToggleable = true, action = () => OrbGun1() },
            new Module() { title = "Orb Rain", tooltip = "Causes orbs to rain down from above.", isToggleable = true, action = () => OrbRain() },
            new Module() { title = "Orb Rain Trace", tooltip = "Creates tracer lines for raining orbs.", isToggleable = true, action = () => OrbRain1() },
            }));

            categories.Add(new Category("World", Category.IconType.World, new Module[] {
            new Module() { title = "Stump Text", tooltip = "Toggles special 'stump' text in the world.", isToggleable = true, toggled = true, action = () => Stumpy(), disableAction = () => STUMPY() },
            new Module() { title = "Unlock Comp", tooltip = "Unlocks world-based components or content.", isToggleable = true, action = () => UnlockComp() },
            new Module() { title = "Enable I Lava You Update", tooltip = "Enables the 'I Lava You' world update.", isToggleable = true, action = () => EnableILavaYou(), disableAction = () => DisableILavaYou() },
            new Module() { title = "Enable Rain", tooltip = "Toggles rain weather effects in the world.", isToggleable = true, action = () => Rain(), disableAction = () => Rain1() },
            new Module() { title = "Enable Sniw", tooltip = "Toggles snow weather effects in forest.", isToggleable = true, action = () => ToggleSnow(true), disableAction = () => ToggleSnow(false) },
            new Module() { title = "Change Time Night", tooltip = "Sets world time to night.", isToggleable = false, action = () => NightTimeMod() },
            new Module() { title = "Change Time Day", tooltip = "Sets world time to day.", isToggleable = false, action = () => idkTimeMod() },
            new Module() { title = "Enable Shadows", tooltip = "Enables or disables world shadow rendering.", isToggleable = true, action = () => Shadows(true), disableAction = () => Shadows(false) },
            }));

            categories.Add(new Category("Credits", Category.IconType.Info, new Module[] {
            new Module() { title = "Menu Owners", tooltip = "", isToggleable = false, action = () => Placeholder() },
            new Module() { title = "Cha554", tooltip = "", isToggleable = false, action = () => Placeholder() },
            new Module() { title = "GLXY", tooltip = "", isToggleable = false, action = () => Placeholder() },
            new Module() { title = "Fwog", tooltip = "", isToggleable = false, action = () => Placeholder() },
            new Module() { title = "Discord Link", tooltip = "", isToggleable = false, action = () => Discord() },
            }));
        }
    }
}
