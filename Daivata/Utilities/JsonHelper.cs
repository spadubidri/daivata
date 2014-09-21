using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.UI
{
    public class JsonHelper
    {

        public static StatusForm GetStatusForm(bool status, string message)
        {
            StatusForm returnStatusForm = new StatusForm();
            returnStatusForm.Status = status;
            returnStatusForm.Messages = new Collection<string>();
            returnStatusForm.Messages.Add(message);
            return returnStatusForm;
        }

    }

    public class StatusForm
    {
        public bool Status;
        public IList<string> Messages;
    }
}
