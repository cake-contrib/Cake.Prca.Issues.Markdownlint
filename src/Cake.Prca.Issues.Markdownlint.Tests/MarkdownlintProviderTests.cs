namespace Cake.Prca.Issues.Markdownlint.Tests
{
    using System.Linq;
    using Core.IO;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MarkdownlintProviderTests
    {
        public sealed class TheMsBuildCodeAnalysisProviderCtor
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    new MarkdownlintProvider(
                        null,
                        MarkdownlintSettings.FromContent("Foo")));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                var result = Record.Exception(() =>
                    new MarkdownlintProvider(
                        new FakeLog(),
                        null));

                // Then
                result.IsArgumentNullException("settings");
            }
        }

        public sealed class TheReadIssuesMethod
        {
            [Fact]
            public void Should_Read_Issue_Correct()
            {
                // Given
                var fixture = new MarkdownlintProviderFixture("markdownlint.json");

                // When
                var issues = fixture.ReadIssues().ToList();

                // Then
                issues.Count.ShouldBe(3);
                CheckIssue(
                    issues[0],
                    @"bad.md",
                    3,
                    "MD010",
                    "https://github.com/DavidAnson/markdownlint/blob/master/doc/Rules.md#md010",
                    0,
                    "Hard tabs");
                CheckIssue(
                    issues[1],
                    @"bad.md",
                    1,
                    "MD018",
                    "https://github.com/DavidAnson/markdownlint/blob/master/doc/Rules.md#md018",
                    0,
                    "No space after hash on atx style header");
                CheckIssue(
                    issues[2],
                    @"bad.md",
                    3,
                    "MD018",
                    "https://github.com/DavidAnson/markdownlint/blob/master/doc/Rules.md#md018",
                    0,
                    "No space after hash on atx style header");
            }

            private static void CheckIssue(
                ICodeAnalysisIssue issue,
                string affectedFileRelativePath,
                int? line,
                string rule,
                string ruleUrl,
                int priority,
                string message)
            {
                if (issue.AffectedFileRelativePath == null)
                {
                    affectedFileRelativePath.ShouldBeNull();
                }
                else
                {
                    issue.AffectedFileRelativePath.ToString().ShouldBe(new FilePath(affectedFileRelativePath).ToString());
                    issue.AffectedFileRelativePath.IsRelative.ShouldBe(true, "Issue path is not relative");
                }

                issue.Line.ShouldBe(line);
                issue.Rule.ShouldBe(rule);

                if (issue.RuleUrl == null)
                {
                    ruleUrl.ShouldBeNull();
                }
                else
                {
                    issue.RuleUrl.ToString().ShouldBe(ruleUrl);
                }

                issue.Priority.ShouldBe(priority);
                issue.Message.ShouldBe(message);
            }
        }
    }
}
