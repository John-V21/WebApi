
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

Inject validators on startup (injects FluentValidator)

```C#
services.AddFluentValidation( c => c.Add<MatchOddValidator>().Add<MatchValidator>()
);
```

manually test an object using injecsted validators

```C#
fluentValidator.ThrowIfInvalid(match)
```


###### Docker Support

docker-compose build

docker-compose up

visit

http://localhost:5001/swagger
