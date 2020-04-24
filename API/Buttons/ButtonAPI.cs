using Il2CppSystem.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Notorious.API
{
    public static class ButtonAPI
    {
        public static Transform InstantiateGameobject(string type)
        {
            var quickMenu = Wrappers.GetQuickMenu();
            var VrcUIManager = Wrappers.GetVRCUiManager();
            switch (type)
            {
                default:
                    return Object.Instantiate<GameObject>(Wrappers.GetVRCUiPageManager().transform.Find("MenuContent/Screens/Settings/AudioDevicePanel/LevelText").gameObject).transform;
                case "back":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("CameraMenu/BackButton").gameObject).transform;
                case "nameplates":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("UIElementsMenu/ToggleNameplatesButton").gameObject).transform;
                case "block1":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("NotificationInteractMenu/BlockButton").gameObject).transform;
                case "next":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/NextArrow_Button").gameObject).transform;
                case "prev":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Selected/PreviousArrow_Button").gameObject).transform;
                case "emojimenu":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("EmojiMenu").gameObject).transform;
                case "userinteractmenu":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("UserInteractMenu").gameObject).transform;
                case "block":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("UserInteractMenu/BlockButton").gameObject).transform;
                case "menu":
                    return Object.Instantiate<GameObject>(quickMenu.transform.Find("CameraMenu").gameObject).transform;
                case "favorite":
                    return Object.Instantiate<GameObject>(VrcUIManager.transform.Find("MenuContent/Screens/Avatar/ButtonCreate").gameObject).transform;
            }
        }
        private static string ConvertToType(this ButtonType type)
        {
            switch(type)
            {
                case ButtonType.Default:
                    return "back";
                case ButtonType.Toggle:
                    return "block";
                case ButtonType.Half:
                    return "back";
            }

            return "block";
        }
        public static GameObject CreateButton(ButtonType type, string text, string tooltip, Color textColor, Color backgroundColor, float x_pos, float y_pos, Transform parent, Action listener, Action SecondListener = null)
        {
            Transform transform = InstantiateGameobject(type.ConvertToType());
            var quickMenu = Wrappers.GetQuickMenu();

            float num = quickMenu.transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - quickMenu.transform.Find("UserInteractMenu/BanButton").localPosition.x;
            float num2 = quickMenu.transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - quickMenu.transform.Find("UserInteractMenu/BanButton").localPosition.x;

            transform.localPosition = new Vector3(transform.localPosition.x + num * x_pos, transform.localPosition.y + num2 * y_pos, transform.localPosition.z);

            transform.SetParent(parent, false);

            switch(type)
            {
                case ButtonType.Toggle:
                    var EnableButton = transform.Find("Toggle_States_Visible/ON").gameObject;
                    var DisableButton = transform.Find("Toggle_States_Visible/OFF").gameObject;

                    EnableButton.GetComponentsInChildren<Text>()[0].text = $"{text}\nON";
                    DisableButton.GetComponentsInChildren<Text>()[0].text = $"{text}\nON";
                    var fontSize = EnableButton.GetComponentsInChildren<Text>()[0].fontSize;

                    EnableButton.GetComponentsInChildren<Text>()[1].text = $"{text}\nOFF";
                    DisableButton.GetComponentsInChildren<Text>()[1].text = $"{text}\nOFF";

                    EnableButton.GetComponentInChildren<Image>().color = backgroundColor;
                    DisableButton.GetComponentInChildren<Image>().color = backgroundColor;

                    transform.transform.GetComponent<UiTooltip>().text = tooltip;
                    transform.transform.GetComponent<UiTooltip>().alternateText = tooltip;

                    transform.transform.GetComponent<UiTooltip>().SetToolTipBasedOnToggle();

                    transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

                    DisableButton.SetActive(true);
                    EnableButton.SetActive(false);

                    transform.GetComponent<Button>().onClick.AddListener(new Action(() =>
                    {
                        if (EnableButton.activeSelf)
                        {
                            SecondListener.Invoke();
                            EnableButton.SetActive(false);
                            DisableButton.SetActive(true);
                        }
                        else
                        {
                            listener.Invoke();
                            DisableButton.SetActive(false);
                            EnableButton.SetActive(true);
                        }
                    }));
                    break;
                case ButtonType.Default:
                    transform.GetComponentInChildren<Text>().text = text;
                    transform.GetComponentInChildren<UiTooltip>().text = tooltip;
                    transform.GetComponentInChildren<Text>().color = textColor;
                    transform.GetComponentInChildren<Image>().color = backgroundColor;

                    transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                    transform.GetComponent<Button>().onClick.AddListener(listener);
                    break;
                case ButtonType.Half:
                    transform.localScale += new Vector3(0, 0.2f, 0);
                    transform.GetComponentInChildren<Text>().text = text;
                    transform.GetComponentInChildren<UiTooltip>().text = tooltip;
                    transform.GetComponentInChildren<Text>().color = textColor;
                    transform.GetComponentInChildren<Image>().color = backgroundColor;

                    transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                    transform.GetComponent<Button>().onClick.AddListener(listener);
                    break;
            }

            return transform.gameObject;
        }

        public static Transform CreateMenuNestedButton(string text, string tooltip, Color textColor, Color backgroundColor, float x_pos, float y_pos, Transform parent)
        {
            var quickMenu = Wrappers.GetQuickMenu();
            Transform menu = InstantiateGameobject("menu");
            menu.name = $"MENU_INDEX_{x_pos}_{y_pos}";

            CreateButton(ButtonType.Default, text, tooltip, textColor, backgroundColor, x_pos, y_pos, parent, new Action(() =>
            {
                ButtonAPI.ShowCustomMenu(menu.name);
            }));

            IEnumerator enumerator = menu.transform.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                Transform btnEnum = (Transform)obj;
                if (btnEnum != null)
                {
                    UnityEngine.Object.Destroy(btnEnum.gameObject);
                }
            }

            CreateButton(ButtonType.Default, "Back", "Go Back to the previous menu", Color.cyan, Color.white, 4, 2, menu, new Action(() => { ButtonAPI.ShowCustomMenu($"MENU_INDEX_0_0"); }));

            return menu;
        }

        private static FieldInfo currentPageGetter;
        private static FieldInfo quickmenuContextualDisplayGetter;

        public static void ShowCustomMenu(string index)
        {
            var quickMenu = Wrappers.GetQuickMenu();
            var Menu = quickMenu.transform.Find(index);

            if (Menu == null)
            {
                GameObject gameObject2 = (GameObject)currentPageGetter.GetValue(quickMenu);
                gameObject2.SetActive(true);
                quickMenu.transform.Find("QuickMenu_NewElements/_InfoBar").gameObject.SetActive(true);
            }
            else
            {
                GameObject gameObject = quickMenu.transform.Find("ShortcutMenu").gameObject;
                FieldInfo[] array = (from fi in typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                     where fi.FieldType == typeof(GameObject)
                                     select fi).ToArray<FieldInfo>();
                int num = 0;
                foreach (FieldInfo fieldInfo in array)
                {
                    if (fieldInfo.GetValue(quickMenu) as GameObject == gameObject && ++num == 2)
                    {
                        currentPageGetter = fieldInfo;
                        break;
                    }
                }

                if (currentPageGetter != null)
                {
                    GameObject Active = (GameObject)currentPageGetter.GetValue(quickMenu);
                    Active.SetActive(false);
                    quickMenu.transform.Find("QuickMenu_NewElements/_InfoBar").gameObject.SetActive(false);
                    quickmenuContextualDisplayGetter = typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault((FieldInfo fi) => fi.FieldType == typeof(QuickMenuContextualDisplay));
                    FieldInfo fieldInfo2 = quickmenuContextualDisplayGetter;
                    if (fieldInfo2 == null) return;
                    QuickMenuContextualDisplay quickMenuContextualDisplay = ((fieldInfo2 != null) ? fieldInfo2.GetValue(quickMenu) : null) as QuickMenuContextualDisplay;
                    if (quickMenuContextualDisplay == null) return;
                    currentPageGetter.SetValue(quickMenu, Menu.gameObject);
                    MethodBase method = typeof(QuickMenuContextualDisplay).GetMethod("SetDefaultContext", BindingFlags.Instance | BindingFlags.Public);
                    object obj = quickMenuContextualDisplay;
                    object[] array3 = new object[3];
                    array3[0] = 0;
                    method.Invoke(obj, array3);

                    currentPageGetter.SetValue(quickMenu, Menu.gameObject);
                    MethodBase method2 = typeof(QuickMenu).GetMethod("SetContext", BindingFlags.Instance | BindingFlags.Public);
                    object obj2 = quickMenu;
                    object[] array4 = new object[3];
                    array4[0] = 1;
                    method2.Invoke(obj2, array4);
                    Menu.gameObject.SetActive(true);
                }
            }

            // THIS IS CREDITS TO DUBYADUDE HERE
        }
    }
}
