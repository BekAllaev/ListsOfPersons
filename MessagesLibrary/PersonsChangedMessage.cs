using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace MessagesLibrary
{
    public enum CRUD { Create, Read, Update, Delete }
    /// <summary>
    /// Instance of this type incasulate information about collection changing
    /// </summary>
    public class PersonsChangedMessage : MessageBase
    {
        public bool IsAvailable;

        public CRUD OperationType { set; get; }
    }
}
