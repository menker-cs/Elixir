using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

namespace Elixir.Utilities
{
    public class ColorLib
    {
        #region Solid
        public static Color Elixir = new Color(0.10f, 0.10f, 0.10f, 1f);
        public static Color Menker = new Color(0.44f, 0.99f, 0.95f, 1f);
        public static Color Menker2 = new Color(0.16f, 0.54f, 0.51f, 1f);

        // Reds
        public static Color Red = new Color(1.00f, 0.00f, 0.00f, 1f);
        public static Color DarkRed = new Color(0.71f, 0.00f, 0.00f, 1f);
        public static Color Salmon = new Color(0.98f, 0.50f, 0.45f, 1f);
        public static Color WineRed = new Color(0.48f, 0.00f, 0.00f, 1f);
        public static Color IndianRed = new Color(0.80f, 0.36f, 0.36f, 1f);
        public static Color Crimson = new Color(0.86f, 0.08f, 0.24f, 1f);
        public static Color FireBrick = new Color(0.70f, 0.13f, 0.13f, 1f);
        public static Color Coral = new Color(1.00f, 0.50f, 0.31f, 1f);
        public static Color DarkCoral = new Color(0.92f, 0.42f, 0.24f, 1f);
        public static Color Tomato = new Color(1.00f, 0.39f, 0.28f, 1f);
        public static Color Maroon = new Color(0.50f, 0.00f, 0.00f, 1f);

        // Greens
        public static Color Green = new Color(0.00f, 1.00f, 0.00f, 1f);
        public static Color Lime = new Color(0.00f, 0.50f, 0.00f, 1f);
        public static Color DarkGreen = new Color(0.00f, 0.39f, 0.00f, 1f);
        public static Color Olive = new Color(0.50f, 0.50f, 0.00f, 1f);
        public static Color ForestGreen = new Color(0.13f, 0.55f, 0.13f, 1f);
        public static Color SeaGreen = new Color(0.18f, 0.55f, 0.34f, 1f);
        public static Color MediumSeaGreen = new Color(0.24f, 0.70f, 0.44f, 1f);
        public static Color Aquamarine = new Color(0.50f, 1.00f, 0.83f, 1f);
        public static Color MediumAquamarine = new Color(0.40f, 0.80f, 0.67f, 1f);
        public static Color DarkSeaGreen = new Color(0.56f, 0.74f, 0.56f, 1f);

        // Blues
        public static Color Blue = new Color(0.00f, 0.00f, 1.00f, 1f);
        public static Color Navy = new Color(0.00f, 0.00f, 0.50f, 1f);
        public static Color DarkBlue = new Color(0.00f, 0.00f, 0.63f, 1f);
        public static Color RoyalBlue = new Color(0.25f, 0.41f, 0.88f, 1f);
        public static Color DodgerBlue = new Color(0.12f, 0.56f, 1.00f, 1f);
        public static Color DarkDodgerBlue = new Color(0.03f, 0.35f, 0.69f, 1f);
        public static Color DeepSkyBlue = new Color(0.00f, 0.75f, 1.00f, 1f);
        public static Color SkyBlue = new Color(0.53f, 0.81f, 0.92f, 1f);
        public static Color SteelBlue = new Color(0.27f, 0.51f, 0.71f, 1f);
        public static Color Cyan = new Color(0.00f, 1.00f, 1.00f, 1f);

        // Yellows
        public static Color Yellow = new Color(1.00f, 1.00f, 0.00f, 1f);
        public static Color Gold = new Color(1.00f, 0.84f, 0.00f, 1f);
        public static Color LightYellow = new Color(1.00f, 1.00f, 0.88f, 1f);
        public static Color LemonChiffon = new Color(1.00f, 0.98f, 0.80f, 1f);
        public static Color Khaki = new Color(0.94f, 0.90f, 0.55f, 1f);
        public static Color PaleGoldenrod = new Color(0.93f, 0.91f, 0.67f, 1f);
        public static Color LightGoldenrodYellow = new Color(0.98f, 0.98f, 0.82f, 1f);

        // Oranges
        public static Color Orange = new Color(1.00f, 0.65f, 0.00f, 1f);
        public static Color DarkOrange = new Color(1.00f, 0.55f, 0.00f, 1f);
        public static Color RedOrange = new Color(1.00f, 0.27f, 0.00f, 1f);
        public static Color PeachPuff = new Color(1.00f, 0.85f, 0.73f, 1f);
        public static Color DarkGoldenrod = new Color(0.72f, 0.53f, 0.04f, 1f);
        public static Color Peru = new Color(0.80f, 0.52f, 0.25f, 1f);
        public static Color OrangeRed = new Color(1.00f, 0.27f, 0.00f, 1f);

