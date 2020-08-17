namespace MainServer
{
    public class Response
    {
        private Quote quote;
        private UsersList users;

        public Response(Quote quote, UsersList users)
        {
            this.quote = quote;
            this.users = users;
        }

        public string GetResponse()
        {
            return quote.ToString() + "<br>\n" + users.GetUserOperationList();
        }

        public void Clear()
        {
            users.Clear();
        }
    }
}