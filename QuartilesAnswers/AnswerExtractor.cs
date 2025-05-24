using HtmlAgilityPack;
using Paths;
using Updater;

class AnswerExtractor
{
    static async Task Main(string[] args)
    {
        var paths = new QuartilePaths(true);
        var updater = new DictionaryUpdater();

        // The first quartiles game was released on this date
        DateTime endDate = new DateTime(2024, 5, 10);

        for (DateTime date = DateTime.Today; date >= endDate; date = date.AddDays(-1))
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string outputPath = Path.Combine(paths.QuartilesAnswersFolder, $"quartiles-answers-{formattedDate}.txt");

            if(File.Exists(outputPath))
            {
                Console.WriteLine($"Answers for {formattedDate} already exist. Skipping.");
                continue;
            }

            string url = $"https://quartilesanswers.com/quartiles/{formattedDate}";

            var httpClient = new HttpClient();
            string html;

            try
            {
                html = await httpClient.GetStringAsync(url);
            }

            catch
            {
                Console.WriteLine($"Failed to load url {url}. Skipping date {formattedDate}.");
                continue;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Only select words within the div with class 'list-none flex-wrap' (this contains the answers on the website)
            var wordList = doc.DocumentNode.SelectNodes("//div[contains(@class, 'list-none') and contains(@class, 'flex-wrap')]/p");

            if (wordList != null)
            {
                var words = wordList.Select(node => node.InnerText.Trim().ToLower()).ToHashSet();

                // Write all words, except the last one, with newline; last one without newline
                await using var writer = new StreamWriter(outputPath);

                for (int i = 0; i < words.Count; i++)
                {
                    if (i < words.Count - 1)
                    {
                        await writer.WriteLineAsync(words.ElementAt(i));
                    }

                    else
                    {
                        await writer.WriteAsync(words.ElementAt(i)); // no newline
                    }
                }

                updater.AddUpdate(words);

                Console.WriteLine($"Saved answers to {outputPath}.");
            }

            else
            {
                Console.WriteLine("No answers found on this page.");
            }
        }
    }
}