        // Purples
        public static Color Magenta = new Color(1.00f, 0.00f, 1.00f, 1f);
        public static Color Purple = new Color(0.48f, 0.01f, 0.99f, 1f);
        public static Color DarkPurple = new Color(0.15f, 0.09f, 0.30f, 1f);
        public static Color Lavender = new Color(0.90f, 0.90f, 0.98f, 1f);
        public static Color Plum = new Color(0.87f, 0.63f, 0.87f, 1f);
        public static Color Indigo = new Color(0.29f, 0.00f, 0.51f, 1f);
        public static Color MediumOrchid = new Color(0.73f, 0.33f, 0.83f, 1f);
        public static Color SlateBlue = new Color(0.42f, 0.35f, 0.80f, 1f);
        public static Color DarkSlateBlue = new Color(0.28f, 0.24f, 0.55f, 1f);

        // Pinks
        public static Color Pink = new Color(1.00f, 0.75f, 0.80f, 1f);
        public static Color LightSalmon = new Color(1.00f, 0.63f, 0.48f, 1f);
        public static Color DarkSalmon = new Color(0.91f, 0.59f, 0.48f, 1f);
        public static Color LightCoral = new Color(0.94f, 0.50f, 0.50f, 1f);
        public static Color MistyRose = new Color(1.00f, 0.89f, 0.88f, 1f);
        public static Color HotPink = new Color(1.00f, 0.41f, 0.71f, 1f);
        public static Color DeepPink = new Color(1.00f, 0.08f, 0.58f, 1f);

        // Browns
        public static Color Brown = new Color(0.55f, 0.27f, 0.07f, 1f);
        public static Color RosyBrown = new Color(0.74f, 0.56f, 0.56f, 1f);
        public static Color SaddleBrown = new Color(0.55f, 0.27f, 0.07f, 1f);
        public static Color Sienna = new Color(0.63f, 0.32f, 0.18f, 1f);
        public static Color Chocolate = new Color(0.82f, 0.41f, 0.12f, 1f);
        public static Color SandyBrown = new Color(0.96f, 0.64f, 0.38f, 1f);
        public static Color DarkSandyBrown = new Color(0.88f, 0.56f, 0.30f, 1f);
        public static Color BurlyWood = new Color(0.87f, 0.72f, 0.53f, 1f);
        public static Color Tan = new Color(0.82f, 0.71f, 0.55f, 1f);

        // Whites
        public static Color White = new Color(1.00f, 1.00f, 1.00f, 1f);
        public static Color Linen = new Color(0.98f, 0.94f, 0.90f, 1f);
        public static Color OldLace = new Color(0.99f, 0.96f, 0.90f, 1f);
        public static Color SeaShell = new Color(1.00f, 0.96f, 0.93f, 1f);
        public static Color MintCream = new Color(0.96f, 1.00f, 0.98f, 1f);

        // Blacks and Grays
        public static Color Black = new Color(0.00f, 0.00f, 0.00f, 1f);
        public static Color Grey = new Color(0.50f, 0.50f, 0.50f, 1f);
        public static Color LightGrey = new Color(0.75f, 0.75f, 0.75f, 1f);
        public static Color DarkGrey = new Color(0.31f, 0.31f, 0.31f, 1f);
        public static Color DarkerGrey = new Color(0.16f, 0.16f, 0.16f, 1f);
#endregion

        #region Transparent
        // Reds
        public static Color RedTransparent = new Color(1.00f, 0.00f, 0.00f, 0.31f);
        public static Color DarkRedTransparent = new Color(0.71f, 0.00f, 0.00f, 0.31f);
        public static Color SalmonTransparent = new Color(0.98f, 0.50f, 0.45f, 0.31f);
        public static Color IndianRedTransparent = new Color(0.80f, 0.36f, 0.36f, 0.31f);
        public static Color CrimsonTransparent = new Color(0.86f, 0.08f, 0.24f, 0.31f);
        public static Color WineRedTransparent = new Color(0.48f, 0.00f, 0.00f, 0.31f);
        public static Color FireBrickTransparent = new Color(0.70f, 0.13f, 0.13f, 0.31f);
        public static Color CoralTransparent = new Color(1.00f, 0.50f, 0.31f, 0.31f);
        public static Color TomatoTransparent = new Color(1.00f, 0.39f, 0.28f, 0.31f);
        public static Color MaroonTransparent = new Color(0.50f, 0.00f, 0.00f, 0.31f);

