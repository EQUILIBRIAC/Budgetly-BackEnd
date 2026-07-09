using com.split.backend.MemberContributions.Domain.Model.Aggregates;
using com.split.backend.MemberContributions.Domain.Model.Commands;
using com.split.backend.MemberContributions.Domain.Model.ValueObjects;
using Xunit;

namespace com.split.backend.Tests;

public class MemberContributionMarkAsPaidTests
{
    [Fact]
    public void MarkAsPaid_WithoutAmount_UsesExistingAmountAndSetsDone()
    {
        var contribution = new MemberContribution("CN-1", "HM-1", 90m, (int)EStatus.Pending);

        contribution.MarkAsPaid();

        Assert.Equal(90m, contribution.Amount);
        Assert.Equal(EStatus.Done, contribution.Status);
        Assert.True(contribution.PayedAt.Year >= 2020);
    }

    [Fact]
    public void MarkAsPaid_WithAmount_UpdatesAmountAndSetsDone()
    {
        var contribution = new MemberContribution("CN-1", "HM-1", 90m, (int)EStatus.Pending);

        contribution.MarkAsPaid(100.08m);

        Assert.Equal(100.08m, contribution.Amount);
        Assert.Equal(EStatus.Done, contribution.Status);
    }

    [Fact]
    public void MarkAsPaid_WithInvalidAmount_Throws()
    {
        var contribution = new MemberContribution("CN-1", "HM-1", 90m, (int)EStatus.Pending);

        Assert.Throws<ArgumentException>(() => contribution.MarkAsPaid(0m));
    }

    [Fact]
    public void CreateMemberContribution_StartsAsPending()
    {
        var contribution = new MemberContribution(new CreateMemberContributionCommand("CN-1", "HM-1", 50m));

        Assert.Equal(EStatus.Pending, contribution.Status);
        Assert.Equal("HM-1", contribution.MemberId);
    }
}
