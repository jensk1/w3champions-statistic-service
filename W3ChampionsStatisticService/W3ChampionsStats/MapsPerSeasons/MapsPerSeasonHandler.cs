﻿using System.Threading.Tasks;
using W3ChampionsStatisticService.PadEvents;
using W3ChampionsStatisticService.Ports;
using W3ChampionsStatisticService.ReadModelBase;

namespace W3ChampionsStatisticService.W3ChampionsStats.MapsPerSeasons
{
    public class MapsPerSeasonHandler : IReadModelHandler
    {
        private readonly IW3StatsRepo _w3Stats;

        public MapsPerSeasonHandler(
            IW3StatsRepo w3Stats
            )
        {
            _w3Stats = w3Stats;
        }

        public async Task Update(MatchFinishedEvent nextEvent)
        {
            if (nextEvent.WasFakeEvent) return;
            var match = nextEvent.match;

            var statCurrent = await _w3Stats.LoadMapsPerSeason(match.season) ?? MapsPerSeason.Create(match.season);
            var statOverall = await _w3Stats.LoadMapsPerSeason(-1) ?? MapsPerSeason.Create(-1);

            statCurrent.Count(match.map, match.gameMode);
            statOverall.Count(match.map, match.gameMode);

            await _w3Stats.Save(statCurrent);
            await _w3Stats.Save(statOverall);
        }
    }
}