using BepInEx;
using Elixir.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elixir.Notifications
{
    [BepInPlugin("org.ekixir.menu.notiflib", "Elixir NotifLib", "1.0.0")]
    public class NotificationLib : BaseUnityPlugin
    {
        private static NotificationLib Instance;

        private static Camera mainCamera;
        private static GameObject notifObj;
        private static GameObject HUD;
        private static readonly List<GameObject> activeNotifications = new List<GameObject>();
        private bool initialized;

        private static readonly Queue<string> pendingNotifications = new Queue<string>();
        private static readonly int maxQueueSize = 10;

        private static AssetBundle cachedBundle;

        public static bool disableNotifications = false;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(InitializeAsync());
        }

        private IEnumerator InitializeAsync()
        {
            var bundle = LoadAssetBundle("Elixir.Resources.notifbundle");
            notifObj = bundle.LoadAsset<GameObject>("notif");

            yield return StartCoroutine(WaitForCamera());
        }

        private IEnumerator WaitForCamera()
        {
            int attempts = 0;
            while (Camera.main == null && attempts < 200)
            {
                attempts++;
                yield return new WaitForSeconds(0.1f);
            }

            mainCamera = Camera.main;

            attempts = 0;
            while (GorillaTagger.Instance == null && attempts < 100)
            {
                attempts++;
                yield return new WaitForSeconds(0.1f);
            }

            CreateHUD();
            initialized = true;

            if (pendingNotifications.Count > 0)
            {
                while (pendingNotifications.Count > 0)
                {
                    string message = pendingNotifications.Dequeue();
                    CoroutineHandler.StartCoroutine1(SpawnNotif(message));
                    yield return new WaitForSeconds(0.2f);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        public static AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        private void CreateHUD()
        {
            HUD = new GameObject("HUD");
            Canvas canvas = HUD.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = mainCamera;
            HUD.AddComponent<CanvasScaler>();
            HUD.AddComponent<GraphicRaycaster>();

            RectTransform rect = HUD.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(1000f, 1000f);
            rect.localScale = Vector3.one * 0.01f;

            HUD.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform, false);
            HUD.transform.localPosition = new Vector3(0f, -0.3f, 0.5f);
        }

        private void FixedUpdate()
        {
            if (!initialized || mainCamera == null || HUD == null) return;

            if (GorillaTagger.Instance == null || GorillaTagger.Instance.headCollider == null) return;

            Transform headTransform = GorillaTagger.Instance.headCollider.transform;
            HUD.transform.rotation = Quaternion.LookRotation(
                HUD.transform.position - headTransform.position
            );
        }

        public static void SendNotification(string message)
        {
            if (!Instance.initialized || notifObj == null || HUD == null)
            {
                if (pendingNotifications.Count < maxQueueSize)
                {
                    pendingNotifications.Enqueue(message);
                }
                return;
            }

            CoroutineHandler.StartCoroutine1(SpawnNotif(message));
        }

        static IEnumerator SpawnNotif(string message)
        {
            if (notifObj == null || HUD == null || mainCamera == null)
                yield break;

            GameObject notif = null;
            notif = Instantiate(notifObj, HUD.transform, false);

            notif.transform.localScale = Vector3.one * 2.5f;
            notif.transform.localPosition = Vector3.zero;

            Transform canvas = notif.transform.Find("Canvas");
            if (canvas != null)
            {
                Canvas notifCanvas = canvas.GetComponent<Canvas>();
                if (notifCanvas != null)
                {
                    notifCanvas.worldCamera = mainCamera;
                }
            }

            Transform textTransform = notif.transform.Find("Canvas/Text");
            if (textTransform != null)
            {
                TextMeshProUGUI text = textTransform.GetComponent<TextMeshProUGUI>();
                text.text = message;
            }

            activeNotifications.Add(notif);
            UpdateNotificationPositions();

            float fadeTime = 0.15f;
            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one * 2.5f;

            while (elapsed < fadeTime)
            {
                if (notif == null) yield break;
                notif.transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / fadeTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (notif != null)
            {
                notif.transform.localScale = endScale;
            }

            yield return new WaitForSeconds(2.5f);

            fadeTime = 0.25f;
            elapsed = 0f;
            startScale = Vector3.one * 2.5f;
            endScale = Vector3.zero;

            while (elapsed < fadeTime)
            {
                if (notif == null) yield break;
                notif.transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / fadeTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (activeNotifications.Contains(notif))
                activeNotifications.Remove(notif);

            if (notif != null)
                Destroy(notif);

            UpdateNotificationPositions();
        }

        static void UpdateNotificationPositions()
        {
            const float spacing = 120f;

            activeNotifications.RemoveAll(n => n == null);

            for (int i = 0; i < activeNotifications.Count; i++)
            {
                if (activeNotifications[i] == null) continue;

                RectTransform rect = activeNotifications[i].GetComponent<RectTransform>();
                if (rect == null) continue;

                Vector3 targetPos = new Vector3(0, -i * spacing, 0);
                CoroutineHandler.StartCoroutine1(Move(rect, targetPos, 0.1f));
            }
        }

        static IEnumerator Move(RectTransform rect, Vector3 target, float duration)
        {
            if (rect == null) yield break;

            Vector3 start = rect.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                rect.localPosition = Vector3.Lerp(start, target, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rect.localPosition = target;
        }

        public static void ClearAllNotifications()
        {
            if (Instance == null) return;

            foreach (var notif in activeNotifications)
            {
                if (notif != null) Destroy(notif);
            }

            activeNotifications.Clear();
            pendingNotifications.Clear();
        }

        private void OnDestroy()
        {
            ClearAllNotifications();
            Instance = null;
        }
    }
}