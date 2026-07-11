using System.Collections.Generic;
using Duckov.Quests;
using DuckovDOOM.Event;
using FeatherMod;
using FeatherMod.Utils;

namespace DuckovDOOM.Quests;

public class QuestsHandler
{
    public static void Init()
    {
        if (ModBehaviour.Instance == null) return;

        var quest1 = new QuestData()
        {
            displayName = "duckovdoom.quest.quest1",
            description = "duckovdoom.quest.desc1",
            Id = new Identifier(ModBehaviour.Instance.GetModid(), "quest1"),
            questGiver = QuestGiverID.Jeff,
            requireLevel = 1,
            tasks = new List<TaskData>(),
            rewards = new List<RewardData>()
        };
        quest1.tasks.Add(new TaskCustomTask<TaskDoomBreakLevel>
        {
            Initialization = (tasks) =>
            {
                tasks.targetLevel = new DoomEventHandler.LevelIndicator
                {
                    episode = 1,
                    map = 1
                };
                tasks.cartidge = "freedoom1.wad";
                tasks.cartidgeTranslateKey = "DuckovDOOM.item.name.doomcartridge";
            }
        });
        quest1.rewards.Add(new RewardMoney
        {
            amount = 1000
        });
        quest1.rewards.Add(new RewardEXP
        {
            amount = 250
        });
        QuestUtils.RegisterQuest(quest1, ModBehaviour.Instance.GetModid());
        QuestUtils.TryGetQuestIdentifier(111, out var rootId);
        QuestUtils.AddQuestRelation(quest1.Id, rootId, null);
        
        var quest2 = new QuestData()
        {
            displayName = "duckovdoom.quest.quest2",
            description = "duckovdoom.quest.desc2",
            Id = new Identifier(ModBehaviour.Instance.GetModid(), "quest2"),
            questGiver = QuestGiverID.Jeff,
            requireLevel = 1,
            tasks = new List<TaskData>(),
            rewards = new List<RewardData>()
        };
        quest2.tasks.Add(new TaskCustomTask<TaskDoomBreakLevel>
        {
            Initialization = (tasks) =>
            {
                tasks.targetLevel = new DoomEventHandler.LevelIndicator
                {
                    episode = 0,
                    map = 1
                };
                tasks.cartidge = "freedoom2.wad";
                tasks.cartidgeTranslateKey = "DuckovDOOM.item.name.doomcartridge2";
            }
        });
        quest2.rewards.Add(new RewardMoney
        {
            amount = 1000
        });
        quest2.rewards.Add(new RewardEXP
        {
            amount = 250
        });
        QuestUtils.RegisterQuest(quest2, ModBehaviour.Instance.GetModid());
        QuestUtils.AddQuestRelation(quest2.Id, rootId, null);
    }
}