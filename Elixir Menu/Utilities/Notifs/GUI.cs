using System;
using System.Collections.Generic;
using BepInEx;
using Hidden.Utilities;
using UnityEngine;

public class Notification
{
    public string Type;
    public string Message;
    public float Time;
    public float Duration;
}

[BepInPlugin("com.glxy.guiNotifs", "GUI Notifications", "0.0.1")]
public class GUINotifs : BaseUnityPlugin
{
    private static List<Notification> notifications = new List<Notification>();
    private static GUIStyle backgroundStyle;
    private static GUIStyle red;
    private static GUIStyle white;
    private static Texture2D blackTex;

    public static void SendNotification(string type, string message, float duration = 5f)
    {
        if (notifications.Count >= 6)
        {
            notifications.RemoveAt(0);
        }

        notifications.Add(new Notification
        {
            Type = type,
            Message = message,
            Time = Time.time,
            Duration = duration
        });
    }

    void OnGUI()
    {
        InitStyles();

        float padding = 10f;
        float width = 400f;
        float height = 40f;
        float yOffset = Screen.height - padding;

        for (int i = notifications.Count - 1; i >= 0; i--)
        {
            Notification n = notifications[i];
            if (Time.time - n.Time > n.Duration)
            {
                notifications.RemoveAt(i);
                continue;
            }
            yOffset -= height;

            Rect boxRect = new Rect(Screen.width - width - padding, yOffset, width, height);
            GUI.Box(boxRect, GUIContent.none, backgroundStyle);

            string msg1 = Trim(n.Message, white, 280);
            string msg2 = Trim(n.Type + ":", red, 280);
            GUI.Label(new Rect(boxRect.x + 10, boxRect.y + 8, 1000, 30), msg2, red);
            GUI.Label(new Rect(boxRect.x + 100, boxRect.y + 8, 1000, 30), msg1, white);

            yOffset -= padding;
        }
    }
    public static Texture2D MakeRoundedTexture(int width, int height, Color col, int radius)
    {
        Texture2D texture2D = new Texture2D(width, height);
        Color[] array = new Color[width * height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                array[i * width + j] = ((j < radius && i < radius && Mathf.Sqrt((float)((j - radius) * (j - radius) + (i - radius) * (i - radius))) > (float)radius) || (j > width - radius && i < radius && Mathf.Sqrt((float)((j - (width - radius)) * (j - (width - radius)) + (i - radius) * (i - radius))) > (float)radius) || (j < radius && i > height - radius && Mathf.Sqrt((float)((j - radius) * (j - radius) + (i - (height - radius)) * (i - (height - radius)))) > (float)radius) || (j > width - radius && i > height - radius && Mathf.Sqrt((float)((j - (width - radius)) * (j - (width - radius)) + (i - (height - radius)) * (i - (height - radius)))) > (float)radius) ? new Color(col.r, col.g, col.b, 0f) : col);
            }
        }
        texture2D.SetPixels(array);
        texture2D.Apply();
        return texture2D;
    }

    public string Trim(string input, GUIStyle style, float maxLength)
    {
        string trimmed = input;
        while (style.CalcSize(new GUIContent(trimmed)).x > maxLength)
        {
            if (trimmed.Length <= 1)
            {
                return "...";
            }
            trimmed = trimmed.Substring(0, trimmed.Length - 1);
        }

        if (trimmed.Length < input.Length)
        {
            trimmed = trimmed.TrimEnd() + "...";
        }

        return trimmed;
    }

    public static Material colorMaterial = new Material(Shader.Find("GUI/Text Shader")) { color = Color.Lerp(Color.gray * 1.3f, Color.grey * 1.6f, Mathf.PingPong(Time.time, 1.5f)) };
    void InitStyles()
    {
        if (backgroundStyle == null)
        {
            blackTex = MakeRoundedTexture(400, 40, ColorLib.Hidden, (int)7.5);

            backgroundStyle = new GUIStyle(GUI.skin.box);
            backgroundStyle.normal.background = blackTex;
            backgroundStyle.border = new RectOffset(12, 12, 12, 12);
        }

        if (red == null)
        {
            red = new GUIStyle(GUI.skin.label);
            red.fontSize = 18;
            red.normal.textColor = colorMaterial.color;
            red.fontStyle = FontStyle.Bold;
        }

        if (white == null)
        {
            white = new GUIStyle(GUI.skin.label);
            white.fontSize = 18;
            white.normal.textColor = Color.white;
            white.fontStyle = FontStyle.Bold;
        }
    }
}
