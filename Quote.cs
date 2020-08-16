namespace MainServer
{
    public class Quote
    {
        public string Who { get; set; }
        public string How { get; set; }
        public string Does { get; set; }
        public string What { get; set; }

        public override string ToString()
        {
            return Who + " " + How + " " + Does + " " + What + "\n";
        }
    }
}