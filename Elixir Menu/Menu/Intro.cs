using BepInEx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Elixir.Menu
{
    internal class Intro : MonoBehaviour
    {
        private Texture2D logoTexture;
        private float alpha = 0f;
        private bool showLogo = false;

        void Start()
        {
            StartCoroutine(LoadAndPlay(
                "https://raw.githubusercontent.com/menker-cs/Elixir/refs/heads/main/goop.png"
            ));
        }

        void OnGUI()
        {
            if (showLogo && logoTexture != null)
            {
                Color originalColor = GUI.color;
                GUI.color = new Color(0f, 0f, 0f, alpha * 0.6f);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);

                GUI.color = new Color(1f, 1f, 1f, alpha);
                float width = 300f;
                float height = 300f;
                float x = (Screen.width - width) / 2f;
                float y = (Screen.height - height) / 2f;
                GUI.DrawTexture(new Rect(x, y, width, height), logoTexture);

                GUI.color = originalColor;
            }
        }

        private IEnumerator LoadAndPlay(string imageUrl)
        {

            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return imageRequest.SendWebRequest();
            if (imageRequest.result != UnityWebRequest.Result.Success)
                yield break;

            logoTexture = DownloadHandlerTexture.GetContent(imageRequest);
            showLogo = true;

            float fadeInDuration = 2f;
            float fadeInTime = 0f;
            while (fadeInTime < fadeInDuration)
            {
                fadeInTime += Time.deltaTime;
                alpha = Mathf.Lerp(0f, 1f, fadeInTime / fadeInDuration);
                yield return null;
            }
            alpha = 1f;

            yield return new WaitForSeconds(3f);

            float fadeOutDuration = 2f;
            for (float t = 0f; t < fadeOutDuration; t += Time.deltaTime)
            {
                alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
                yield return null;
            }
            alpha = 0f;
            showLogo = false;
        }
    }
}