namespace Cake.Prca.Issues.Markdownlint.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Core.Diagnostics;
    using Testing;

    internal class MarkdownlintIssuesProviderFixture
    {
        public MarkdownlintIssuesProviderFixture(string fileResourceName)
        {
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.Markdownlint.Tests.Testfiles." + fileResourceName))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.Settings =
                        MarkdownlintIssuesSettings.FromContent(
                            sr.ReadToEnd());
                }
            }

            this.PrcaSettings =
                new ReportIssuesToPullRequestSettings(@"c:\Source\Cake.Prca");
        }

        public FakeLog Log { get; set; }

        public MarkdownlintIssuesSettings Settings { get; set; }

        public ReportIssuesToPullRequestSettings PrcaSettings { get; set; }

        public MarkdownlintIssuesProvider Create()
        {
            var provider = new MarkdownlintIssuesProvider(this.Log, this.Settings);
            provider.Initialize(this.PrcaSettings);
            return provider;
        }

        public IEnumerable<ICodeAnalysisIssue> ReadIssues()
        {
            var codeAnalysisProvider = this.Create();
            return codeAnalysisProvider.ReadIssues(PrcaCommentFormat.PlainText);
        }
    }
}
