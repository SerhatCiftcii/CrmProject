﻿using System.Threading.Tasks;

namespace CrmProject.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
