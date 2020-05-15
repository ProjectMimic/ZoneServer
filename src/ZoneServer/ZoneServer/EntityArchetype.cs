using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.ClearScript;

namespace ZoneServer
{
    public class EntityArchetype
    {
        public Job Main;
        public Job Secondary;
        public Ecosystem Ecosystem;
        public byte STR;
        public byte DEX;
        public byte VIT;
        public byte AGI;
        public byte INT;
        public byte MND;
        public byte CHR;
        public byte ATK;

        public EntityArchetype Copy()
        {
            return (EntityArchetype)this.MemberwiseClone();
        }
    }

    public class Archetypes
    {
        private static Dictionary<string, EntityArchetype> Data = new Dictionary<string, EntityArchetype>();

        public static void Add(string name, ScriptObject data)
        {
            EntityArchetype archetype = null;
            if (Data.ContainsKey(name))
            {
                archetype = Data[name];
            }
            else
            {
                archetype = new EntityArchetype();
                Data[name] = archetype;
            }

            data.TryGetProperty<Job>("main", ref archetype.Main);
            data.TryGetProperty<Job>("secondary", ref archetype.Secondary);
            data.TryGetProperty<Ecosystem>("ecosystem", ref archetype.Ecosystem);
            data.TryGetProperty("str", ref archetype.STR);
            data.TryGetProperty("dex", ref archetype.DEX);
            data.TryGetProperty("vit", ref archetype.VIT);
            data.TryGetProperty("agi", ref archetype.AGI);
            data.TryGetProperty("atk", ref archetype.ATK);
        }

        public static void Extend(string archetype, string name, ScriptObject data)
        {
            if (Data.ContainsKey(archetype))
            {
                Data[name] = Data[archetype].Copy();
                Add(name, data);
            }
        }
    }

}
