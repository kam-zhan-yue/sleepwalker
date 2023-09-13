using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPopup : Popup
{
    public MainMenuPopup mainMenuPopup;
    public RectTransform itemHolder;
    public LevelSelectPopupItem samplePopupItem;
    public LevelSelectDatabase database;
    
    private List<LevelSelectPopupItem> popupList = new();
    
    protected override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        int totalToSpawn = database.levelSelectDataList.Count - popupList.Count;
        samplePopupItem.gameObject.SetActiveFast(true);
        
        for (int i = 0; i < totalToSpawn; ++i)
        {
            LevelSelectPopupItem item = Instantiate(samplePopupItem, itemHolder);
            popupList.Add(item);
        }
        
        samplePopupItem.gameObject.SetActiveFast(false);
        
        ShowData();
    }

    private void ShowData()
    {
        for (int i = 0; i < popupList.Count; ++i)
        {
            if (i < database.levelSelectDataList.Count)
            {
                popupList[i].Init(database.levelSelectDataList[i]);
                popupList[i].gameObject.SetActiveFast(true);
            }
            else
            {
                popupList[i].gameObject.SetActiveFast(false);
            }
        }
    }

    public override void CloseButtonClicked()
    {
        base.CloseButtonClicked();
        mainMenuPopup.CloseButtonClicked();
    }
}
