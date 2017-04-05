namespace Cake.Prca.Issues.Markdownlint
{
    using Core;
    using Core.Annotations;
    using Core.IO;

    /// <summary>
    /// Contains functionality related to importing code analysis issues from Markdownlint
    /// to write them to pull requests.
    /// </summary>
    [CakeAliasCategory(CakeAliasConstants.MainCakeAliasCategory)]
    [CakeNamespaceImport("Cake.Prca.Issues.Markdownlint")]
    public static class MarkdownlintProviderAliases
    {
        /// <summary>
        /// Gets an instance of a provider for code analysis issues reported by Markdownlint using a log file from disk.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logFilePath">Path to the the Markdownlint log file.</param>
        /// <returns>Instance of a provider for code analysis issues reported by Markdownlint.</returns>
        /// <example>
        /// <para>Report code analysis issues reported by Markdownlint to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     var repoRoot = new DirectoryPath("c:\repo");
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MarkdownlintFromFilePath(
        ///             new FilePath("C:\build\Markdownlint.log")),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         repoRoot);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ICodeAnalysisProvider MarkdownlintFromFilePath(
            this ICakeContext context,
            FilePath logFilePath)
        {
            context.NotNull(nameof(context));
            logFilePath.NotNull(nameof(logFilePath));

            return context.Markdownlint(MarkdownlintSettings.FromFilePath(logFilePath));
        }

        /// <summary>
        /// Gets an instance of a provider for code analysis issues reported by JMarkdownlint using log file content.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logFileContent">Content of the the Markdownlint log file.</param>
        /// <returns>Instance of a provider for code analysis issues reported by Markdownlint.</returns>
        /// <example>
        /// <para>Report code analysis issues reported by Markdownlint to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     var repoRoot = new DirectoryPath("c:\repo");
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         MarkdownlintFromContent(
        ///             logFileContent),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         repoRoot);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ICodeAnalysisProvider MarkdownlintFromContent(
            this ICakeContext context,
            string logFileContent)
        {
            context.NotNull(nameof(context));
            logFileContent.NotNullOrWhiteSpace(nameof(logFileContent));

            return context.Markdownlint(MarkdownlintSettings.FromContent(logFileContent));
        }

        /// <summary>
        /// Gets an instance of a provider for code analysis issues reported by Markdownlint using specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">Settings for reading the Markdownlint log.</param>
        /// <returns>Instance of a provider for code analysis issues reported by Markdownlint.</returns>
        /// <example>
        /// <para>Report code analysis issues reported by Markdownlint to a TFS pull request:</para>
        /// <code>
        /// <![CDATA[
        ///     var repoRoot = new DirectoryPath("c:\repo");
        ///     var settings =
        ///         new MarkdownlintSettings(
        ///             new FilePath("C:\build\Markdownlint.log"));
        ///
        ///     ReportCodeAnalysisIssuesToPullRequest(
        ///         Markdownlint(settings),
        ///         TfsPullRequests(
        ///             new Uri("http://myserver:8080/tfs/defaultcollection/myproject/_git/myrepository"),
        ///             "refs/heads/feature/myfeature",
        ///             TfsAuthenticationNtlm()),
        ///         repoRoot);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory(CakeAliasConstants.CodeAnalysisProviderCakeAliasCategory)]
        public static ICodeAnalysisProvider Markdownlint(
            this ICakeContext context,
            MarkdownlintSettings settings)
        {
            context.NotNull(nameof(context));
            settings.NotNull(nameof(settings));

            return new MarkdownlintProvider(context.Log, settings);
        }
    }
}
