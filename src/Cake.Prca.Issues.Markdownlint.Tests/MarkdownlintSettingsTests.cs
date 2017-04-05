namespace Cake.Prca.Issues.Markdownlint.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Shouldly;
    using Xunit;

    public class MarkdownlintSettingsTests
    {
        public sealed class TheMarkdownlintSettingsCtor
        {
            [Fact]
            public void Should_Throw_If_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MarkdownlintSettings.FromFilePath(null));

                // Then
                result.IsArgumentNullException("logFilePath");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MarkdownlintSettings.FromContent(null));

                // Then
                result.IsArgumentNullException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_Empty()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MarkdownlintSettings.FromContent(string.Empty));

                // Then
                result.IsArgumentOutOfRangeException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_WhiteSpace()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MarkdownlintSettings.FromContent(" "));

                // Then
                result.IsArgumentOutOfRangeException("logFileContent");
            }

            [Fact]
            public void Should_Set_Property_Values_Passed_To_Constructor()
            {
                // Given
                const string logFileContent = "foo";

                // When
                var settings = MarkdownlintSettings.FromContent(logFileContent);

                // Then
                settings.LogFileContent.ShouldBe(logFileContent);
            }

            [Fact]
            public void Should_Read_File_From_Disk()
            {
                var fileName = Path.GetTempFileName();
                try
                {
                    // Given
                    string expected;
                    using (var ms = new MemoryStream())
                    using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.Markdownlint.Tests.Testfiles.markdownlint.json"))
                    {
                        stream.CopyTo(ms);
                        var data = ms.ToArray();

                        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            file.Write(data, 0, data.Length);
                        }

                        expected = ConvertFromUtf8(data);
                    }

                    // When
                    var settings =
                        MarkdownlintSettings.FromFilePath(fileName);

                    // Then
                    settings.LogFileContent.ShouldBe(expected);
                }
                finally
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
            }

            private static string ConvertFromUtf8(byte[] bytes)
            {
                var enc = new UTF8Encoding(true);
                var preamble = enc.GetPreamble();

                if (preamble.Where((p, i) => p != bytes[i]).Any())
                {
                    throw new ArgumentException("Not utf8-BOM");
                }

                return enc.GetString(bytes.Skip(preamble.Length).ToArray());
            }
        }
    }
}
