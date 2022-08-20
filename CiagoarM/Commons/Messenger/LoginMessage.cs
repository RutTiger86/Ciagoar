using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiagoarM.Commons.Messenger
{
    public class LoginMessage : ValueChangedMessage<bool>
    {
        public LoginMessage(bool LoginResult) : base(LoginResult)
        {

        }
    }
}
