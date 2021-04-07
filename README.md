*DotNet Core WebApi using DI, EF, Services, Controllers, DTOs, AutoMapper, Swagger, MSSql Server*

###### FluentValidation using DI and manual validation

Inject validators on startup (injects FluentValidator)

```
services.AddFluentValidation( c => c.Add<MatchOddValidator>().Add<MatchValidator>()
);
```

manually test an object using injecsted validators

```
fluentValidator.ThrowIfInvalid(match)
```


###### Docker Support

docker-compose build

docker-compose up

http://localhost:5001/swagger
