namespace WotrSandbox.Content.Dragon.Bloodlines
{
    public struct Spell
    {
        public Spell(int level, string blueprintId)
        {
            Level = level;
            BlueprintId = blueprintId;
        }

        public int Level { get; }
        public string BlueprintId { get; }
    }
}
