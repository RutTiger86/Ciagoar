using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiagoarM.Commons.Messenger
{
    public class CheckViewHiddenMessage : ValueChangedMessage<bool>
    {
        public CheckViewHiddenMessage(bool IsHidden) : base(IsHidden)
        {

        }
    }
}
