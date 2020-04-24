using Il2CppSystem.Collections;
using NET_SDK;
using Notorious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QoL.API
{
    public class QMButtonBase
    {
        protected GameObject button;
        protected string btnQMLoc;
        protected string btnType;
        protected string btnTag;
        protected int[] initShift = { 0, 0 };

        public GameObject getGameObject()
        {
            return button;
        }

        public void setActive(bool isActive)
        {
            button.gameObject.SetActive(isActive);
        }

        public void setLocation(int buttonXLoc, int buttonYLoc)
        {
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (420 * (buttonXLoc + initShift[0]));
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.down * (420 * (buttonYLoc + initShift[1]));

            btnTag = "(" + buttonXLoc + "," + buttonYLoc + ")";
            button.name = btnQMLoc + "/" + btnType + btnTag;
            button.GetComponent<Button>().name = btnType + btnTag;
        }

        public void setToolTip(string buttonToolTip)
        {
            button.GetComponent<UiTooltip>().text = buttonToolTip;
            button.GetComponent<UiTooltip>().alternateText = buttonToolTip;
        }
    }

    public class QMSingleButton : QMButtonBase
    {

        public QMSingleButton(QMNestedButton btnMenu, int btnXLocation, int btnYLocation, String btnText, UnityAction btnAction, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            btnQMLoc = btnMenu.getMenuName();
            btnType = "SingleButton";
            initButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, btnBackgroundColor, btnTextColor);
        }

        public QMSingleButton(string btnMenu, int btnXLocation, int btnYLocation, String btnText, UnityAction btnAction, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            btnQMLoc = btnMenu;
            btnType = "SingleButton";
            initButton(btnXLocation, btnYLocation, btnText, btnAction, btnToolTip, btnBackgroundColor, btnTextColor);
        }


        private void initButton(int btnXLocation, int btnYLocation, String btnText, UnityAction btnAction, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            Transform btnTemplate = null;
            btnTemplate = QuickMenuStuff.GetQuickMenuInstance().transform.Find("ShortcutMenu/WorldsButton");

            button = UnityEngine.Object.Instantiate<GameObject>(btnTemplate.gameObject, QuickMenuStuff.GetQuickMenuInstance().transform.Find(btnQMLoc), true);

            initShift[0] = -1;
            initShift[1] = 0;
            setLocation(btnXLocation, btnYLocation);
            setButtonText(btnText);
            setToolTip(btnToolTip);
            setAction(btnAction);

            if (btnBackgroundColor != null)
                setBackgroundColor((Color)btnBackgroundColor);
            if (btnTextColor != null)
                setTextColor((Color)btnTextColor);

            setActive(true);
        }

        public void setButtonText(string buttonText)
        {
            button.GetComponentInChildren<Text>().text = buttonText;
        }

        public void setAction(UnityAction buttonAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            button.GetComponent<Button>().onClick.AddListener(buttonAction);
        }

        public void setBackgroundColor(Color buttonBackgroundColor)
        {
            button.GetComponentInChildren<UnityEngine.UI.Image>().color = buttonBackgroundColor;
        }

        public void setTextColor(Color buttonTextColor)
        {
            button.GetComponentInChildren<Text>().color = buttonTextColor;
        }
    }

    public class QMToggleButton : QMButtonBase
    {

        public GameObject btnOn;
        public GameObject btnOff;


        public QMToggleButton(QMNestedButton btnMenu, int btnXLocation, int btnYLocation, String btnTextOn, UnityAction btnActionOn, String btnTextOff, UnityAction btnActionOff, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            btnQMLoc = btnMenu.getMenuName();
            btnType = "ToggleButton";
            initButton(btnXLocation, btnYLocation, btnTextOn, btnActionOn, btnTextOff, btnActionOff, btnToolTip, btnBackgroundColor, btnTextColor);
        }

        public QMToggleButton(string btnMenu, int btnXLocation, int btnYLocation, String btnTextOn, UnityAction btnActionOn, String btnTextOff, UnityAction btnActionOff, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            btnQMLoc = btnMenu;
            btnType = "ToggleButton";
            initButton(btnXLocation, btnYLocation, btnTextOn, btnActionOn, btnTextOff, btnActionOff, btnToolTip, btnBackgroundColor, btnTextColor);
        }

        private void initButton(int btnXLocation, int btnYLocation, String btnTextOn, UnityAction btnActionOn, String btnTextOff, UnityAction btnActionOff, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            Transform btnTemplate = null;
            btnTemplate = QuickMenuStuff.GetQuickMenuInstance().transform.Find("UserInteractMenu/BlockButton");

            button = UnityEngine.Object.Instantiate<GameObject>(btnTemplate.gameObject, QuickMenuStuff.GetQuickMenuInstance().transform.Find(btnQMLoc), true);

            btnOn = button.transform.Find("Toggle_States_Visible/ON").gameObject;
            btnOff = button.transform.Find("Toggle_States_Visible/OFF").gameObject;

            initShift[0] = -4;
            initShift[1] = 0;
            setLocation(btnXLocation, btnYLocation);

            setOnText(btnTextOn);
            setOffText(btnTextOff);
            Text[] btnTextsOn = btnOn.GetComponentsInChildren<Text>();
            btnTextsOn[0].name = "Text_ON";
            btnTextsOn[1].name = "Text_OFF";
            Text[] btnTextsOff = btnOff.GetComponentsInChildren<Text>();
            btnTextsOff[0].name = "Text_ON";
            btnTextsOff[1].name = "Text_OFF";

            setToolTip(btnToolTip);
            button.transform.GetComponentInChildren<UiTooltip>().SetToolTipBasedOnToggle();

            setAction(btnActionOn, btnActionOff);
            btnOn.SetActive(false);
            btnOff.SetActive(true);

            if (btnBackgroundColor != null)
                setBackgroundColor((Color)btnBackgroundColor);

            if (btnTextColor != null)
                setTextColor((Color)btnTextColor);

            setActive(true);

        }

        public void setBackgroundColor(Color buttonBackgroundColor)
        {
            UnityEngine.UI.Image[] btnBgColorList = ((btnOn.GetComponentsInChildren<UnityEngine.UI.Image>()).Concat(btnOff.GetComponentsInChildren<UnityEngine.UI.Image>()).ToArray()).Concat(button.GetComponentsInChildren<UnityEngine.UI.Image>()).ToArray();
            foreach (UnityEngine.UI.Image btnBackground in btnBgColorList) btnBackground.color = buttonBackgroundColor;
        }

        public void setTextColor(Color buttonTextColor)
        {
            Text[] btnTxtColorList = (btnOn.GetComponentsInChildren<Text>()).Concat(btnOff.GetComponentsInChildren<Text>()).ToArray();
            foreach (Text btnText in btnTxtColorList) btnText.color = buttonTextColor;
        }

        public void setAction(UnityAction buttonOnAction, UnityAction buttonOffAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            button.GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                if (btnOn.activeSelf)
                {
                    buttonOffAction.Invoke();
                    btnOn.SetActive(false);
                    btnOff.SetActive(true);
                }
                else
                {
                    buttonOnAction.Invoke();
                    btnOff.SetActive(false);
                    btnOn.SetActive(true);
                }
            }));
        }

        public void setOnText(string buttonOnText)
        {
            Text[] btnTextsOn = btnOn.GetComponentsInChildren<Text>();
            btnTextsOn[0].text = buttonOnText;
            Text[] btnTextsOff = btnOff.GetComponentsInChildren<Text>();
            btnTextsOff[0].text = buttonOnText;
        }

        public void setOffText(string buttonOffText)
        {
            Text[] btnTextsOn = btnOn.GetComponentsInChildren<Text>();
            btnTextsOn[1].text = buttonOffText;
            Text[] btnTextsOff = btnOff.GetComponentsInChildren<Text>();
            btnTextsOff[1].text = buttonOffText;
        }
    }

    public class QMNestedButton
    {
        protected QMSingleButton mainButton;
        protected QMSingleButton backButton;
        protected string menuName;
        protected string btnQMLoc;
        protected string btnType;

        public QMNestedButton(QMNestedButton btnMenu, int btnXLocation, int btnYLocation, String btnText, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null, Nullable<Color> backbtnBackgroundColor = null, Nullable<Color> backbtnTextColor = null)
        {
            btnQMLoc = btnMenu.getMenuName();
            btnType = "NestedButton";
            initButton(btnXLocation, btnYLocation, btnText, btnToolTip, btnBackgroundColor, btnTextColor, backbtnBackgroundColor, backbtnTextColor);
        }

        public QMNestedButton(string btnMenu, int btnXLocation, int btnYLocation, String btnText, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null, Nullable<Color> backbtnBackgroundColor = null, Nullable<Color> backbtnTextColor = null)
        {
            btnQMLoc = btnMenu;
            btnType = "NestedButton";
            initButton(btnXLocation, btnYLocation, btnText, btnToolTip, btnBackgroundColor, btnTextColor, backbtnBackgroundColor, backbtnTextColor);
        }

        public void initButton(int btnXLocation, int btnYLocation, String btnText, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null, Nullable<Color> backbtnBackgroundColor = null, Nullable<Color> backbtnTextColor = null)
        {
            Transform menu = UnityEngine.Object.Instantiate<Transform>(QuickMenuStuff.GetQuickMenuInstance().transform.Find("CameraMenu"), QuickMenuStuff.GetQuickMenuInstance().transform);
            menuName = "CustomMenu" + btnQMLoc + "_" + btnXLocation + "_" + btnYLocation;
            menu.name = menuName;

            mainButton = new QMSingleButton(btnQMLoc, btnXLocation, btnYLocation, btnText, new Action(() => { QuickMenuStuff.ShowQuickmenuPage(menuName); }), btnToolTip, btnBackgroundColor, btnTextColor);

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

            if (backbtnTextColor == null)
            {
                backbtnTextColor = Color.yellow;
            }
            backButton = new QMSingleButton(this, 4, 2, "Back", new Action(() => { QuickMenuStuff.ShowQuickmenuPage(btnQMLoc); }), "Go Back", backbtnBackgroundColor, backbtnTextColor);
        }

        public string getMenuName()
        {
            return menuName;
        }

        public QMSingleButton getMainButton()
        {
            return mainButton;
        }

        public QMSingleButton getBackButton()
        {
            return backButton;
        }
    }

    public class QuickMenuStuff : MonoBehaviour
    {
        // <3 VRCTools
        private static VRCUiManager vrcuimInstance = Wrappers.GetVRCUiManager();
        public static QuickMenu GetQuickMenuInstance()
        {
            return Wrappers.GetQuickMenu();
        }

        private static FieldInfo currentPageGetter;
        private static FieldInfo quickmenuContextualDisplayGetter;
        public static void ShowQuickmenuPage(string pagename)
        {
            QuickMenu quickMenuInstance = GetQuickMenuInstance();
            Transform transform = (quickMenuInstance != null) ? quickMenuInstance.transform.Find(pagename) : null;
            if (transform == null)
            {
                Console.WriteLine("[QuickMenuUtils] pageTransform is null !");
            }
            if (currentPageGetter == null)
            {
                if (currentPageGetter == null)
                {
                    GameObject gameObject = quickMenuInstance.transform.Find("ShortcutMenu").gameObject;
                    FieldInfo[] array = (from fi in typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic) where fi.FieldType == typeof(GameObject) select fi).ToArray<FieldInfo>();
                    int num = 0;
                    foreach (FieldInfo fieldInfo in array)
                    {
                        if (fieldInfo.GetValue(quickMenuInstance) as GameObject == gameObject && ++num == 2)
                        {
                            currentPageGetter = fieldInfo;
                            break;
                        }
                    }
                }

                if (currentPageGetter == null)
                {
                    GameObject gameObject = quickMenuInstance.transform.Find("UserInteractMenu").gameObject;
                    FieldInfo[] array = (from fi in typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic) where fi.FieldType == typeof(GameObject) select fi).ToArray<FieldInfo>();
                    int num = 0;
                    foreach (FieldInfo fieldInfo in array)
                    {
                        if (fieldInfo.GetValue(quickMenuInstance) as GameObject == gameObject && ++num == 2)
                        {
                            currentPageGetter = fieldInfo;
                            break;
                        }
                    }
                }

                if (currentPageGetter == null)
                {
                    Console.WriteLine("[QuickMenuUtils] Unable to find field currentPage in QuickMenu");
                    return;
                }
            }
            GameObject gameObject2 = (GameObject)currentPageGetter.GetValue(quickMenuInstance);
            if (gameObject2 != null)
            {
                gameObject2.SetActive(false);
            }
            GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar").gameObject.SetActive(false);
            if (quickmenuContextualDisplayGetter != null)
            {
                quickmenuContextualDisplayGetter = typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault((FieldInfo fi) => fi.FieldType == typeof(QuickMenuContextualDisplay));
            }
            FieldInfo fieldInfo2 = quickmenuContextualDisplayGetter;
            QuickMenuContextualDisplay quickMenuContextualDisplay = ((fieldInfo2 != null) ? fieldInfo2.GetValue(quickMenuInstance) : null) as QuickMenuContextualDisplay;
            if (quickMenuContextualDisplay != null)
            {
                currentPageGetter.SetValue(quickMenuInstance, transform.gameObject);
                quickMenuContextualDisplay.Method_Public_Nested0_0(QuickMenuContextualDisplay.Nested0.NoSelection);
            }
            currentPageGetter.SetValue(quickMenuInstance, transform.gameObject);
            quickMenuContextualDisplay.Method_Public_Nested0_0(QuickMenuContextualDisplay.Nested0.NoSelection);
            transform.gameObject.SetActive(true);
        }

        public VRCUiPage GetPage(string path)
        {
            GameObject gameObject = GameObject.Find(path);
            VRCUiPage vrcuiPage = null;
            if (gameObject != null)
            {
                vrcuiPage = gameObject.GetComponent<VRCUiPage>();
                if (vrcuiPage == null)
                {
                    UnityEngine.Debug.LogError(new Il2CppSystem.Object(IL2CPP.StringToIntPtr("Screen Not Found - " + path)));
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning(new Il2CppSystem.Object(IL2CPP.StringToIntPtr("Screen Not Found - " + path)));
            }
            return vrcuiPage;
        }
    }


}
