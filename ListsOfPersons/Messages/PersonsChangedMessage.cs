using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace ListsOfPersons.Messages
{
    public enum CRUD { Create, Read, Update, Delete }
    class PersonsChangedMessage : MessageBase
    {
        public string ExceptionMessage;

        public CRUD OperationType { set; get; }
    }
}
