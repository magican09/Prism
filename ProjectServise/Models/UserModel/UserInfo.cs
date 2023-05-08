using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectServise
{
   public  class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UsersSecurityGroup UsersSecurityGroup { get; set; }
    }
}
