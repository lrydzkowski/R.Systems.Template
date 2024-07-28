using Bogus;
using R.Systems.Template.Infrastructure.PostgreSqlDb;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Api.DataGeneratorCli.Services;

internal class ElementService
{
    private readonly AppDbContext _appDbContext;
    private readonly Random _random;

    public ElementService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _random = new Random();
    }

    public async Task CreateElementsAsync(int elementsCount)
    {
        List<ElementEntity> elements = BuildElementEntities(elementsCount);
        await _appDbContext.Elements.AddRangeAsync(elements);
        await _appDbContext.SaveChangesAsync();
    }

    private List<ElementEntity> BuildElementEntities(int elementsCount)
    {
        return Enumerable.Range(1, elementsCount)
            .Select(_ => BuildElementEntityFaker().Generate())
            .ToList();
    }

    private Faker<ElementEntity> BuildElementEntityFaker()
    {
        return new Faker<ElementEntity>().RuleFor(
                x => x.Name,
                faker => faker.Commerce.ProductName()
            )
            .RuleFor(
                x => x.Description,
                faker => ShouldGenerateValue() ? faker.Lorem.Sentences() : null
            )
            .RuleFor(
                x => x.Value,
                _ => _random.Next(0, int.MaxValue)
            )
            .RuleFor(
                x => x.AdditionalValue,
                _ => ShouldGenerateValue() ? _random.Next(0, int.MaxValue) : null
            )
            .RuleFor(
                x => x.BigValue,
                _ => _random.NextInt64(0, long.MaxValue)
            )
            .RuleFor(
                x => x.BigAdditionalValue,
                _ => ShouldGenerateValue() ? _random.NextInt64(0, long.MaxValue) : null
            )
            .RuleFor(
                x => x.Price,
                _ => (decimal)_random.NextDouble() * _random.Next(0, int.MaxValue)
            )
            .RuleFor(
                x => x.Discount,
                _ => ShouldGenerateValue() ? (decimal)_random.NextDouble() * _random.Next(0, int.MaxValue) : null
            )
            .RuleFor(
                x => x.CreationDate,
                faker => DateOnly.FromDateTime(faker.Date.Past().Date)
            )
            .RuleFor(
                x => x.UpdateDate,
                faker => ShouldGenerateValue() ? DateOnly.FromDateTime(faker.Date.Past().Date) : null
            )
            .RuleFor(
                x => x.CreationDateTime,
                faker => faker.Date.Past().ToUniversalTime()
            )
            .RuleFor(
                x => x.UpdateDateTime,
                faker => ShouldGenerateValue() ? faker.Date.Past().ToUniversalTime() : null
            )
            .RuleFor(
                x => x.IsNew,
                _ => _random.Next(0, 2) == 1
            )
            .RuleFor(
                x => x.IsActive,
                _ => ShouldGenerateValue() ? _random.Next(0, 2) == 1 : null
            );
    }

    private bool ShouldGenerateValue()
    {
        return _random.Next(0, 2) == 1;
    }
}
