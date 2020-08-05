using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Model;

namespace Taxi.Web.Helpers
{
   public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
