using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accepted.Models
{
    [Table("MatchOdd")]
    public class MatchOdd
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Specifier { get; set; }

        [Required]
        [Column(TypeName = "decimal(3, 2)")]
        public Decimal Odd { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
