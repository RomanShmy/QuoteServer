namespace MainServer
{
    public class Quote
    {
        public string Who { get; set; }
        public string How { get; set; }
        public string Does { get; set; }
        public string What { get; set; }

        public void GetQuoteFromString(string quote)
        {
            string[] words = quote.Split(" ");
            this.Who = words[0];
            this.How = words[1];
            this.Does = words[2];
            this.What = words[3];
        }
        

        public override string ToString()
        {
            return Who + " " + How + " " + Does + " " + What + "\n";
        }
    }
}