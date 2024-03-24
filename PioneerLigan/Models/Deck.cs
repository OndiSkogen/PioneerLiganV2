namespace PioneerLigan.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SuperArchType { get; set; } = string.Empty;
        public string ColorAffiliation { get; set; } = string.Empty;
        public MetaGame MetaGame { get; set; } = new MetaGame();
    }
}
