using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CodexUI : MonoBehaviour
{
    [System.Serializable]
    public class TuyulCharacter
    {
        public string Name;
        public string Title;
        public int HP;
        public int BaseAttack;
        public int PocketMoney;
        public string Description;
        public string[] Abilities;
        public string[] SpecialSkill;
        public string PassiveTalent;
        public bool isUnlocked;

    }
    public TuyulCharacter[] tuyulCodex = new TuyulCharacter[]
    {
        new TuyulCharacter {
        Name = "Aventurine",
        Title = "The Sparkling Trickster",
        HP = 50,
        BaseAttack = 10,
        PocketMoney = 30,
        Description = "Aventurine, the Sparkling Trickster, is a Tuyul who loves to gamble. He spends most of his time buried in his treasure chest, rarely coming out, and is always accompanied by his trusted duck!",
        Abilities = new string[] {
            "Ketimpuk Batu: Aventurine lazily throws a stone, dealing his base damage.",
            "Tangan Panjang, Badan Pendek: Aventurine can steal between 1-100 coins without you noticing."
        },
        SpecialSkill = new string []{
            "The Great Gatsby: Aventurine deals 1.5x his base attack. After this, whenever the player lands a critical hit, Aventurine retaliates with Follow-Up Attack (FUA), dealing half of his base attack. (30% chance of use)"
        },
        PassiveTalent = "" 
        },
        new TuyulCharacter {
        Name = "Mr.Rizzler",
        Title = "The Flirtatious Dancer",
        HP = 50,
        BaseAttack = 10,
        PocketMoney = 30,
        Description = "Mr. Rizzler is a flirtatious Tuyul who enjoys dancing. His charming moves might leave you blushing, but beware—he's as sly as he is smooth!",
        Abilities = new string[] {
            "Ketimpuk Batu: He throws a stone with style and rizz, dealing base damage.",
            "Tangan Panjang, Badan Pendek: Distracted by his charisma, you won't notice when he steals 1-100 coins."
        },
        SpecialSkill = new string []{"Seduce You To Death: Mr. Rizzler performs an irresistible dance. For 3 rounds, your health potion's effectiveness and critical hit chances are reduced. (20% chance of use)"},
        PassiveTalent = "" 
        },
        new TuyulCharacter {
        Name = "Rolly Polly",
        Title = "The Stalwart Defender & Reckless Attacker",
        HP = 50,
        BaseAttack = 10,
        PocketMoney = 30,
        Description = "Rolly and Polly are inseparable best friends who work together to take down bigger opponents. With Polly supporting him, Rolly never lets his friend down.",
        Abilities = new string[] {
            "Polly cheers Rolly, giving him either 1.5x base attack or reducing incoming damage before attacking.",
            "Tangan Panjang, Badan Pendek: While Polly distracts you, Rolly steals 1-100 coins"
        },
        SpecialSkill = new string []{"Teamwork is Dreamwork: Polly makes Rolly invincible for 3 rounds, taking 8 damage per turn in exchange. Rolly deals 2x base attack during this time. (20% chance of use)"},
        PassiveTalent = "" 
        },
        new TuyulCharacter {
        Name = "Chaeng Yul",
        Title = "Bestie of Pocong",
        HP = 200,
        BaseAttack = 15,
        PocketMoney = 100,
        Description = "ChaengYul, a ghostly Tuyul, borrows the power of his best friend Pocong to haunt his enemies. Don’t be fooled by his innocent looks!",
        Abilities = new string[] {
            "Ketimpuk Batu: ChaengYul uses his body to headbutt you, dealing base damage.",
            "Tangan Panjang, Badan Pendek: Using Pocong's powers, he steals 1-100 coins unnoticed."
        },
        SpecialSkill = new string []{"Beyond the Grave: ChaengYul sneaks up behind you, dealing 2x base attack. There's a 20% chance you’ll die instantly from his Scare You To Death move. (20% chance of use)"},
        PassiveTalent = "Cursed Hop: When HP drops below 50%, ChaengYul has a 30% chance to heal 15% of his max HP." 
        },
        new TuyulCharacter {
        Name = "Cheok Yul",
        Title = "Impish Who Studies under The Ancient Beast; [Kecoak Terbang]",
        HP = 200,
        BaseAttack = 15,
        PocketMoney = 100,
        Description = "Cheok Yul is an impish Tuyul who studies under the terrifying Kecoak Terbang (Flying Cockroach). His mischievous nature and insect allies make him a formidable foe.",
        Abilities = new string[] {
            "Ketimpuk Batu: Cheok Yul throws stones while staying hidden, dealing base Attack.",
            "Tangan Panjang, Badan Pendek: Fear of his mentor makes you freeze as Cheok Yul steals 1-100 coins."
        },
        SpecialSkill = new string[] {
            "Monster Lurks Beneath the Shadow of the Dawn: Deals poison damage equal to half his base attack for 3 rounds. (20% chance of use)",
            "The Democracy: Summons 2-8 cockroaches, each dealing 5-10 damage. (20% chance of use)"
        },
                    
        PassiveTalent = "The Flying Horror: When HP is below 50%, Cheok Yul flies and can only be attacked by ranged attacks." 
        },
        new TuyulCharacter {
        Name = "Jaek Yul",
        Title = "Tuyul of All Trades, Master of All",
        HP = 350,
        BaseAttack = 20,
        PocketMoney = 200,
        Description = "Jaek Yul is the ultimate Tuyul, capable of transforming into any of his fellow Tuyuls and using their special skills. A true master of trickery!",
        Abilities = new string[] {
            "Ketimpuk Batu: Uses the most effective Tuyul defense agreed by Tuyul Ascosiation, throwing a stone, dealing Base Attack.",
            "Tangan Panjang, Badan Pendek: Can steal 1-100 coins effortlessly."
        },
        SpecialSkill = new string []{"Can transform into any Tuyul and access their special skills."},
        PassiveTalent = "" 
        },
    };

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI baseAttackText;
    public TextMeshProUGUI abilitiesText;
    public TextMeshProUGUI specialSkillText;
    public TextMeshProUGUI passiveSkillText;
    public Image targetImage;
    public Sprite[] tuyulSprites;
    public AudioSource newIntro;
    private static bool isGameInitialized = false;

    public static CodexUI Instance { get; private set; }

    private void Awake()
    {
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void DisplayTuyulCharacter(int index)
    {
        SetImageByIndex(index);
        TuyulCharacter character = tuyulCodex[index];
        // if (character.isUnlocked)
        // {
            titleText.text = character.Title;
            descriptionText.text = character.Description;
            hpText.text = "HP: " + character.HP;
            moneyText.text = "Money: " + character.PocketMoney;
            baseAttackText.text = "Base Attack: " + character.BaseAttack;
            abilitiesText.text = string.Join("\n", character.Abilities);
            specialSkillText.text = string.Join("\n", "Special Skill: " + character.SpecialSkill);
            passiveSkillText.text = "Passive Skill: " + character.PassiveTalent;
        // }
        // else
        // {
        //     titleText.text = "???";
        //     descriptionText.text = "You have not encountered this enemy yet.";
        //     hpText.text = "";
        //     moneyText.text = "";
        //     baseAttackText.text = "";
        //     abilitiesText.text = "";
        //     specialSkillText.text = "";
        //     passiveSkillText.text = "";
        // }
    }

    public void SetImageByIndex(int index)
    {
        if (index >= 0 && index < tuyulSprites.Length)
        {
            targetImage.sprite = tuyulSprites[index];
        }
        else
        {
            Debug.LogError("Indeks gambar tidak valid!");
        }
    }

    public int GetTuyulIndexByName(string tuyulName)
    {
        for (int i = 0; i < tuyulCodex.Length; i++)
        {
            if (tuyulCodex[i].Name == tuyulName)
            {
                return i;
            }
        }
        return -1;
    }
    public void ResetCodexData()
    {
        for (int i = 0; i < tuyulCodex.Length; i++)
        {
            tuyulCodex[i].isUnlocked = false;
        }
    }
}
