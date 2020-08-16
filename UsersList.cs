using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
namespace MainServer
{
    public class UsersList
    {
        private List<User> userOperation;
        public UsersList()
        {
            userOperation = new List<User>();
        }

        public void AddUser(User user)
        {
            userOperation.Add(user);
        }

        public string GetUserOperationList()
        {
            StringBuilder result = new StringBuilder();
            foreach(var user in userOperation)
            {
                var usr = user.Name;
                var operation = user.Operation;
                result.Append("\"" + operation + "\"" + " received from " + usr);
                result.Append("<br>\n");
            }

            return result.ToString();
        }

        public void Clear()
        {
            userOperation.Clear();
        }
    }
}