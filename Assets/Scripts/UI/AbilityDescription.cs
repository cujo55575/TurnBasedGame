using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDescription : Singleton<AbilityDescription>
{
    private string Angry(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level * 2 + " Damage and " + level + " HP";

        return temp;
    }

    private string AngryDevil(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level + " HP";

        return temp;
    }

    private string Attracted(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + " " + level + " Damage and " + level + " HP";

        return temp;
    }

    private string Angle(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level + " HP";

        return temp;
    }

    private string Cold(EmojiData data)
    {
        var level = data.level + 1;

        string temp = data.abilityName + " : " + data.abilityDescription + level + " Damage and " + level + " HP";

        return temp;
    }
    private string Hot(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + " " + level + " Damage";

        return temp;
    }
    private string Dead(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal " + level + " Damage to First Enemy";

        return temp;
    }
    private string Amaze(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gives a random friend  " + level + " Damage and " + level + " HP.";

        return temp;
    }

    private string Wink(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain " + level + " Gold.";

        return temp;
    }
    private string HoldingTears(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Copy " + 50 * data.level + "% HP of the friend who has highest HP.";

        return temp;
    }

    private string Sick(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + 50 * level + "% Damage";

        return temp;
    }

    private string Monocle(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Give " + level + " Damage to friend behind.";

        return temp;
    }

    private string Glasses(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level + " Damage and " + level + " HP";

        return temp;
    }
    private string StarStruck(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level * 4 + " Damage.";

        return temp;
    }

    private string Surprise(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription;

        return temp;
    }

    private string Crazy(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level + " HP";

        return temp;
    }

    private string Laugh(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Summon Tier 3 Emoji with " + level * 2 + " Damage and " + level * 2 + " HP";

        return temp;
    }

    private string Tongue(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal " + level * 2 + " Damage to All Enemies";

        return temp;

    }
    private string Cry(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain " + level + " Coin";

        return temp;
    }
    private string Worry(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal " + level * 50 + " % ATK to adjuacent pet.";

        return temp;
    }
    private string Teary(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal " + level * 2 + " Damage to Random Enemy";

        return temp;
    }
    private string Drunk(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Give Friend Behind " + level * 2 + " HP and " + level * 2 + " Damage.";

        return temp;
    }
    private string Silence(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain " + level * 2 + " HP and " + level * 2 + " Damage.";

        return temp;
    }
    private string Nonchalant(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain " + level + " HP and " + level + " Damage.";

        return temp;
    }
    private string Happy(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain " + level * 2 + " HP and " + level * 2 + " Damage.";

        return temp;
    }
    private string Cheerful(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain  " + level + " Armour.";

        return temp;
    }
    private string Kiss(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Give it  " + level + " HP.";

        return temp;
    }
    private string Smooch(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Summon Two Kisses with   " + level + " HP and " + level + " Damage.";

        return temp;
    }
    private string Chuckle(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : If you lost last battle give all friends " + level + " HP and " + level + " Damage.";

        return temp;
    }
    private string Roar(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal 3 Damage to the lowest HP enemy.Trigger " + level + " Times.";

        return temp;
    }
    private string RollEyes(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain  " + level * 2 + " HP and " + level * 2 + " Damage.If level 3 friend existed.";

        return temp;
    }
    private string Sad(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Summon a sad with  " + level * 5 + " HP and " + level * 5 + " Damage.";

        return temp;
    }
    private string Fever(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Gain " + level * 3 + " HP and " + level * 3 + " Damage.";

        return temp;
    }
    private string Sleep(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + ": Copy level " + level + " ability from friend ahead until the end of battle.";

        return temp;
    }
    private string Nap(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Give two level 2 and 3 friends  " + level + " HP and " + level + " Damage.";

        return temp;
    }
    private string Think(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Reduce Hp of the highest enemy by " + level * 33 + " %.";

        return temp;
    }
    private string Disgust(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Summon " + level + "Surprise with 1Hp and 2 Attack.";

        return temp;
    }
    private string Like(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + " " + level + " Gold.";

        return temp;
    }
    private string Dislike(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Give " + level + " Armor to friends behind.";

        return temp;
    }
    private string Heart(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal " + level * 2 + " Damage to Random Enemy.";

        return temp;
    }
    private string Money(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : Deal " + level * 6 + " Damage to a Random enemy.";

        return temp;
    }
    private string Enjoy(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + " " + level + " Damage and " + level + " HP.";

        return temp;
    }
    private string LoveFace(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + level + " time(s).";

        return temp;
    }
    private string Poop(EmojiData data)
    {
        var level = data.level;

        string temp = data.abilityName + " : " + data.abilityDescription + " " + level * 2 + " Damage and " + level * 3 + " HP.";

        return temp;
    }
    public string GetDescription(EmojiData emojiData)
    {
        string description = "";

        switch (emojiData.type)
        {
            case ENUM.EMOJI.Angry:
                description = Angry(emojiData);
                break;
            case ENUM.EMOJI.AngryDevil:
                description = AngryDevil(emojiData);
                break;
            case ENUM.EMOJI.Attracted:
                description = Attracted(emojiData);
                break;
            case ENUM.EMOJI.Angel:
                description = Angle(emojiData);
                break;
            case ENUM.EMOJI.Cold:
                description = Cold(emojiData);
                break;
            case ENUM.EMOJI.Hot:
                description = Hot(emojiData);
                break;
            case ENUM.EMOJI.Dead:
                description = Dead(emojiData);
                break;
            case ENUM.EMOJI.Amaze:
                description = Amaze(emojiData);
                break;
            case ENUM.EMOJI.Wink:
                description = Wink(emojiData);
                break;
            case ENUM.EMOJI.HoldingTears:
                description = HoldingTears(emojiData);
                break;
            case ENUM.EMOJI.Sick:
                description = Sick(emojiData);
                break;
            case ENUM.EMOJI.Monocle:
                description = Monocle(emojiData);
                break;
            case ENUM.EMOJI.Glasses:
                description = Glasses(emojiData);
                break;
            case ENUM.EMOJI.StarStruck:
                description = StarStruck(emojiData);
                break;
            case ENUM.EMOJI.Surprise:
                description = Surprise(emojiData);
                break;
            case ENUM.EMOJI.Crazy:
                description = Crazy(emojiData);
                break;
            case ENUM.EMOJI.Laugh:
                description = Laugh(emojiData);
                break;
            case ENUM.EMOJI.Tongue:
                description = Tongue(emojiData);
                break;
            case ENUM.EMOJI.Cry:
                description = Cry(emojiData);
                break;
            case ENUM.EMOJI.Worry:
                description = Worry(emojiData);
                break;
            case ENUM.EMOJI.Teary:
                description = Teary(emojiData);
                break;
            case ENUM.EMOJI.Drunk:
                description = Drunk(emojiData);
                break;
            case ENUM.EMOJI.Silence:
                description = Silence(emojiData);
                break;
            case ENUM.EMOJI.Nonchalant:
                description = Nonchalant(emojiData);
                break;
            case ENUM.EMOJI.Happy:
                description = Happy(emojiData);
                break;
            case ENUM.EMOJI.Cheerful:
                description = Cheerful(emojiData);
                break;
            case ENUM.EMOJI.Kiss:
                description = Kiss(emojiData);
                break;
            case ENUM.EMOJI.Smooch:
                description = Smooch(emojiData);
                break;
            case ENUM.EMOJI.Chuckle:
                description = Chuckle(emojiData);
                break;
            case ENUM.EMOJI.Roar:
                description = Roar(emojiData);
                break;
            case ENUM.EMOJI.RollEyes:
                description = RollEyes(emojiData);
                break;
            case ENUM.EMOJI.Sad:
                description = Sad(emojiData);
                break;
            case ENUM.EMOJI.Fever:
                description = Fever(emojiData);
                break;
            case ENUM.EMOJI.Sleep:
                description = Sleep(emojiData);
                break;
            case ENUM.EMOJI.Nap:
                description = Nap(emojiData);
                break;
            case ENUM.EMOJI.Think:
                description = Think(emojiData);
                break;
            case ENUM.EMOJI.Disgust:
                description = Disgust(emojiData);
                break;
            case ENUM.EMOJI.Like:
                description = Like(emojiData);
                break;
            case ENUM.EMOJI.Dislike:
                description = Dislike(emojiData);
                break;
            case ENUM.EMOJI.Heart:
                description = Heart(emojiData);
                break;
            case ENUM.EMOJI.Money:
                description = Money(emojiData);
                break;
            case ENUM.EMOJI.Enjoy:
                description = Enjoy(emojiData);
                break;
            case ENUM.EMOJI.LoveFace:
                description = LoveFace(emojiData);
                break;
            case ENUM.EMOJI.Poop:
                description = Poop(emojiData);
                break;
            default:
                break;
        }
        return description;
    }
}