        // Greens
        public static Color GreenTransparent = new Color(0.00f, 1.00f, 0.00f, 0.31f);
        public static Color LimeTransparent = new Color(0.00f, 0.50f, 0.00f, 0.31f);
        public static Color DarkGreenTransparent = new Color(0.00f, 0.39f, 0.00f, 0.31f);
        public static Color OliveTransparent = new Color(0.50f, 0.50f, 0.00f, 0.31f);
        public static Color ForestGreenTransparent = new Color(0.13f, 0.55f, 0.13f, 0.31f);
        public static Color SeaGreenTransparent = new Color(0.18f, 0.55f, 0.34f, 0.31f);
        public static Color MediumSeaGreenTransparent = new Color(0.24f, 0.70f, 0.44f, 0.31f);
        public static Color AquamarineTransparent = new Color(0.50f, 1.00f, 0.83f, 0.31f);
        public static Color MediumAquamarineTransparent = new Color(0.40f, 0.80f, 0.67f, 0.31f);
        public static Color DarkSeaGreenTransparent = new Color(0.56f, 0.74f, 0.56f, 0.31f);

        // Blues
        public static Color BlueTransparent = new Color(0.00f, 0.00f, 1.00f, 0.31f);
        public static Color NavyTransparent = new Color(0.00f, 0.00f, 0.50f, 0.31f);
        public static Color DarkBlueTransparent = new Color(0.00f, 0.00f, 0.55f, 0.31f);
        public static Color RoyalBlueTransparent = new Color(0.25f, 0.41f, 0.88f, 0.31f);
        public static Color DodgerBlueTransparent = new Color(0.12f, 0.56f, 1.00f, 0.31f);
        public static Color DarkDodgerBlueTransparent = new Color(0.03f, 0.35f, 0.69f, 0.31f);
        public static Color DeepSkyBlueTransparent = new Color(0.00f, 0.75f, 1.00f, 0.31f);
        public static Color SkyBlueTransparent = new Color(0.53f, 0.81f, 0.92f, 0.31f);
        public static Color SteelBlueTransparent = new Color(0.27f, 0.51f, 0.71f, 0.31f);
        public static Color CyanTransparent = new Color(0.00f, 1.00f, 1.00f, 0.31f);

        // Yellows
        public static Color YellowTransparent = new Color(1.00f, 1.00f, 0.00f, 0.31f);
        public static Color GoldTransparent = new Color(1.00f, 0.84f, 0.00f, 0.31f);
        public static Color LightYellowTransparent = new Color(1.00f, 1.00f, 0.88f, 0.31f);
        public static Color LemonChiffonTransparent = new Color(1.00f, 0.98f, 0.80f, 0.31f);
        public static Color KhakiTransparent = new Color(0.94f, 0.90f, 0.55f, 0.31f);
        public static Color PaleGoldenrodTransparent = new Color(0.93f, 0.91f, 0.67f, 0.31f);
        public static Color LightGoldenrodYellowTransparent = new Color(0.98f, 0.98f, 0.82f, 0.31f);

        // Oranges
        public static Color OrangeTransparent = new Color(1.00f, 0.65f, 0.00f, 0.31f);
        public static Color DarkOrangeTransparent = new Color(1.00f, 0.55f, 0.00f, 0.31f);
        public static Color RedOrangeTransparent = new Color(1.00f, 0.27f, 0.00f, 0.31f);
        public static Color PeachPuffTransparent = new Color(1.00f, 0.85f, 0.73f, 0.31f);
        public static Color DarkGoldenrodTransparent = new Color(0.72f, 0.53f, 0.04f, 0.31f);
        public static Color PeruTransparent = new Color(0.80f, 0.52f, 0.25f, 0.31f);
        public static Color OrangeRedTransparent = new Color(1.00f, 0.27f, 0.00f, 0.31f);

        // Purples
        public static Color MagentaTransparent = new Color(1.00f, 0.00f, 1.00f, 0.31f);
        public static Color PurpleTransparent = new Color(0.48f, 0.01f, 0.99f, 0.31f);
        public static Color LavenderTransparent = new Color(0.90f, 0.90f, 0.98f, 0.31f);
        public static Color PlumTransparent = new Color(0.87f, 0.63f, 0.87f, 0.31f);
        public static Color IndigoTransparent = new Color(0.29f, 0.00f, 0.51f, 0.31f);
        public static Color MediumOrchidTransparent = new Color(0.73f, 0.33f, 0.83f, 0.31f);
        public static Color SlateBlueTransparent = new Color(0.42f, 0.35f, 0.80f, 0.31f);
        public static Color DarkSlateBlueTransparent = new Color(0.28f, 0.24f, 0.55f, 0.31f);

