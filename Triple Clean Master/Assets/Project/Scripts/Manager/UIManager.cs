using System;
using System.Collections.Generic;
using Project.Scripts.UI;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.Manager
{
    public enum ScreenType { HomeScreen, PlayScreen, Revive, NextScreen, Shop, StatusBar, Setting, PopupEditTheme }
    public enum PopupType {  Revive, Setting, PopupEditTheme }
    public enum CommonUIType {  StatusBar }

    



    [Serializable]
    public class ScreenConfig
    {
        public ScreenType screenType;
        public string path;
    }
    [Serializable]
    public class PopupConfig
    {
        public PopupType popupType;
        public string path;
    }
    [Serializable]
    public class CommonUIConfig
    {
        public CommonUIType commonUIType;
        public string path;
    }
    

    public class UIManager : Singleton<UIManager>
    {
        [Header("Roots")]
        [SerializeField] private Transform screenRoot;
        [SerializeField] private Transform popupRoot;
        [SerializeField] private Transform commonRoot;
        
        //public List<ScreenConfig> uiConfigs = new();
        
        public List<ScreenConfig> screenConfigs = new();
        public List<PopupConfig> popupConfigs = new();
        public List<CommonUIConfig> commonUIConfigs = new();

        private const string FolderScreen = "UI/Screen/";
        private const string FolderPopup = "UI/Popup/";
        private const string FolderCommon = "UI/Common/";


        private static readonly Dictionary<ScreenType, string> ScreenPaths = new();
        private static readonly Dictionary<PopupType, string> PopupPaths = new();
        private static readonly Dictionary<CommonUIType, string> CommonPaths = new();

        private static readonly Dictionary<ScreenType, UIScreen> ScreenCache = new();
        private static readonly Dictionary<PopupType, UIPopup> PopupCache = new();
        private static readonly Dictionary<CommonUIType, UICommon> CommonCache = new();

        private static readonly Dictionary<ScreenType, int> OrderScreen = new();
        private readonly List<UIPopup> _popupStack = new();

        private ScreenType? _currentScreen;


        private void Awake()
        {
            InitConfig();
        }
        

        private void InitConfig()
        {
            ScreenPaths.Clear();
            PopupPaths.Clear();
            CommonPaths.Clear();

            OrderScreen.Clear();

            for (int i = 0; i < screenConfigs.Count; i++)
            {
                ScreenPaths[screenConfigs[i].screenType] = screenConfigs[i].path;
                OrderScreen[screenConfigs[i].screenType] = i;
            }
            for (int i = 0; i < popupConfigs.Count; i++)
            {
                PopupPaths[popupConfigs[i].popupType] = popupConfigs[i].path;
            }
            for (int i = 0; i < commonUIConfigs.Count; i++)
            {
                CommonPaths[commonUIConfigs[i].commonUIType] = commonUIConfigs[i].path;
            }
        }

        public static T OpenUI<T>(ScreenType screenType) where T : UIScreen
        {
            var ui = GetUI<T>(screenType);
            if (ui == null) return null;
            if(!ui.gameObject.activeSelf) ui.Open();
            return ui;
        }

        public static T OpenScreen<T>(ScreenType screenType) where T : UIScreen
        {
            if (Instance._currentScreen.HasValue && Instance._currentScreen.Value != screenType)
            {
                Instance.CloseScreen(Instance._currentScreen.Value);
            }

            T screen = Instance.GetScreen<T>(screenType);
            if (screen == null)
            {
                return null;
            }
            screen.Open();
            Instance._currentScreen = screenType;
            return screen;
        }
        public static T OpenPopup<T>(PopupType type) where T : UIPopup
        {
            var instance = Instance;

            T popup = GetPopup<T>(type);
            if (popup == null) return null;

            popup.Open();
            popup.transform.SetAsLastSibling();

            instance._popupStack.Remove(popup);
            instance._popupStack.Add(popup);

            return popup;
        }
        public static T OpenCommon<T>(CommonUIType type) where T : UICommon
        {
            var instance = Instance;

            T common = GetCommon<T>(type);
            if (common == null) return null;

            common.Open();
            return common;
        }
        public static void ClosePopup(UIPopup popup)
        {
            if (popup == null) return;

            Instance._popupStack.Remove(popup);
            popup.Close();
        }
        public static void ClosePopup(PopupType type)
        {
            var instance = Instance;

            if (PopupCache.TryGetValue(type, out var popup))
                return;

            ClosePopup(popup);
        }
        public static void CloseTopPopup()
        {
            var instance = Instance;

            if (instance._popupStack.Count == 0)
                return;

            var topPopup = instance._popupStack[^1];
            ClosePopup(topPopup);
        }
        public static void CloseAllPopups()
        {
            var instance = Instance;

            for (int i = instance._popupStack.Count - 1; i >= 0; i--)
            {
                instance._popupStack[i].Close();
            }

            instance._popupStack.Clear();
        }
        private void CloseScreen(ScreenType type)
        {
            if (!ScreenCache.TryGetValue(type, out var screen))
                return;

            screen.Close();
        }
        private T GetScreen<T>(ScreenType type) where T : UIScreen
        {
            if (ScreenCache.TryGetValue(type, out var cached))
                return cached as T;

            var screen = LoadUI<T, ScreenType>(type, ScreenPaths, FolderScreen, screenRoot);
            if (screen == null) return null;

            ScreenCache[type] = screen;
            return screen;
        }
        public static T GetPopup<T>(PopupType type) where T : UIPopup
        {
            if (PopupCache.TryGetValue(type, out var cached))
                return cached as T;

            var popup = LoadUI<T, PopupType>(type, PopupPaths, FolderPopup, Instance.popupRoot);
            if (popup == null) return null;

            PopupCache[type] = popup;
            return popup;
        }
        public static T GetCommon<T>(CommonUIType type) where T : UICommon
        {
            if (CommonCache.TryGetValue(type, out var cached))
                return cached as T;

            var common = LoadUI<T, CommonUIType>(type, CommonPaths, FolderCommon,Instance.commonRoot);
            if (common == null) return null;

            CommonCache[type] = common;
            return common;
        }
        private static T LoadUI<T, TKey>(TKey type, Dictionary<TKey, string> paths,string folder, Transform root) where T : UIBase
        {
            if (!paths.TryGetValue(type, out var path))
            {
                Debug.LogError($"[UIManager] No path config for {type}");
                return null;
            }

            var prefab = Resources.Load<GameObject>(folder + path);

            if (prefab == null)
            {
                Debug.LogError($"[UIManager] Cannot load prefab at path: UI Screen/{path}");
                return null;
            }

            var go = Instantiate(prefab, root);
            var ui = go.GetComponent<T>();

            if (ui == null)
            {
                Debug.LogError($"[UIManager] Prefab {type} missing component {typeof(T).Name}");
                Destroy(go);
                return null;
            }

            ui.gameObject.SetActive(false);
            return ui;
        }
       
        private static void SortLoadedUIByConfig()
        {
            var list = new List<KeyValuePair<ScreenType, UIScreen>>(ScreenCache);

            list.Sort((a, b) =>
            {
                int orderA = OrderScreen.TryGetValue(a.Key, out var indexA) ? indexA : int.MaxValue;
                int orderB = OrderScreen.TryGetValue(b.Key, out var indexB) ? indexB : int.MaxValue;

                return orderA.CompareTo(orderB);
            });

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Value == null) continue;
                list[i].Value.transform.SetSiblingIndex(i);
            }
        }
        public static void CloseUI<T>(ScreenType screenType) where T : UIScreen
        {
            if (ScreenCache.TryGetValue(screenType, out var ui) && IsOpened(screenType))
            {
                ui.Close();
            }
        }

        private static bool IsOpened(ScreenType screenType)
        {
            return IsLoaded(screenType) && ScreenCache[screenType].gameObject.activeInHierarchy;
        }

        private static bool IsLoaded(ScreenType screenType)
        {
            return ScreenCache.ContainsKey(screenType) && ScreenCache[screenType] != null;
        }

        public static T GetUI<T>(ScreenType screenType) where T : UIScreen
        {
            if (ScreenCache.TryGetValue(screenType, out var ui))
            {
                return ui as T;
            }   
            
            if (!ScreenCache.TryGetValue(screenType, out var path))
            {
                Debug.LogError($"[UIManager] no path for {screenType}");
                return null;
            }

            
            GameObject prefab = Resources.Load<GameObject>("UI Screen/" + path);

            if (prefab == null )
            {
                Debug.LogError($"[UIManager] cannot load prefab at: {path}");
                return null;
            }

            
            GameObject go = Instantiate(prefab, Instance.transform);

            var screen = go.GetComponent<T>();

            if (screen == null)
            {
                Debug.LogError($"Prefab {screenType} no component {typeof(T)}");
                return null;
            }

            ScreenCache[screenType] = screen;

            SortLoadedUIByConfig();

            return screen;


        }

        private static readonly List<UICanvas> BackUI = new();

        private static UICanvas BackTopUI
        {
            get
            {
                UICanvas uiCanvas = null;
                if (BackUI.Count > 0) uiCanvas = BackUI[^1];
                return uiCanvas;
            }
        }

        public static void RemoveBackUI(UICanvas canvas)
        {
            BackUI.Remove(canvas);
        }

       
    }
}