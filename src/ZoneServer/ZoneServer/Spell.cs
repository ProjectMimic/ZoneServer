using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneServer
{
    public enum SpellRequirement
    {
        None,
        Merit,
        AddendumBlack,
        AddendumWhite,
        TabulaRasa,
        UnbridledLearning,
    }

    public enum SpellGroup
    {
        None,
        Song,
        Black,
        Blue,
        Ninjutsu,
        Summoning,
        White,
        Geomancy,
        Trust,
    }

    class Spell
    {
        UInt16 ID;
        string Name;
        SpellRequirement Requirement;
        SpellGroup Group;
        //SpellType Type;
        Element Element;
        //which zones the spell can be used in (summoning magic cant be used in towns)
        //valid targets (who can be targeted with the spell self, allies, enemies)
        //skill (which magic skill is it associated with)
        UInt16 MPCost;
        UInt16 CastTime;
        UInt16 RecastTime;
        float Range;
        //message - not sure if this is needed or can be derived from other data
        //magic burst message - same as above
        UInt16 Animation;
        UInt16 AnimationTime;
        TargetArea Area;
        //base?
        //multiplier?
        UInt16 CE;
        UInt16 VE;
        //special params for each spell type
        //spell check function
        //spell cast function
    }
}
