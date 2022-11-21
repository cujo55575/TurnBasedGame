using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ENUM
{
    public enum EMOJI
    {
        Angry,
        AngryDevil,
        Attracted,
        Angel,
        Cold,
        Hot,
        Dead,
        Amaze,
        Wink,
        HoldingTears,
        Sick,
        Monocle,
        Glasses,
        Tongue,
        StarStruck,
        Surprise,
        Crazy,
        Laugh,
        Cry,
        Worry,
        Teary,
        Drunk,
        Silence,
        Nonchalant,
        Happy,
        Cheerful,
        Kiss,
        Smooch,
        Chuckle,
        Roar,
        RollEyes,
        Sad,
        Fever,
        Sleep,
        Nap,
        Think,
        Disgust,
        Like,
        Dislike,
        Heart,
        Money,
        Enjoy,
        LoveFace,
        Poop
    }
    public enum Emoji_Level_Layer : int
    {
        LevelOne = 0,
        LevelTwo = 2,
        LevelThree = 5,
    }
    public enum ABILITY_TYPE
    {
        Faint,
        StartOfBattle,
        Sell,
        FriendSummoned,
        Hurt,
        BeforeAttack,
        Buy,
        FriendSold,
        TierChange,
        FaintSummon,
        FriendHurt,
        LevelUp,
        BeforeFaint,
        StartOfTurn,
        EndTurn,
        FriendAheadAttacks,
        FriendAheadFaints,
        EmojiEatsUpgrade,
        KnockOut
    }
}
