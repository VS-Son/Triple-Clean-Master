using System;
using System.Collections.Generic;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.Manager
{
    public enum TypeScreen { HomeScreen, PlayScreen, Revive, Result, NextScreen, Shop, StatusBar, Setting, PopupEditTheme }
    
    [Serializable]
    public class UIConfig
    {
        public TypeScreen typeScreen;
        public string path;
    }

    public class UIManager : Singleton<UIManager>
    {
        public List<UIConfig> uiConfigs = new();

        private static readonly Dictionary<TypeScreen, string> PathScreen = new();

        private static readonly Dictionary<TypeScreen, UICanvas> Cache = new();

        private static readonly Dictionary<TypeScreen, int> OrderScreen = new();



        private void Awake()
        {
            InitConfig();
        }
        

        private void InitConfig()
        {
            PathScreen.Clear();

            OrderScreen.Clear();

            for (int i = 0; i < uiConfigs.Count; i++)
            {
                var config = uiConfigs[i];

                PathScreen[config.typeScreen] = config.path;
                OrderScreen[config.typeScreen] = i;
            }
        }

        public static T OpenUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            var ui = GetUI<T>(typeScreen);
            if (ui == null) return null;
            if(!ui.gameObject.activeSelf) ui.Open();
            return ui;
        }
        private static void SortLoadedUIByConfig()
        {
            var list = new List<KeyValuePair<TypeScreen, UICanvas>>(Cache);

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
        public static void CloseUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            if (Cache.TryGetValue(typeScreen, out var ui) && IsOpened(typeScreen))
            {
                ui.CloseDirectly();
            }
        }

        private static bool IsOpened(TypeScreen typeScreen)
        {
            return IsLoaded(typeScreen) && Cache[typeScreen].gameObject.activeInHierarchy;
        }

        private static bool IsLoaded(TypeScreen typeScreen)
        {
            return Cache.ContainsKey(typeScreen) && Cache[typeScreen] != null;
        }

        public static T GetUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            if (Cache.TryGetValue(typeScreen, out var ui))
            {
                return ui as T;
            }   
            
            if (!PathScreen.TryGetValue(typeScreen, out var path))
            {
                Debug.LogError($"[UIManager] no path for {typeScreen}");
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
                Debug.LogError($"Prefab {typeScreen} no component {typeof(T)}");
                return null;
            }

            Cache[typeScreen] = screen;

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