using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Carum.Inventory
{
    public class CategoryItem : MonoBehaviour
    {
        public TMP_Text tmp;
        public InventoryController inventoryController;
        private FurnitureCategory category;
        private string text;
        void Awake()
        {
            int idx = transform.GetSiblingIndex();
            category = (FurnitureCategory)idx;
            switch (category)
            {
                case FurnitureCategory.TOY:
                    text = "장난감";
                    break;
                case FurnitureCategory.WALL:
                    text = "벽";
                    break;
                case FurnitureCategory.FOOD:
                    text = "음식";
                    break;
                case FurnitureCategory.CHAIR_TABLE:
                    text = "책걸상";
                    break;
                case FurnitureCategory.LIFE:
                    text = "일상";
                    break;
                case FurnitureCategory.BOX:
                    text = "상자";
                    break;
                case FurnitureCategory.LAUNDRY_BATH:
                    text = "욕실";
                    break;
                case FurnitureCategory.BED:
                    text = "침실";
                    break;
                case FurnitureCategory.ANIMAL_PLANT:
                    text = "동식물";
                    break;
                case FurnitureCategory.FLOOR:
                    text = "바닥";
                    break;
                case FurnitureCategory.MUSIC:
                    text = "음악";
                    break;
                case FurnitureCategory.KITCHEN:
                    text = "주방";
                    break;
                case FurnitureCategory.ELECTRIC:
                    text = "전자";
                    break;
                case FurnitureCategory.OUTSIDE:
                    text = "야외";
                    break;
                case FurnitureCategory.CLOTH:
                    text = "의류";
                    break;
                case FurnitureCategory.STAND:
                    text = "스탠드";
                    break;
            }
            
            // ReSharper disable once Unity.NoNullPropagation
            tmp?.SetText(text);
        }

        public void OpenItemList()
        {
            inventoryController.ToggleCategory(false);
            inventoryController.ToggleItemList(category.ToString(),text,true);
        }
    }

    public enum FurnitureCategory
    {
        TOY,
        WALL,
        FOOD,
        CHAIR_TABLE,
        LIFE,
        BOX,
        LAUNDRY_BATH,
        BED,
        ANIMAL_PLANT,
        FLOOR,
        MUSIC,
        KITCHEN,
        ELECTRIC,
        OUTSIDE,
        CLOTH,
        STAND
    }
}