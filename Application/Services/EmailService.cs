﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Application.Interfaces.Services;
using MobileMend.Application.DTOs;
using Application.Interfaces.Repositories;

namespace Application.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IEmailRepository emailRepository;
        public EmailService(IConfiguration config,IEmailRepository _emailRepository)
        {
            _config = config;
            emailRepository = _emailRepository;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Smtp:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }


        public async Task<ResponseDTO<object>> ConfirmEmail(string email, string token)
        {
            try
            {
                var user = await emailRepository.GetUserByEmailAsync(email);
                if (user == null || user.EmailVerificationToken != token)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Invalid token or email." };
                }

                await emailRepository.UpdateConfirmation(user.UserID);

                return new ResponseDTO<object> { StatusCode = 200, Message = "Email confirmed successfully. You can now log in." };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Message = "An error occurred while confirming the email." };
            }
        }

    }
}
