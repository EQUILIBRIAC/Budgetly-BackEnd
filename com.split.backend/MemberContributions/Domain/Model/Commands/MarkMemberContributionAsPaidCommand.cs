namespace com.split.backend.MemberContributions.Domain.Model.Commands;

public record MarkMemberContributionAsPaidCommand(string Id, decimal? Amount);
