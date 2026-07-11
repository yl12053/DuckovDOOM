using System;
using DuckovDOOM.Minigame;
using FeatherMod.Events;
using FeatherMod.Register;
using FeatherMod.Saves;
using FeatherMod.Utils;
using ManagedDoom.Event;

namespace DuckovDOOM.Event;

public class DoomEventHandler
{
    public static void Init()
    {
        if (ModBehaviour.Instance == null) return;
        EventBusManager.Instance.Sync.Register<DoomFinishedLevel>(OnDoomFinishedLevel, 0, ModBehaviour.Instance.GetModid());
    }
    
    public static readonly int[] CALC = new[] { 3, 5, 6, 2 };

    public class LevelIndicator: IComparable, IComparable<LevelIndicator>
    {
        public int episode;
        public int map;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is LevelIndicator right)
            {
                return CompareTo(right);
            }

            throw new ArgumentException();
        }

        public int CompareTo(LevelIndicator right)
        {
            if ((episode == 0) ^ (right.episode == 0)) return 1;
            if (episode != 0)
            {
                if (episode != right.episode) return episode - right.episode;
                if (map == right.map) return 0;
                if (map == 9) return right.map <= CALC[episode - 1] ? 1 : -1;
                if (right.map == 9) return map > CALC[episode - 1] ? 1 : -1;
            }
            else
            {
                if (map == right.map) return 0;
                if ((map >= 31) ^ (right.map >= 31))
                {
                    if (map >= 31) return right.map <= 15 ? 1 : -1;
                    return map > 15 ? 1 : -1;
                }
            }

            return map - right.map;
        }
    }

    public static void OnDoomFinishedLevel(DoomFinishedLevel evt)
    {
        if (ModBehaviour.Instance == null) return;
        LevelIndicator? olddata = DoomUtils.PersonalBestOf(evt.wadName);
        LevelIndicator newInd = new LevelIndicator
        {
            episode = evt.episode,
            map = evt.map
        };
        if (olddata != null)
        {
            LevelIndicator oldInd = olddata;
            if (newInd.CompareTo(oldInd) <= 0) return;
        };
        SaveUtils.Save(
            new Identifier(ModBehaviour.Instance.GetModid(), $"max_level_{evt.wadName}"), newInd
        );
        EventBusManager.Instance.Sync.Post(new DoomRecordLevelBreak(evt.wadName, evt.episode, evt.map));
    }
}