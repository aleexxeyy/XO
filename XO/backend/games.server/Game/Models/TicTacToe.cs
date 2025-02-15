using Game.Convertors;
using System;
using System.Text.Json.Serialization;

namespace Game.Models
{
    public class TicTacToe
    {
        public Guid Id { get; init; }
        [JsonConverter(typeof(StringArrayJsonConverter))]
        public string[,] Board { get; init; } = new string[3, 3];
        public string? PlayerX { get; set; }
        public string? PlayerO { get; set; }
        public string? CurrentPlayer { get; set; }
        public string? Winner { get; set; }
        public bool IsGameOver { get; set; } = false;
    }
}
