using CleanArchitecture.Application.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Mail
{
    public class EmailSender : IEmailSender
    {
        public async Task<bool> SendEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
