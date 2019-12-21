using System;
using System.Net;
using System.Net.Mail;

namespace BusinessLogic.Services
{
    public class MessageService
    {
        private readonly SmtpClient _smtp = new SmtpClient("smtp.gmail.com", 587);
        public void SendEmail(string email, string meetingName, DateTime dateTime)
        {
            var from = new MailAddress("somemail@gmail.com", "Client");
            var to = new MailAddress(email);
            var m = new MailMessage(from, to)
            {
                Subject = "Приглашение на встречу " + meetingName,
                Body = "<h2>Вы приглашены на встречу " + dateTime.ToShortDateString() + " в " +
                       dateTime.ToShortTimeString() + "</h2>",
                IsBodyHtml = true
            };

            _smtp.Credentials = new NetworkCredential("somemail@gmail.com", "mypassword");
            _smtp.EnableSsl = true;
            _smtp.Send(m);
        }
    }
}