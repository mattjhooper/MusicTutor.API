https://hamidmosalla.com/2018/11/25/stop-using-repository-pattern-with-an-orm/
https://lostechies.com/jimmybogard/2012/10/08/favor-query-objects-over-repositories/
https://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework-core/
https://www.edgesidesolutions.com/cqrs-with-entity-framework-core/


https://ardalis.com/moving-from-controllers-and-actions-to-endpoints-with-mediatr/

# Watch Command
dotnet watch --project .\Unit.Tests\MusicTutor.Api.UnitTests test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info

# Test Coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
reportgenerator -reports:"C:\dev\MusicTutor.API\Unit.Tests\MusicTutor.Api.UnitTests\TestResults\e062ba8f-c202-4a0b-8df2-6b8e0f075f62\coverage.cobertura.xml" -targerdir:"coverageresults" -reporttypes:Html