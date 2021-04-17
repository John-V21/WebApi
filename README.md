
*DotNet Core WebApi using DI, EF, Services, Controllers, DTOs, AutoMapper, Swagger, MSSql Server*

###### Tables

Match
ID | Description | MatchDate | MachTime | TeamA | TeamB | Sport
-- | ----------- | --------- | -------- | ----- | ----- | -----
1 | TeamA vs TeamB | 19/03/2021 | 12:00 | team a | team b | 1

MatchOdd
ID | MatchId | Specifier | Odd
-- | ------- | --------- | ----
1  | 1 | X | 1.2

###### FluentValidation using DI and manual validation

Create validator for a class (eg Match)

```C#
    public class MatchValidator : AbstractValidator<Match>
    {
        public MatchValidator()
        {
            RuleFor(x => x.TeamA).Must((model, field) => model.TeamA != model.TeamB).WithMessage("TeamA and TeamB cannot be equal");
        }
    }
```

Inject validators on startup (injects FluentValidator service)

```C#
services.AddFluentValidation( c => c.Add<MatchOddValidator>().Add<MatchValidator>()
);
```

manually test an object using injected FluentValidator service

```C#
fluentValidator.ThrowIfInvalid(match)
```


###### Docker Support

docker-compose build

docker-compose up

visit

http://localhost:5001/swagger
