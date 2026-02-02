using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string recipient,string link);
    }
}
