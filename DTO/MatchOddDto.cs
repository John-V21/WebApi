﻿using System;

namespace Accepted.DTOs
{
    public class MatchOddDto
    {
        public int Id { get; set; }

        public char Specifier { get; set; }

        public Decimal Odd { get; set; }

        public int MatchId { get; set; }
    }
}
