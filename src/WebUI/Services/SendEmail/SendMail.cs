using MailKit.Net.Smtp;
using mentor_v1.Domain.Identity;
using MimeKit;

namespace WebUI.Services.SendEmail;


public class SendMail
{
    private readonly IWebHostEnvironment _environment;

    public SendMail(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    public bool SendEmailAsync(string toMail, string subject, string confirmLink)
    {
        if (_environment.IsDevelopment() || _environment.IsStaging())
        {
            subject = "TEST EMAIL" + "'" + subject + "'";
        }
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse("tramygeo@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toMail));
        var bccAddress = new MailboxAddress("", "");

        email.Bcc.Add(bccAddress);
        email.Subject = subject;

        var builder = new BodyBuilder();
        
        builder.HtmlBody = confirmLink + "<div style=\"font-weight: bold;\">Trân trọng, <br>\r\n        <div style=\"color: #FF630E;\">Bộ phận hỗ trợ học viên BSMART</div>\r\n    </div>\r\n<br>    <img src=\"cid:image1\" alt=\"\" width=\"200px\">\r\n    <br>\r\n    <br>\r\n    <div>\r\n        Email liên hệ: admin@bsmart.edu.vn\r\n    </div>\r\n    <div>Số điện thoại: 028 9999 79 39</div>\r\n</div>";
        // Khởi tạo phần đính kèm của email (ảnh)
        var attachment = builder.LinkedResources.Add("wwwroot/files/icon-logo-along.webp");
        attachment.ContentId = "image1"; // Thiết lập Content-ID cho phần đính kèm

        email.Body = builder.ToMessageBody();
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate("", " ");
        try
        {
            smtp.Send(email);

        }
        catch (SmtpCommandException ex)
        {
            return false;
        }
        smtp.Disconnect(true);
        return true;

    }


    public bool SendEmailNoBccAsync(string toMail, string subject, string confirmLink)
    {


        if (_environment.IsDevelopment() || _environment.IsStaging())
        {
           
        }
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse("tramygeo@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toMail));
        email.Subject = subject;

        var builder = new BodyBuilder();

        builder.HtmlBody = confirmLink + "<div style=\"font-weight: bold;\">Trân trọng, <br>\r\n        <div style=\"color: #FF630E;\">Bộ phận Nhân sự TechGenius</div>\r\n    </div>\r\n<br>    <img src=\"cid:image1\" alt=\"\" width=\"200px\">\r\n    <br>\r\n    <br>\r\n    <div>\r\n        Email liên hệ: admin@bsmart.edu.vn\r\n    </div>\r\n    <div>Số điện thoại: 028 9999 79 39</div>\r\n</div>";
        // Khởi tạo phần đính kèm của email (ảnh)
        var attachment = builder.LinkedResources.Add("wwwroot/files/icon-logo-along.webp");
        attachment.ContentId = "image1"; // Thiết lập Content-ID cho phần đính kèm

        email.Body = builder.ToMessageBody();
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        smtp.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate("tramygeo@gmail.com", "shvmqyxqovhiapgh");
        try
        {
            smtp.Send(email);

        }
        catch (SmtpCommandException ex)
        {
            return false;
        }
        smtp.Disconnect(true);
        return true;

    }


    public bool SendEmailBcc(List<ApplicationUser> listMail, string subject, string confirmLink)
    {
        if (_environment.IsDevelopment() || _environment.IsStaging())
        {
            subject = "TEST EMAIL" + "'" + subject + "'";
        }
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse(""));
        var bccAdminAddress = new MailboxAddress("admin", "");

        email.Bcc.Add(bccAdminAddress);
        foreach (var item in listMail)
        {
            var bccAddress = new MailboxAddress(item.UserName, item.Email);

            email.Bcc.Add(bccAddress);
        }
        
        email.Subject = subject;

        var builder = new BodyBuilder();

        builder.HtmlBody = confirmLink + "<div style=\"font-weight: bold;\">Trân trọng, <br>\r\n        <div style=\"color: #FF630E;\">Bộ phận hỗ trợ học viên BSMART</div>\r\n    </div>\r\n<br>    <img src=\"cid:image1\" alt=\"\" width=\"200px\">\r\n    <br>\r\n    <br>\r\n    <div>\r\n        Email liên hệ: admin@bsmart.edu.vn\r\n    </div>\r\n    <div>Số điện thoại: 028 9999 79 39</div>\r\n</div>";
        // Khởi tạo phần đính kèm của email (ảnh)
        var attachment = builder.LinkedResources.Add("wwwroot/files/icon-logo-along.webp");
        attachment.ContentId = "image1"; // Thiết lập Content-ID cho phần đính kèm

        email.Body = builder.ToMessageBody();
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate("", "");
        try
        {
            smtp.Send(email);

        }
        catch (SmtpCommandException ex)
        {
            return false;
        }
        smtp.Disconnect(true);
        return true;

    }
}
