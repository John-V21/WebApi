using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accepted.Models
{


    [Table("Match")]
    public class Match: IValidatableObject
    {
        public enum SportType
        {
            Football = 1,
            Basketball
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime MatchDate { get; set; }

        [Required]
        [Column(TypeName = "Time")]
        [DataType(DataType.Time)]
        public TimeSpan MatchTime { get; set; }

        [Required]
        public string TeamA { get; set; }

        [Required]
        public string TeamB { get; set; }

        [Required]
        [Column(TypeName = "Numeric(1)")]
        public SportType Sport { get; set; }


        public List<MatchOdd> MatchOdds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.TeamA == this.TeamB)
            {
                yield return new ValidationResult("TeamA and TeamB must be different");
            }
        }
    }
}
