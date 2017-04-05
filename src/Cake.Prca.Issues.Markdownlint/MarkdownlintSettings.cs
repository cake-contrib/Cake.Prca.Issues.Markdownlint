namespace Cake.Prca.Issues.Markdownlint
{
    using System.IO;
    using Core.IO;

    /// <summary>
    /// Settings for <see cref="MarkdownlintProvider"/>.
    /// </summary>
    public class MarkdownlintSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownlintSettings"/> class.
        /// </summary>
        /// <param name="logFilePath">Path to the the Markdownlint log file.</param>
        protected MarkdownlintSettings(FilePath logFilePath)
        {
            logFilePath.NotNull(nameof(logFilePath));

            using (var stream = new FileStream(logFilePath.FullPath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.LogFileContent = sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownlintSettings"/> class.
        /// </summary>
        /// <param name="logFileContent">Content of the the Markdownlint log file.</param>
        protected MarkdownlintSettings(string logFileContent)
        {
            logFileContent.NotNullOrWhiteSpace(nameof(logFileContent));

            this.LogFileContent = logFileContent;
        }

        /// <summary>
        /// Gets the content of the log file.
        /// </summary>
        public string LogFileContent { get; private set; }

        /// <summary>
        /// Returns a new instance of the <see cref="MarkdownlintSettings"/> class from a log file on disk.
        /// </summary>
        /// <param name="logFilePath">Path to the Markdownlintlog file.</param>
        /// <returns>Instance of the <see cref="MarkdownlintSettings"/> class.</returns>
        public static MarkdownlintSettings FromFilePath(FilePath logFilePath)
        {
            return new MarkdownlintSettings(logFilePath);
        }

        /// <summary>
        /// Returns a new instance of the <see cref="MarkdownlintSettings"/> class from the content
        /// of a Markdownlint log file.
        /// </summary>
        /// <param name="logFileContent">Content of the Markdownlint log file.</param>
        /// <returns>Instance of the <see cref="MarkdownlintSettings"/> class.</returns>
        public static MarkdownlintSettings FromContent(string logFileContent)
        {
            return new MarkdownlintSettings(logFileContent);
        }
    }
}
