using Ciagoar.Core.Helper;
using Ciagoar.Data.HTTPS;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace CiagoarS.Common
{
    public class EmailHelper
    {
        public static bool SendEMail(SMTP_INFO sMTP_INFO, UserInfo user, ILogger<BaseController> _mLogger, [CallerMemberName] string propertyName = "")
        {
            bool result = true;

            SmtpClient smtpClient = new ();
            MailMessage mailMessage = new ();

            try
            {

                string Key = CryptographyHelper.GetHash(user.CreateTime.ToString("HHmmssfff")).ToString()[..6];

                string sTitle = "CIAGOAR USER AUTHENTICATION";
                
                string Body = $"Your Authentication Key {Environment.NewLine} {Key} {Environment.NewLine} Please Input Next Login One Time";

                // 보내는 사람
                mailMessage.From = new MailAddress(sMTP_INFO.sSMTPUser, "CIAGOAR");

                // 받는 사람
                mailMessage.To.Add(new MailAddress(user.Email, user.Nickname));

                // 제목
                mailMessage.Subject = sTitle;

                // 내용
                //mailMessage.IsBodyHtml = true;
                mailMessage.Body = Body;

                // 전송
                smtpClient.Host = sMTP_INFO.sSMTPServer;
                smtpClient.Port = sMTP_INFO.nSMTPPort;
                smtpClient.EnableSsl = true;
                //_mSmtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(sMTP_INFO.sSMTPUser, sMTP_INFO.sSMTPPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception Exp)
            {
                _mLogger.LogError($"[{propertyName}] Detail- {Exp.Message}{Environment.NewLine}");
                result = false;
            }

            return result;
        }
    }
}
