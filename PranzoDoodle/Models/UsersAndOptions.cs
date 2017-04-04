using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PranzoDoodle.Models
{
    public class UsersAndOptions
    {
        public IEnumerable<PranzoUsers> Users { get; set; }
        public IEnumerable<PranzoOptions> Options { get; set; }

        public UsersAndOptions(IEnumerable<PranzoUsers> users, IEnumerable<PranzoOptions> options)
        {
            Users = users;
            Options = options;
        }
    }
}