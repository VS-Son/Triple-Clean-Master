using System.Collections.Generic;
using UnityEngine;
using System;
using Project.Manager;
using UI.Screen;


namespace Project.Services
{
    public enum TypeScreen { HomeScreen, PlayScreen, Revive, Result, Next, Shop }
    public enum TypeUI { TypeScreen, TypePopup }
    public enum TypePopup {Setting}



    [Serializable]
    public class UIConfig
    {
        public TypeScreen typeScreen;
        public string path;
    }

    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Transform uiRoot;
        public List<UIConfig> uiConfigs = new();

        private readonly Dictionary<TypeScreen, string> _pathScreen = new();

        private readonly Dictionary<TypeScreen, UICanvas> _cache = new();
        



        private void Awake()
        {
            InitConfig();
        }

        private void Start()
        {
            
        }

        private void InitConfig()
        {
            _pathScreen.Clear();

            foreach (var config in uiConfigs)
            {
                _pathScreen[config.typeScreen] = config.path;
            }
        }

        public T OpenUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            var ui = GetUI<T>(typeScreen);
            if (ui == null) return null;
            if(!ui.gameObject.activeSelf) ui.Open();
            return ui;
        }

        public void CloseUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            if (_cache.TryGetValue(typeScreen, out var ui) && IsOpened(typeScreen))
            {
                ui.CloseDirectly();
            }
        }

        private bool IsOpened(TypeScreen typeScreen)
        {
            return IsLoaded(typeScreen) && _cache[typeScreen].gameObject.activeInHierarchy;
        }

        private bool IsLoaded(TypeScreen typeScreen)
        {
            return _cache.ContainsKey(typeScreen) && _cache[typeScreen] != null;
        }

        private T GetUI<T>(TypeScreen typeScreen) where T : UICanvas
        {
            if (_cache.TryGetValue(typeScreen, out var ui))
            {
                return ui as T;
            }   
            
            if (!_pathScreen.TryGetValue(typeScreen, out var path))
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

            
            GameObject go = Instantiate(prefab, uiRoot);


            var screen = go.GetComponent<T>();

            if (screen == null)
            {
                Debug.LogError($"Prefab {typeScreen} no component {typeof(T)}");
                return null;
            }


            _cache[typeScreen] = screen;

            return screen;


        }

        private readonly List<UICanvas> _backUICanvas = new();

        private UICanvas BackTopUI
        {
            get
            {
                UICanvas uiCanvas = null;
                if (_backUICanvas.Count > 0) uiCanvas = _backUICanvas[_backUICanvas.Count - 1];
                return uiCanvas;
            }
        }

        public void RemoveBackUI(UICanvas canvas)
        {
            _backUICanvas.Remove(canvas);
        }

    }
}