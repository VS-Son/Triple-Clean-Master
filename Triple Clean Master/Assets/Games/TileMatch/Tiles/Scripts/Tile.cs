using Games.TileMatch.Tiles.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Games.TileMatch.Tiles.Scripts
{
    public class Tile : TileData, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
        }
    }
}
