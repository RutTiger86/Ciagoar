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
            SmtpClient _mSmtpClient = new();
            MailMessage _mMailMessage = new ();
            try
            {

                string Key = CryptographyHelper.GetHash(user.Createtime.ToString("HHmmssfff")).ToString()[..6];
                string sTitle = "CIAGOAR USER AUTHENTICATION";
                string Body = $"Your Authentication Key {Environment.NewLine} {Key} {Environment.NewLine} Please Input Next Login One Time";

                // 보내는 사람
                _mMailMessage.From = new MailAddress(sMTP_INFO.sSMTPUser, "CIAGOAR");
                // 받는 사람
                _mMailMessage.To.Add(new MailAddress(user.Email, user.Nickname));
                // 제목
                _mMailMessage.Subject = sTitle;
                // 내용
                _mMailMessage.Body = Body;

                // 전송
                _mSmtpClient.Host = sMTP_INFO.sSMTPServer;
                _mSmtpClient.Port = sMTP_INFO.nSMTPPort;
                _mSmtpClient.EnableSsl = true;
                _mSmtpClient.Credentials = new NetworkCredential(sMTP_INFO.sSMTPUser, sMTP_INFO.sSMTPPassword);
                _mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                _mSmtpClient.Send(_mMailMessage);
                return true;
            }
            catch (Exception Exp)
            {
                _mLogger.LogError($"[{propertyName}] Detail- {Exp.Message}{Environment.NewLine}");
                return false;
            }

        }
    }
}
