using System.Collections.Generic;
namespace MainServer
{
    public class DataStorage
    {
        
        public string[] Who { get; set; } = {"who", "Барсик", "Собака"};
        public string[] How { get; set; } = {"how", "красиво", "глупо", "плохо"};
        public string[] Does { get; set; } = {"does", "пишет", "рисует", "танцует"};
        public string[] What { get; set; } = {"what", "код", "танго", "море"};

        public List<string[]> GetQuoteElements()
        {
            List<string[]> quoteElements = new List<string[]>();
            quoteElements.Add(Who);
            quoteElements.Add(How);
            quoteElements.Add(Does);
            quoteElements.Add(What);

            return quoteElements;
        }
    }
}