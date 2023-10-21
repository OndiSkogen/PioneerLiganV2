using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PioneerLigan.Data;
using PioneerLigan.Models;

namespace PioneerLigan.Pages.LeagueEvents
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string? selectedId)
        {
            LoadData();
            DeckInfoList = new List<DeckInfo>();

            for (int i = 0; i < 28; i++)
            {
                DeckInfoList.Add(new DeckInfo { DeckName = string.Empty, NoOfDecks = 0 });
            }

            DeckNames = new List<string>
            {
                "A Deck Name 1",
                "D Deck Name 2",
                "H Deck Name 3",
            };

            if (selectedId != null)
            {
                SelectedLeague = int.Parse(selectedId);
                DisplayEvents = Events.Where(i => i.LeagueID == SelectedLeague).ToList();
            }
            else
            {
                SelectedLeague = 0;
            }

            return Page();
        }

        [BindProperty]
        public string HtmlContent { get; set; } = string.Empty;

        [BindProperty]
        public LeagueEvent LeagueEvent { get; set; } = default!;
        public List<LeagueEvent> Events { get; set; } = new List<LeagueEvent>();
        public List<LeagueEvent> DisplayEvents { get; set; } = new List<LeagueEvent>();
        public List<League> Leagues { get; set; } = new List<League>();
        public List<Player> Players { get; set; } = new List<Player>();
        public int SelectedLeague { get; set; }
        public List<DeckInfo> DeckInfoList { get; set; }
        public List<string> DeckNames { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var leagueId = Request.Form["league-id"];



            SelectedLeague = int.Parse(Request.Form["league-id"]);

            if (!ModelState.IsValid || _context.LeagueEvent == null || _context.EventResult == null || _context.Player == null || SelectedLeague == 0)
            {
                return Page();
            }

            LoadData();

            LeagueEvent.LeagueID = SelectedLeague;
            LeagueEvent.MetaGame = JsonConvert.SerializeObject(DeckInfoList);

            string filePath = @"C:\Sites\Logs\CreateEventLog.txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Date: " + DateTime.Now);
                writer.WriteLine("Selected League Id: " + SelectedLeague);
                writer.WriteLine("League Id: " + LeagueEvent.LeagueID);
                writer.WriteLine("Event nr: " + LeagueEvent.EventNumber);
                writer.WriteLine("Event Date: " + LeagueEvent.Date);
            }

            _context.LeagueEvent.Add(LeagueEvent);
            await _context.SaveChangesAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlContent);

            // Select the table element you want to extract data from
            var table = doc.DocumentNode.SelectSingleNode("//table");

            // Extract the table rows
            var rows = table.SelectNodes(".//tr");

            // Loop through each row and extract the data from the cells
            for (int i = 0; i < rows.Count; i++)
            {
                // Extract the cells in this row
                var cells = rows[i].SelectNodes(".//td");

                var eventResult = new EventResult();
                eventResult.EventId = LeagueEvent.Id;

                // Loop through each cell and extract the data
                if (cells != null && !string.IsNullOrEmpty(cells[0].InnerText))
                {
                    for (int j = 0; j < cells.Count; j++)
                    {
                        // Extract the inner text of the cell
                        var cellText = cells[j].InnerText;
                        string[] temp;

                        // Do something with the cell text
                        if (cellText != null)
                        {
                            switch (j)
                            {
                                case 0:
                                    eventResult.Placement = int.Parse(cellText);
                                    break;
                                case 1:
                                    eventResult.PlayerName = ExtractPlayerName(cellText);
                                    break;
                                case 2:
                                    eventResult.Points = int.Parse(cellText);
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    temp = cellText.Split('%');
                                    eventResult.OMW = decimal.Parse(temp[0]);
                                    break;
                                case 5:
                                    temp = cellText.Split('%');
                                    eventResult.GW = decimal.Parse(temp[0]);
                                    break;
                                case 6:
                                    temp = cellText.Split('%');
                                    eventResult.OGW = decimal.Parse(temp[0]);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(eventResult.PlayerName))
                {
                    string errorFilePath = @"C:\Sites\Logs\Errorlog.txt";
                    string stringToAppend = "Row: " + i + "\n" +
                        "EventId: " + eventResult.EventId.ToString() + "\n" +
                        "PlayerName: " + eventResult.PlayerName + "\n" +
                        "PlayerId: " + eventResult.PlayerId.ToString() + "\n" +
                        "Placement: " + eventResult.Placement.ToString() + "\n" +
                        "Points: " + eventResult.Points.ToString() + "\n" +
                        "OMW: " + eventResult.OMW.ToString() + "\n" +
                        "GW: " + eventResult.GW.ToString() + "\n" +
                        "OGW: " + eventResult.OGW.ToString();

                    using (StreamWriter sw = new StreamWriter(errorFilePath, true))
                    {
                        sw.WriteLine(stringToAppend);
                    }

                    continue;
                }

                var playerExists = Players.Where(n => n.PlayerName == eventResult.PlayerName).ToList();
                if (playerExists.Any())
                {
                    var playerToUpdate = playerExists.First();
                    playerToUpdate.Events++;
                    playerToUpdate.Points += eventResult.Points;

                    playerToUpdate = AddWinLossTie(playerToUpdate, eventResult.Points);

                    _context.Player.Update(playerToUpdate);
                    await _context.SaveChangesAsync();

                    eventResult.PlayerId = playerToUpdate.Id;

                    _context.EventResult.Add(eventResult);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var playerToAdd = new Player();

                    playerToAdd.PlayerName = eventResult.PlayerName;
                    playerToAdd.Events = 1;
                    playerToAdd.Points = eventResult.Points;

                    playerToAdd = AddWinLossTie(playerToAdd, eventResult.Points);

                    _context.Player.Add(playerToAdd);
                    await _context.SaveChangesAsync();

                    eventResult.PlayerId = playerToAdd.Id;
                    _context.EventResult.Add(eventResult);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./");
        }

        private string ExtractPlayerName(string cellText)
        {
            string nameToExtract = string.Empty;
            cellText = cellText.Trim();
            string[] splits = cellText.Split(' ');

            foreach (var str in splits)
            {
                var temp = NormalizeString(str);
                if (IsOnlyLetters(temp))
                {
                    nameToExtract += temp + " ";
                }
            }
            nameToExtract = nameToExtract.Trim();

            return nameToExtract;
        }

        private string NormalizeString(string str)
        {
            string returnString = string.Empty;

            if (string.IsNullOrEmpty(str))
            {
                return returnString;
            }

            returnString = str.Trim();
            returnString = returnString.ToLower();
            returnString = char.ToUpper(returnString[0]) + returnString.Substring(1);

            return returnString;
        }

        private bool IsOnlyLetters(string cellText)
        {
            string pattern = "^([A-Z]|Å|Ä|Ö|Í|Á)([a-z]|å|ä|ö|í|á)+$";
            Regex regex = new Regex(pattern);
            bool containsOnlyLetters = regex.IsMatch(cellText);

            return containsOnlyLetters;
        }

        private void LoadData()
        {
            var leagues = from l in _context.League select l;
            Leagues = leagues.OrderBy(l => l.Id).ToList();
            var events = from e in _context.LeagueEvent select e;
            Events = events.OrderBy(l => l.Id).ToList();
            var players = from p in _context.Player select p;
            Players = players.ToList();
        }

        private Models.Player AddWinLossTie(Models.Player player, int points)
        {
            switch (points)
            {
                case 0:
                    player.Losses += 4;
                    break;
                case 1:
                    player.Ties++;
                    player.Losses += 3;
                    break;
                case 2:
                    player.Ties += 2;
                    player.Losses += 2;
                    break;
                case 3:
                    player.Wins++;
                    player.Losses += 3;
                    break;
                case 4:
                    player.Wins++;
                    player.Ties++;
                    player.Losses += 2;
                    break;
                case 5:
                    player.Wins++;
                    player.Ties += 2;
                    player.Losses++;
                    break;
                case 6:
                    player.Wins += 2;
                    player.Losses += 2;
                    break;
                case 7:
                    player.Wins += 2;
                    player.Ties++;
                    player.Losses++;
                    break;
                case 8:
                    player.Wins += 2;
                    player.Ties += 2;
                    break;
                case 9:
                    player.Wins += 3;
                    player.Losses++;
                    break;
                case 10:
                    player.Wins += 3;
                    player.Ties++;
                    break;
                case 12:
                    player.Wins += 4;
                    break;
                default:
                    break;
            }

            return player;
        }

        public string NamePlayer(int id)
        {
            var str = "player" + id.ToString();
            return str;
        }

        public string OmwPlayer(int id)
        {
            var str = "omw" + id.ToString();
            return str;
        }

        public string GwPlayer(int id)
        {
            var str = "gw" + id.ToString();
            return str;
        }

        public string OgwPlayer(int id)
        {
            var str = "ogw" + id.ToString();
            return str;
        }

        public string PlacementPlayer(int id)
        {
            var str = "placement" + id.ToString();
            return str;
        }

        public string PointsPlayer(int id)
        {
            var str = "points" + id.ToString();
            return str;
        }

        public string DeckName(int id)
        {
            var str = "deck" + id.ToString();
            return str;
        }

        public string NoOfDecks(int id)
        {
            var str = "noOfDecks" + id.ToString();
            return str;
        }
    }
}
