using Notorious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QoL.Utils
{
    public static class ColorModule
    {

        //Credits: Meep#0231

        private static IList<Text> CachedText;

        private static IList<Transform> CachedTransforms;

        private static Color CachedColor = new Color32(0x6A, 0xE2, 0xF8, 0xFF);

        private static Color CachedTextColor = new Color32(0x0E, 0xA6, 0xAD, 0xFF);

        private static bool Initialised = false;

        public static void Update()
        {
            if (!Initialised)
            {
                if (Wrappers.GetQuickMenu() != null)
                {
                    CleanupUI();
                    //SetColorTheme(Color.green, Color.white, 0.5f);
                    Initialised = true;
                }
            }
        }
        public static void Start()
        {
            new Thread(() =>
            {
                for(; ;)
                {
                    Thread.Sleep(5000);
                    Update();
                }
            }).Start();
        }
        public static void CleanupUI()
        {
            Color32 textColor = CachedColor;
            GameObject gameObject = GameObject.Find("MenuContent");
            if (ColorModule.CachedTransforms == null) ColorModule.CachedTransforms = gameObject.GetComponentsInChildren<Transform>(true);
            if (ColorModule.CachedText == null) ColorModule.CachedText = gameObject.GetComponentsInChildren<Text>(true);

            gameObject.transform.Find("Screens").Find("Worlds").Find("Vertical Scroll View").GetComponentsInChildren<Text>().ToList<Text>().ForEach(delegate (Text x)
            {
                x.color = textColor;
            });
            gameObject.transform.Find("Screens").Find("Avatar").Find("Vertical Scroll View").GetComponentsInChildren<Text>().ToList<Text>().ForEach(delegate (Text x)
            {
                x.color = textColor;
            });
            for (int i = 0; i < ColorModule.CachedText.Count; i++)
            {
                Color32 color = CachedTextColor;
                Color32 color2 = CachedColor;
                Color clear = Color.clear;
                if (ColorModule.CachedText[i].color == color || ColorModule.CachedText[i].color == color2 || ColorModule.CachedText[i].color == clear || ColorModule.CachedText[i].text == "You Are In" || ColorModule.CachedText[i].text == "Edit Status")
                {
                    ColorModule.CachedText[i].color = textColor;
                }
            }
            for (int j = 0; j < ColorModule.CachedTransforms.Count; j++)
            {
                Transform transform = ColorModule.CachedTransforms[j];
                Text text = (transform != null) ? transform.GetComponent<Text>() : null;
                if (text != null)
                {
                    text.color = textColor;
                }
            }
        }

        public static void SetColorSlider(float colorHue, float colorSaturation, float colorValue, float textColorHue, float textColorSaturation, float textColorValue, float opac)
        {
            Color color = Color.HSVToRGB(colorHue, colorSaturation, colorValue);
            Color textColor = Color.HSVToRGB(textColorHue, textColorSaturation, textColorValue);
            SetColorTheme(color, textColor, opac);
        }

        internal static void ColorOfTypeIfExists(GameObject parent, string contains, Color clr, float opacityToUse, bool useImg)
        {
            clr.a = opacityToUse;
            (from x in parent.GetComponentsInChildren<Transform>(true)
             where x.name.Contains(contains)
             select x).ToList<Transform>().ForEach(delegate (Transform x)
             {
                 if (useImg)
                 {
                     Image image = (x != null) ? x.GetComponent<Image>() : null;
                     if (image != null)
                     {
                         image.color = clr;
                         return;
                     }
                 }
                 else
                 {
                     Text text = (x != null) ? x.GetComponent<Text>() : null;
                     if (text != null)
                     {
                         text.color = clr;
                     }
                 }
             });
            clr.a = 0.5f;
            //Credits: Magic3000
        }


        public static void SetColorTheme(Color color, Color textColor, float opacity)
        {
            CachedColor = color;
            CachedColor.a = opacity;
            CachedTextColor = textColor;
            GameObject gameObject = GameObject.Find("MenuContent");
            gameObject.transform.Find("Screens").Find("UserInfo").Find("User Panel").Find("Panel (1)").GetComponent<Image>().color = CachedColor;
            gameObject.transform.Find("Screens").Find("UserInfo").Find("User Panel").Find("WorldImage").Find("WorldBorder").GetComponent<Image>().color = CachedColor;
            gameObject.transform.Find("Screens").Find("UserInfo").Find("AvatarImage").Find("AvatarBorder").GetComponent<Image>().color = CachedColor;
            gameObject.transform.Find("Screens").Find("WorldInfo").Find("WorldImage").Find("RoomBorder").GetComponent<Image>().color = CachedColor;
            gameObject.transform.Find("Popups").Find("InputPopup").Find("Keyboard").Find("Keys").GetComponentsInChildren<Text>(true).ToList<Text>().ForEach(delegate (Text x)
            {
                x.color = CachedColor;
            });
            gameObject.transform.Find("Popups").Find("InputPopup").Find("InputField").GetComponent<Image>().color = CachedColor;
            Transform transform = gameObject.transform.Find("Popups").Find("InputPopup");
            transform.Find("Rectangle").GetComponent<Image>().color = CachedColor;
            transform.Find("Rectangle").Find("Panel").GetComponent<Image>().color = CachedColor;
            Transform transform2 = gameObject.transform.Find("Backdrop").Find("Header").Find("Tabs").Find("ViewPort").Find("Content").Find("Search");
            transform2.Find("SearchTitle").GetComponent<Text>().color = CachedColor;
            transform2.Find("InputField").GetComponent<Image>().color = CachedColor;
            ColorOfTypeIfExists(gameObject, "Panel_Header", CachedColor, opacity, true);
            ColorOfTypeIfExists(gameObject, "Fill", CachedColor, 0.7f, true);
            ColorOfTypeIfExists(gameObject, "Handle", CachedColor, 1f, true);
            ColorOfTypeIfExists(gameObject, "Background", CachedColor, opacity, true);
            ColorOfTypeIfExists(gameObject, "TitleText", CachedTextColor, 1f, false);
            var theme = new ColorBlock
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = color,
                normalColor = CachedColor,
                pressedColor = Color.white
            };
            gameObject.GetComponentsInChildren<Slider>(true).ToList<Slider>().ForEach(delegate (Slider x)
            {
                x.colors = theme;
            });
            gameObject.GetComponentsInChildren<Button>(true).ToList<Button>().ForEach(delegate (Button x)
            {
                x.colors = theme;
            });
            gameObject.GetComponentsInChildren<Text>(true).ToList<Text>().ForEach(delegate (Text x)
            {
                x.color = CachedTextColor;
            });
            GameObject gameObject2 = GameObject.Find("QuickMenu");
            gameObject2.GetComponentsInChildren<Button>(true).ToList<Button>().ForEach(delegate (Button x)
            {
                x.colors = theme;
            });
            gameObject2.GetComponentsInChildren<Text>(true).ToList<Text>().ForEach(delegate (Text x)
            {
                x.color = CachedTextColor;
            });
            gameObject2.GetComponentsInChildren<UiToggleButton>(true).ToList<UiToggleButton>().ForEach(delegate (UiToggleButton x)
            {
                List<Image> list = x.GetComponentsInChildren<Image>(true).ToList<Image>();
                Action<Image> action = (delegate (Image f)
                {
                    f.color = CachedColor;
                });
                list.ForEach(action);
            });
        }
    }
}