        // Pinks
        public static Color PinkTransparent = new Color(1.00f, 0.75f, 0.80f, 0.31f);
        public static Color LightSalmonTransparent = new Color(1.00f, 0.63f, 0.48f, 0.31f);
        public static Color DarkSalmonTransparent = new Color(0.91f, 0.59f, 0.48f, 0.31f);
        public static Color LightCoralTransparent = new Color(0.94f, 0.50f, 0.50f, 0.31f);
        public static Color MistyRoseTransparent = new Color(1.00f, 0.89f, 0.88f, 0.31f);
        public static Color HotPinkTransparent = new Color(1.00f, 0.41f, 0.71f, 0.31f);
        public static Color DeepPinkTransparent = new Color(1.00f, 0.08f, 0.58f, 0.31f);

        // Browns
        public static Color BrownTransparent = new Color(0.65f, 0.16f, 0.16f, 0.31f);
        public static Color RosyBrownTransparent = new Color(0.74f, 0.56f, 0.56f, 0.31f);
        public static Color SaddleBrownTransparent = new Color(0.55f, 0.27f, 0.07f, 0.31f);
        public static Color SiennaTransparent = new Color(0.63f, 0.32f, 0.18f, 0.31f);
        public static Color ChocolateTransparent = new Color(0.82f, 0.41f, 0.12f, 0.31f);
        public static Color SandyBrownTransparent = new Color(0.96f, 0.64f, 0.38f, 0.31f);
        public static Color BurlyWoodTransparent = new Color(0.87f, 0.72f, 0.53f, 0.31f);
        public static Color TanTransparent = new Color(0.82f, 0.71f, 0.55f, 0.31f);

        // Whites
        public static Color WhiteTransparent = new Color(1.00f, 1.00f, 1.00f, 0.31f);
        public static Color LightWhiteTransparent = new Color(1.00f, 1.00f, 1.00f, 0.04f);
        public static Color LinenTransparent = new Color(0.98f, 0.94f, 0.90f, 0.31f);
        public static Color OldLaceTransparent = new Color(0.99f, 0.96f, 0.90f, 0.31f);
        public static Color SeaShellTransparent = new Color(1.00f, 0.96f, 0.93f, 0.31f);
        public static Color MintCreamTransparent = new Color(0.96f, 1.00f, 0.98f, 0.31f);

        // Blacks and Grays
        public static Color BlackTransparent = new Color(0.00f, 0.00f, 0.00f, 0.31f);
        public static Color GreyTransparent = new Color(0.31f, 0.31f, 0.31f, 0.31f);
        public static Color LightGreyTransparent = new Color(0.75f, 0.75f, 0.75f, 0.31f);
        public static Color DarkGreyTransparent = new Color(0.16f, 0.16f, 0.16f, 0.31f);
        public static Color DarkerGreyTransparent = new Color(0.16f, 0.16f, 0.16f, 0.31f);
        #endregion

        #region Shaders
        public static Shader guiShader = Shader.Find("GUI/Text Shader");
        public static Shader uberShader = Shader.Find("GorillaTag/UberShader");
        public static Shader defaultShader = Shader.Find("UI/Default");
        #endregion

        #region Changing Color
        public static Material RGB = new Material(uberShader);
        public static Material DFade = new Material(uberShader);
        public static Material DFade1 = new Material(uberShader);
        public static Material DBreath = new Material(uberShader);
        public static Material BlueFade = new Material(uberShader);
        public static Material Outline = new Material(uberShader);
        public static string hexColor = "#" + ColorUtility.ToHtmlStringRGB(RGB.color);
        public static string hexColor1 = "#" + ColorUtility.ToHtmlStringRGB(DFade.color);
        public static string Menker1 = "#" + ColorUtility.ToHtmlStringRGB(Menker);
        #endregion

