
using BepInEx;
using DiscordRPC;
using Oculus.Platform;
using UnityEngine.SocialPlatforms;

namespace MyPlugin
{
    [BepInPlugin("com.cha.ok", "Discord RPC", "1.0.0")]
    public class DiscordRPCPlugin : BaseUnityPlugin
    {
        private DiscordRpcClient client;
        private void Awake()
        {
            client = new DiscordRpcClient("1409977134436323348"); client.Initialize();
            client.SetPresence(new DiscordRPC.RichPresence()
            {
                Details = "Using Elixir Menu",
                State = "",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "8e81f09fe9db62df034af6692022cc95",
                    LargeImageText = ""
                }
            });
        }
        private void Update()
        {
            client.Invoke();
        }
        private void onDestroy()
        {
            client.Dispose();
        }
    }
}