﻿using Sportradar_API.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportradar_API_1.Model
{
    public class Game
    {
        public int ID { get; set; }

        public DateOnly? Dateofevent { get; set; }
        public TimeOnly? StartingTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        public Team? HomeTeam { get; set; }

        public Team? AwayTeam { get; set; }

        public Season? Season { get; set; }

        public Result? Result { get; set; }
        public Sport? Sport_Game { get; set; }

        public Game(int id, DateOnly? dateofevent, TimeOnly? startingTime, TimeOnly? endTime, Team? homeTeam, Team? awayTeam, Season? season, Result? result, Sport? sport_game)
        {
            ID = id;
            Dateofevent = dateofevent;
            StartingTime = startingTime;
            EndTime = endTime;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Season = season;
            Result = result;
            Sport_Game = sport_game;
        }

        public Game()
        {


        }
    }
}