        #region Weird Color Stuff
        public static void UpdateClr()
        {
            float num = Mathf.PingPong(Time.time * 0.3f, 1f);
            float num2 = 0.75f;
            RGB.color = Color.HSVToRGB(num, 1f, num2);
            hexColor = "#" + ColorUtility.ToHtmlStringRGB(RGB.color);
            hexColor1 = "#" + ColorUtility.ToHtmlStringRGB(DFade.color);
            Menker1 = "#" + ColorUtility.ToHtmlStringRGB(Menker);
            DFade.color = Color.Lerp(ColorLib.Purple, ColorLib.Indigo, Mathf.PingPong(Time.time, 1f));
            Outline.color = Color.Lerp(new Color32(76, 0, 192, 255), new Color32(101, 0, 255, 255), Mathf.PingPong(Time.time, 1f));
            DBreath.color = Color.Lerp(ColorLib.Purple, ColorLib.Indigo, Mathf.PingPong(Time.time, 1.5f));
        }
        public static Material Color2Mat(Color color)
        {
            return new Material(uberShader)
            {
                color = color
            };
        }
        public static Material Url2Mat(string url)
        {
            byte[] imageData;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                response = httpClient.GetAsync(url).Result;     
                imageData = response.Content.ReadAsByteArrayAsync().Result;
            }

            var texture = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture, imageData);
            texture.Apply();

            var material = new Material(Shader.Find("GorillaTag/UberShader"))
            {
                shaderKeywords = new[] { "_USE_TEXTURE" },
                mainTexture = texture
            };
            return material;
        }
        public static string ClrToHex(Color c)
        {
            Color32 c32 = c;
            return $"{c32.r:X2}{c32.g:X2}{c32.b:X2}";
        }
        #endregion

        public static class GradientText
        {
            public static string MakeGradient(string hexStart, string hexEnd, string text)
            {
                Color start = HexToColor(hexStart);
                Color end = HexToColor(hexEnd);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int len = text.Length;
                for (int i = 0; i < len; i++)
                {
                    float t = (float)i / Mathf.Max(len - 1, 1);
                    Color c = Color.Lerp(start, end, t);
                    sb.Append($"<color=#{ClrToHex(c)}>{text[i]}</color>");
                }
                return sb.ToString();
            }
            public static string MakeAnimatedGradient(string hexStart, string hexEnd, string text, float time, float speed = 1f)
            {
                Color start = HexToColor(hexStart);
                Color end = HexToColor(hexEnd);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int len = text.Length;
                float offset = (time * speed) % 1f;

                for (int i = 0; i < len; i++)
                {
                    float t = ((float)i / Mathf.Max(len - 1, 1) - offset) % 1f;
                    if (t < 0) t += 1f;
                    Color c = Color.Lerp(start, end, t);
                    sb.Append($"<color=#{ClrToHex(c)}>{text[i]}</color>");
                }
                return sb.ToString();
            }
            public static Color HexToColor(string hex)
            {
                if (hex.StartsWith("#")) hex = hex.Substring(1);
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                return new Color32(r, g, b, 255);
            }
        }

        public static Material fmby = Url2Mat("https://i.ebayimg.com/images/g/XI8AAOSwwvRlMHkz/s-l1200.jpg");
        public static Material SherbMat = Url2Mat("https://raw.githubusercontent.com/Cha554/Stone-Networking/main/Stone/Sherbert.jpg");
        public static Material AngrySherbMat = Url2Mat("https://raw.githubusercontent.com/Cha554/Stone-Networking/main/Sherbert(1).jpg");

        #region Menu Colors
        public static Material[] MenuMat = new Material[]
        {
            Color2Mat(DarkPurple),
            DFade,
            DBreath,
            Color2Mat(DarkerGrey),
            Color2Mat(SkyBlue),
            Color2Mat(FireBrick),
            Color2Mat(MediumAquamarine),
            Color2Mat(Elixir),
            Color2Mat(DarkGreen),
            Color2Mat(Tomato),
            Color2Mat(Peru),
            Color2Mat(DarkBlue),
        };
        public static Color[] OutlineClr = new Color[]
        {
            Indigo, 
            Indigo,
            Indigo,
            DarkerGrey,
            DarkDodgerBlue,
            IndianRed,
            Lime,
            DarkerGrey,
            MediumAquamarine,
            DarkSalmon,
            SaddleBrown,
        };
        public static Color[] BtnClrOff = new Color[]
        {
            DarkSlateBlue,
            DarkSlateBlue,
            DarkSlateBlue,
            new Color32(30, 30, 30, 255),
            RoyalBlue,
            WineRed,
            MediumSeaGreen,
            DarkGrey,
            ForestGreen,
            Coral,
            SandyBrown,
        };
        public static Color[] BtnClrOn = new Color[]
        {
            SlateBlue,
            SlateBlue,
            SlateBlue,
            new Color32(30, 30, 30, 255),
            DodgerBlue,
            IndianRed,
            SeaGreen,
            new Color32(35, 35, 35, 255),
            MediumSeaGreen,
            DarkCoral,
            DarkSandyBrown,
        };

        #endregion
    }
}
