using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Text.Json;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Assertion;

internal class JsonElementEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(
        Comparands comparands,
        IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator
    )
    {
        if (comparands.Subject is not JsonElement subject)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        subject.ToString()
            .Should()
            .Be(comparands.Expectation.ToString(), context.Reason.FormattedMessage, context.Reason.Arguments);

        return EquivalencyResult.AssertionCompleted;
    }
}
