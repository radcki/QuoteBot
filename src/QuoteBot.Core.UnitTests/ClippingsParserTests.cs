using QuoteBot.Core.Services.ClippingsLoader;
using Shouldly;

namespace QuoteBot.Core.UnitTests
{
    public class ClippingsParserTests
    {
        [Fact]
        public void ParseClippings_ShouldReturnExpectedResult_ForSingleClipping()
        {
            // Arrange
            var parser = new ClippingsParser();
            var intput = @"Piekni, dwudziestoletni - Marek Hłasko (Marek Hłasko)
- Your Highlight on Location 2836-2837 | Added on Wednesday, February 8, 2023 8:48:03 PM

Bo wtedy jeszcze nie wiedziałem, że świat dzieli się na dwie połowy, z tym jednak, że w jednej z nich jest nie do życia, w drugiej nie do wytrzymania.";
            var expectedTitle = "Piekni, dwudziestoletni";
            var epxectedAuthor = "Marek Hłasko";
            var expectedContent = "Bo wtedy jeszcze nie wiedziałem, że świat dzieli się na dwie połowy, z tym jednak, że w jednej z nich jest nie do życia, w drugiej nie do wytrzymania.";

            // Act
            var clippings = parser.ParseClippings(intput).ToList();

            // Assert
            clippings.Count.ShouldBe(1);
            var clipping = clippings[0];
            clipping.Author.ShouldBe(epxectedAuthor);
            clipping.BookTitle.ShouldBe(expectedTitle);
            clipping.Content.ShouldBe(expectedContent);

        }

        [Fact]
        public void ParseClippings_ShouldReturnExpectedResult_ForSingleClipping2()
        {
            // Arrange
            var parser = new ClippingsParser();
            var intput = @"Wybrane diatryby i Encheiridion. Stoicka s - Epictetus (Epiktet) (Epictetus (Epiktet))
- Your Highlight on Location 914-916 | Added on Wednesday, January 1, 2025 11:03:12 PM

„Skoro ten człowiek skrzywdził samego siebie, zachowując się wobec mnie w niesprawiedliwy sposób, czy nie mogę skrzywdzić samego siebie, podejmując niesprawiedliwe działania zwrócone przeciwko niemu?”. Dlaczego nie wyobrażamy sobie czegoś w tym rodzaju?";
            var expectedTitle = "Wybrane diatryby i Encheiridion. Stoicka s";
            var epxectedAuthor = "Epictetus (Epiktet)";
            var expectedContent = @"„Skoro ten człowiek skrzywdził samego siebie, zachowując się wobec mnie w niesprawiedliwy sposób, czy nie mogę skrzywdzić samego siebie, podejmując niesprawiedliwe działania zwrócone przeciwko niemu?”. Dlaczego nie wyobrażamy sobie czegoś w tym rodzaju?";

            // Act
            var clippings = parser.ParseClippings(intput).ToList();

            // Assert
            clippings.Count.ShouldBe(1);
            var clipping = clippings[0];
            clipping.Author.ShouldBe(epxectedAuthor);
            clipping.BookTitle.ShouldBe(expectedTitle);
            clipping.Content.ShouldBe(expectedContent);

        }
    }
}
