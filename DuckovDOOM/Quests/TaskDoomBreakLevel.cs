using System;
using Duckov.Quests;
using DuckovDOOM.Event;
using DuckovDOOM.Minigame;
using FeatherMod.Events;
using FeatherMod.Saves;
using FeatherMod.Utils;
using UnityEngine;

namespace DuckovDOOM.Quests;

public class TaskDoomBreakLevel : Task
{
    public DoomEventHandler.LevelIndicator targetLevel;
    public string cartidge = String.Empty;
    public string cartidgeTranslateKey = String.Empty;

    public static string LevelFormatter(DoomEventHandler.LevelIndicator indicator)
    {
        if (indicator.episode == 0)
        {
            string basic = String.Format(ModBehaviour.getName("duckovdoom.doom.level2"), indicator.map);
            if (indicator.map >= 31)
            {
                basic = String.Format(ModBehaviour.getName("duckovdoom.doom.hidden"), basic);
            }

            return basic;
        }

        string basic1 = String.Format(ModBehaviour.getName("duckovdoom.doom.level"), indicator.episode, indicator.map);
        if (indicator.map == 9)
        {
            basic1 = String.Format(ModBehaviour.getName("duckovdoom.doom.hidden"), basic1);
        }

        return basic1;
    }
    
    public override string Description =>
        String.Format(
            ModBehaviour.getName("duckovdoom.quest.breaklevel"),
            LevelFormatter(targetLevel),
            ModBehaviour.getName(cartidgeTranslateKey)
        );

    public override string[] ExtraDescriptsions {
        get
        {
            if (ModBehaviour.Instance == null) return new string[] { };
            DoomEventHandler.LevelIndicator? olddata = DoomUtils.PersonalBestOf(cartidge);
            if (olddata == null) return new[] { ModBehaviour.getName("duckovdoom.doom.norecord") };
            return new[] { String.Format(ModBehaviour.getName("duckovdoom.doom.pr"), LevelFormatter(olddata)) };
        }
    }

    protected override bool CheckFinished()
    {
        if (ModBehaviour.Instance == null) return false;
        DoomEventHandler.LevelIndicator? olddata = DoomUtils.PersonalBestOf(cartidge);
        if (olddata == null) return false;
        return olddata.CompareTo(targetLevel) >= 0;
    }

    public override object GenerateSaveData()
    {
        return true;
    }

    public override void SetupSaveData(object data)
    {
    }

    public void EventHandler(DoomRecordLevelBreak evt)
    {
        ReportStatusChanged();
    }

    protected override void OnInit()
    {
        base.OnInit();
        EventBusManager.Instance.Sync.Register<DoomRecordLevelBreak>(EventHandler);
        ReportStatusChanged();
    }

    private void OnDisable()
    {
        EventBusManager.Instance.Sync.Unregister<DoomRecordLevelBreak>(EventHandler);
    }
}