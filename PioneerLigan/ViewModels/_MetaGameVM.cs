using Newtonsoft.Json;
using PioneerLigan.HelperClasses;
using PioneerLigan.Models;

namespace PioneerLigan.ViewModels
{
    public class _MetaGameVM
    {
        public List<MetaDeck> Decks { get; set; } = new List<MetaDeck>();
        public DateTime EventDate { get; set; }

        public _MetaGameVM(string meta, DateTime date)
        {
            //var decks = JsonConvert.DeserializeObject<List<Deck>>(meta);
            //var totalNoOfDecks = decks.Sum(d => d.NoOfDecks);

            //foreach (var deck in decks)
            //{
            //    Decks.Add(new MetaDeck { Name = deck.Name, Count = deck.NoOfDecks, Percentage = (int)Math.Round((double)deck.NoOfDecks / totalNoOfDecks * 100) });
            //}

            //Decks = Decks.OrderByDescending(d => d.Count).ThenByDescending(d => d.Name).ToList();

            //EventDate = date;
        }

        public class MetaDeck
        {
            public string Name { get; set; } = string.Empty;
            public int Count { get; set; } = 0;
            public int Percentage { get; set; } = 0;
        }
    }
}
