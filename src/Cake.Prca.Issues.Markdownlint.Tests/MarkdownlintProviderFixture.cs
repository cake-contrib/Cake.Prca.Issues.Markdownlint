namespace Cake.Prca.Issues.Markdownlint.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Core.Diagnostics;
    using Testing;

    internal class MarkdownlintProviderFixture
    {
        public MarkdownlintProviderFixture(string fileResourceName)
        {
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.Markdownlint.Tests.Testfiles." + fileResourceName))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.Settings =
                        MarkdownlintSettings.FromContent(
                            sr.ReadToEnd());
                }
            }

            this.PrcaSettings =
                new ReportCodeAnalysisIssuesToPullRequestSettings(@"c:\Source\Cake.Prca");
        }

        public FakeLog Log { get; set; }

        public MarkdownlintSettings Settings { get; set; }

        public ReportCodeAnalysisIssuesToPullRequestSettings PrcaSettings { get; set; }

        public MarkdownlintProvider Create()
        {
            var provider = new MarkdownlintProvider(this.Log, this.Settings);
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
