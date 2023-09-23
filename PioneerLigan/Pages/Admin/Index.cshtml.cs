using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using PioneerLigan.HelperClasses;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using PioneerLigan.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.RegularExpressions;
using PioneerLigan.Data;

namespace PioneerLigan.Pages.Admin
{
    [Authorize(Roles = UserRoles.Admin)]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public IndexModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }
        public List<FolderInfo> Folders { get; set; } = new List<FolderInfo>();
        public void OnGet()
        {
            Folders.Clear();
            Folders = FillPages();
        }

        private List<FolderInfo> FillPages()
        {
            var pages = new List<FolderInfo>();

            var razorPagesDirectory = Path.Combine(_environment.ContentRootPath, "Pages");

            DirectoryInfo directoryInfo = new DirectoryInfo(razorPagesDirectory);
            DirectoryInfo[] subDirs = directoryInfo.GetDirectories();

            foreach (DirectoryInfo subDir in subDirs)
            {
                if(subDir.Name != "Shared")
                {
                    FolderInfo folder = new FolderInfo();
                    folder.Name = subDir.Name;
                    // Do something with the subdirectory
                    var files = subDir.GetFiles();
                    foreach (var f in files)
                    {
                        if (f.Name.EndsWith(".cshtml"))
                        {
                            folder.Files.Add(new PageInfo
                            {
                                Path = f.FullName,
                                Name = Path.GetFileNameWithoutExtension(f.Name)
                            });
                        }
                    }
                    pages.Add(folder);
                }                
            }
            return pages;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string url = "http://pioneerligan.net/";
                    string htmlContent = webClient.DownloadString(url);

                    var doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    var specificElements = doc.DocumentNode.SelectNodes("//div[@class='col-sm-12 col-md-6 col-lg-4 event-table']");
                    
                    var events = new List<LeagueEvent>();
                    var eventResults = new List<EventResult>();

                    var leagueOneEventCounter = 1;
                    var leagueTwoEventCounter = 1;
                    var leagueFourEventCounter = 1;

                    if (specificElements != null)
                    {
                        foreach (var e in specificElements)
                        {
                            var playersEntities = from p in _context.Player select p;
                            var players = playersEntities.ToList();

                            var leagueEvent = new LeagueEvent();
                            
                            var dateSpan = e.SelectSingleNode(".//span[@class='event-header']");
                            string date = dateSpan != null ? dateSpan.InnerText.Trim() : string.Empty;
                            leagueEvent.Date = ExtractDate(date);

                            if (leagueEvent.Date == DateTime.MinValue)
                            {
                                continue;
                            }

                            if (leagueEvent.Date > new DateTime(2022, 9, 12) && leagueEvent.Date < new DateTime(2022, 12, 7))
                            {
                                leagueEvent.LeagueID = 1;
                                leagueEvent.EventNumber = leagueOneEventCounter;
                                leagueOneEventCounter++;
                            }
                            else if (leagueEvent.Date > new DateTime(2023, 1, 9) && leagueEvent.Date < new DateTime(2023, 3, 29))
                            {
                                leagueEvent.LeagueID = 2;
                                leagueEvent.EventNumber = leagueTwoEventCounter;
                                leagueTwoEventCounter++;
                            }
                            else if (leagueEvent.Date > new DateTime(2023, 4, 3) && leagueEvent.Date < new DateTime(2023, 6, 7))
                            {
                                leagueEvent.LeagueID = 4;
                                leagueEvent.EventNumber = leagueFourEventCounter;
                                leagueFourEventCounter++;
                            }

                            events.Add(leagueEvent);
                            var addedEntity = _context.LeagueEvent.Add(leagueEvent);

                            // Extract and process the table data
                            var table = e.SelectSingleNode(".//table[@class='table table-striped table-dark']");

                            var placement = 1;

                            if (table != null)
                            {
                                foreach (var row in table.SelectNodes(".//tbody/tr"))
                                {
                                    var columns = row.SelectNodes("td | th");

                                    if (columns != null && columns.Count >= 5)
                                    {
                                        var eventResult = new EventResult();

                                        eventResult.PlayerName = columns[0].InnerText.Trim();
                                        eventResult.Points = int.Parse(columns[1].InnerText.Trim());
                                        eventResult.OMW = decimal.Parse(columns[2].InnerText.Trim().Replace("%", ""));
                                        eventResult.GW = decimal.Parse(columns[3].InnerText.Trim().Replace("%", ""));
                                        eventResult.OGW = decimal.Parse(columns[4].InnerText.Trim().Replace("%", ""));
                                        eventResult.Placement = placement;
                                        eventResult.EventId = addedEntity.Entity.Id;

                                        var playerExists = players.Where(n => n.PlayerName == eventResult.PlayerName).ToList();
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
                                        placement++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return RedirectToPage("./Index");
        }

        private DateTime ExtractDate(string date)
        {
            int dateStartIndex = date.IndexOf(':') + 1; // Skip past "League event:"
            string dateSubstring = date.Substring(dateStartIndex).Trim();

            string pattern = @"\d+"; // \d matches digits, + matches one or more occurrences

            MatchCollection matches = Regex.Matches(dateSubstring, pattern);

            var eventDate = new DateTime(int.Parse(matches[2].Value), int.Parse(matches[0].Value), int.Parse(matches[1].Value));

            return eventDate;
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
    }

    
}
