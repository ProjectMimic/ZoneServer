using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneServer
{
    public enum Job
    {
        NONE = 0,
        WAR = 1,
        MNK = 2,
        WHM = 3,
        BLM = 4,
        RDM = 5,
        THF = 6,
        PLD = 7,
        DRK = 8,
        BST = 9,
        BRD = 10,
        RNG = 11,
        SAM = 12,
        NIN = 13,
        DRG = 14,
        SMN = 15,
        BLU = 16,
        COR = 17,
        PUP = 18,
        DNC = 19,
        SCH = 20,
        GEO = 21,
        RUN = 22,
        MAX = 23,
    }

    public enum Ecosystem
    {
        Error,
        Amorph,
        Aquan,
        Arcana,
        Archaicmachine,
        Avatar,
        Beast,
        Beastmen,
        Bird,
        Demon,
        Dragon,
        Elemental,
        Empty,
        Humanoid,
        Lizard,
        Lumorian,
        Luminion,
        Plantoid,
        Unclassified,
        Undead,
        Vermin,
        Voragean,
    }

    [Flags]
    enum TargetType
    {
        Self = 0x01,
        Player = 0x02,
        PlayerParty = 0x04,
        PlayerAlliance = 0x08,
        PlayerPartyPianissimo = 0x10,
        PlayerDead = 0x20,
        Enemy = 0x40,
        Pet = 0x80,
        Npc = 0x100,
    };

    public enum TargetArea
    {
        None,
        User,
        Target,
        Conal,
        WideConal,
        RadialManifestation,
        RadialAccession,
        Pianissimo,
        Diffusion,
    }

    public enum Element
    {
        None,
        Fire,
        Earth,
        Water,
        Wind,
        Ice,
        Lightning,
        Thunder,
        Light,
        Dark,
    }
}
