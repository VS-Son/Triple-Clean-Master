using System.Collections.Generic;
using UnityEngine;
using System;
using Project.Manager;
using UI.Screen;


namespace Project.Services
{
    public enum TypeScreen { HomeScreen, PlayScreen, Revive, Result, Next, Shop, StatusBar }
    
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



        private void Awake()
        {
            InitConfig();
        }
        

        private void InitConfig()
        {
            PathScreen.Clear();

            foreach (var config in uiConfigs)
            {
                PathScreen[config.typeScreen] = config.path;
            }
        }

        public static T OpenUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            var ui = GetUI<T>(typeScreen);
            if (ui == null) return null;
            if(!ui.gameObject.activeSelf) ui.Open();
            return ui;
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