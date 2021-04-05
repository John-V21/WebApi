using AutoMapper;
using Accepted.Models;
using Accepted.DTOs;
using System;

namespace Accepted
{
    public class AutoMapping : Profile
    {
        public class DateTimeConverter : IValueConverter<DateTime, TimeSpan>
        {
            public TimeSpan Convert(DateTime source, ResolutionContext context)
                => source.TimeOfDay;
        }

        public class DateTimeResolver : IValueResolver<Match, MatchDto, DateTime>
        {
            public DateTime Resolve(Match source, MatchDto destination, DateTime member, ResolutionContext context)
                => source.MatchDate + source.MatchTime;
        }

        public AutoMapping()
        {
            CreateMap<MatchDto, Match>()
                .ForMember(dest => dest.MatchDate, opt => opt.MapFrom(src => src.MatchDateTime))
                .ForMember(dest => dest.MatchTime, opt => opt.ConvertUsing(new DateTimeConverter(), src => src.MatchDateTime));

            CreateMap<Match, MatchDto>()
                .ForMember(dest => dest.MatchDateTime, opt => opt.MapFrom<DateTimeResolver>());

            CreateMap<MatchOddDto, MatchOdd>();
            CreateMap<MatchOdd, MatchOddDto>();

        }
    }
}