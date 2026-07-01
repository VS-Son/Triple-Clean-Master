using System;
using Project.Scripts.Effect;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public class ShopScreen : UIScreen
    {
        [SerializeField] private TMP_Text undoValue;
        [SerializeField] private TMP_Text magicWandValue;
        [SerializeField] private TMP_Text shuffleValue;
        
        public void UpdateTextUndo(int currentUndo)
        {
            undoValue.text = $"{currentUndo}";
           
        }
        public void UpdateTextMagicWand( int currentMagic)
        {
            magicWandValue.text = $"{currentMagic}";
        }
        public void UpdateTextShuffle( int currentShuffle)
        {
           
            shuffleValue.text = $"{currentShuffle}";
        }
    }
}
