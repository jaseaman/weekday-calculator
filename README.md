# weekday-calculator
**Total Development Time:** 8h

Project that calculates amount of weekdays over a period of time.

Optimal speed was the priority of the project. With requests ranging over 500 years taking
on average 30ms per request.

Holiday dates are stored in a Postgres database, and have different placement strategies,
that are extendable through code.

The list of public holidays (Of the 3 types expected) have been taken from:
https://www.industrialrelations.nsw.gov.au/public-holidays/public-holidays-in-nsw/

## Metrics for Success

### RE: Validity
Proof that the application works for a number of normal and edge case scenarios can be explored through the various TDD test cases created.

### RE: Maintainability
Follows SOLID with minimal code re-use. Uses thin controllers as a wrapper for business logic.

### RE: Extensibility
Additional placement strategies for weekends for more complicated logic (i.e. Easter) can be achieved independently of altering other methodologies

### RE: Performance
The calculations are done in two batches. Demonstrated below

```
| 2000, 2001, 2002, 2003, 2004  |   2005 (163 days)  |
|--------- Full Years --------- | -- Partial Year -- |
```

This is a very quick solution, with time-frames of over 500 years taking ~30ms. This is consistent regardless of scale.
Additional optimisations could be achieved with further asynchronous batching, but the startup cost of the additional workers may cause more performance implications than it solves.

## Usage
1. Run `docker-compose.yaml` file in WeekdayCalculator.Api `docker-compose up -d`
2. Run SQL script `initial.sql` in `./infrastructure` to initialise table and holiday data
3. Run Api project with `ASPNETCORE_ENVIRONMENT=local`
4. Visit `http://localhost:5000/swagger`
5. Use swagger to interact with the api

## Testing
Unit tests were used as a part of TDD with this project. With manual calculation of total business
days within a range, then writing functionality to cause the tests to pass.

1. Run `dotnet test`