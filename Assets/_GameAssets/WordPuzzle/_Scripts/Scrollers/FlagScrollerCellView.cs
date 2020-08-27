using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

public class FlagScrollerCellView : EnhancedScrollerCellView
{
    public void SpawnFlagToCell(int dataIndex)
    {
        ClearAllChild();
        var _dictionaryDialog = DictionaryDialog.instance;
        for (int i = 0; i < 2; i++)
        {
            var index = i + dataIndex * 2;
            FlagItemController flagItem = Instantiate(_dictionaryDialog.flagItemPrefab, transform).GetComponent<FlagItemController>();
            flagItem.indexOfSmallFlagImage = FlagTabController.instance.flagItemList[index].flagSmallImageIndex;
            flagItem.indexOfBigFlagImage = FlagTabController.instance.flagItemList[index].flagBigImageIndex;
            flagItem.flagUnlockWord = FlagTabController.instance.flagItemList[index].flagUnlockWord;

            flagItem.flagName = FlagTabController.instance.flagItemList[index].flagName;
            flagItem.subRegion = FlagTabController.instance.flagItemList[index].subRegion;
            flagItem.capital = FlagTabController.instance.flagItemList[index].capital;
            flagItem.population = FlagTabController.instance.flagItemList[index].population;
            flagItem.area = FlagTabController.instance.flagItemList[index].area;

            string checkWord = flagItem.flagUnlockWord != string.Empty ? flagItem.flagUnlockWord : flagItem.flagName;
            if (FlagTabController.instance.unlockedWordHashset.Contains(checkWord.ToLower()))
            {
                flagItem.isLocked = false;
            }
            else
            {
                flagItem.isLocked = true;
            }

            flagItem.indexOfFlag = i;
        }
    }

    private void ClearAllChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
