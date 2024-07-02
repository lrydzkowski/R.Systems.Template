namespace R.Systems.Template.Core.Common.Domain;

public class Element
{
    public long Id { get; init; }

    public string Name { get; init; } = "";

    public string? Description { get; init; }

    public int Value { get; init; }

    public int? AdditionalValue { get; init; }

    public long BigValue { get; init; }

    public long? BigAdditionalValue { get; init; }

    public decimal Price { get; init; }

    public decimal? Discount { get; init; }

    public DateTime CreationDate { get; init; }

    public DateTime? UpdateDate { get; init; }

    public DateTime CreationDateTime { get; set; }

    public DateTime? UpdateDateTime { get; init; }

    public bool IsNew { get; init; }

    public bool? IsActive { get; init; }
}
