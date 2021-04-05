using System;

namespace Accepted.DTOs
{
    public class MatchDto
    {
        public enum SportTypeDto
        {
            Football = 1,
            Basketball
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime MatchDateTime { get; set; }

        public string TeamA { get; set; }

        public string TeamB { get; set; }

        public SportTypeDto Sport { get; set; }
    }
}