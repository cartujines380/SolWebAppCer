using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace FE.Utileria
{
    public class clsEmail
    {
        public string Msg;

        public bool EnviarCorreo(string PI_Smtp, string PI_Puerto, string PI_UsuarioEnvia, string PI_CredencialUsr, string PI_CredencialPassw, string PI_MotivoCorreo, string PI_UsuariosDestinos, System.Data.DataTable PI_valores, List<AdjuntoEnvio> PI_ArrPDF, string PI_ArchivoHtmlCorreo, Boolean aplicassl)
        {
            Msg = "";
            bool lv_Retorno = false;
            try
            {
                string Archivo = PI_ArchivoHtmlCorreo; //PI_TipoCorreo + "_" + PI_RucEmisor + ".htm";
                string htmlBody;
                string RutaDocHtml = System.Configuration.ConfigurationManager.AppSettings["RutaCorreoNotificacion"];
                //TEST - COMENTAR
                //Archivo = "PlantillaGestionPedido.html";
                //TEST - COMENTAR

                if (System.IO.File.Exists(System.IO.Path.Combine(RutaDocHtml, Archivo)))
                {
                    htmlBody = System.IO.File.ReadAllText(System.IO.Path.Combine(RutaDocHtml, Archivo));
                }
                else
                {
                    //Archivo = PI_TipoCorreo + "_Defecto.htm";
                    //if (!System.IO.File.Exists(System.IO.Path.Combine(RutaDocHtml, Archivo)))
                    //{
                    throw new Exception("No existe Archivo HTML para Enviar por Correo.");
                    //}
                    //htmlBody = System.IO.File.ReadAllText(System.IO.Path.Combine(RutaDocHtml, Archivo));
                }

                int x1 = 0;

                while (x1 < PI_valores.Rows.Count)
                {

                    try
                    {
                        string PI_TituloPrincipal = PI_valores.Rows[x1]["TramaTitulo"].ToString();
                        string[] detvalore = PI_TituloPrincipal.Split('|');
                        foreach (string value in detvalore)
                        {
                            if (value.Trim() != "")
                            {
                                string[] vaname = value.Split(';');
                                htmlBody = htmlBody.Replace(vaname[0], vaname[1]);
                            }
                        }
                    }
                    catch (Exception extt)
                    {
                        new Exception("Error debe enviar TramaTitulo un valor por lo menos");
                    }

                    try
                    {
                        string PI_TituloPrincipalTramaBody = PI_valores.Rows[x1]["TramaBody"].ToString();
                        string[] detvaloreTramaBody = PI_TituloPrincipalTramaBody.Split('|');
                        foreach (string value in detvaloreTramaBody)
                        {
                            if (value.Trim() != "")
                            {
                                string[] vaname = value.Split(';');
                                htmlBody = htmlBody.Replace(vaname[0], vaname[1]);
                            }
                        }

                        #region Logos
                        string logoCabecera = System.Configuration.ConfigurationManager.AppSettings["LogoCabecera"]; ;
                        string logoFooter = System.Configuration.ConfigurationManager.AppSettings["LogoFooter"];
                        string imgHelp = System.Configuration.ConfigurationManager.AppSettings["imgHelp"];
                        string imgArrowRight = System.Configuration.ConfigurationManager.AppSettings["imgArrowRight"];

                        htmlBody = htmlBody.Replace("@@logoCabecera", logoCabecera);
                        htmlBody = htmlBody.Replace("@@logoFooter", logoFooter);
                        htmlBody = htmlBody.Replace("@@imgHelp", imgHelp);
                        htmlBody = htmlBody.Replace("@@imgArrowRight", imgArrowRight);

                        #endregion

                    }
                    catch (Exception extt)
                    {
                        new Exception("Error debe enviar TramaBody un valor por lo menos");
                    }
                    x1++;
                }


                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                    (htmlBody, null, MediaTypeNames.Text.Html);

                MailMessage mail = new MailMessage();
                mail.AlternateViews.Add(avHtml);

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = PI_Smtp;
                if (PI_Puerto != "")
                {
                    smtpClient.Port = Convert.ToInt32(PI_Puerto);
                }
                if (PI_CredencialPassw != "")
                {
                    NetworkCredential basicCredential = new NetworkCredential(PI_CredencialUsr, PI_CredencialPassw);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                }
                smtpClient.EnableSsl = aplicassl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                int x2 = 0;
                while (x2 < PI_ArrPDF.Count)
                {

                    if (!(PI_ArrPDF == null))
                    {
                        Attachment attPdf = new Attachment(new MemoryStream(PI_ArrPDF[x2].lisadjunto[0]), PI_ArrPDF[x2].NombreAdjunto);
                        mail.Attachments.Add(attPdf);
                    }

                    x2++;
                }
                //if (!(PI_ArrXml == null))
                //{ Attachment attXml = new Attachment(new MemoryStream(PI_ArrXml), PI_NombreXml);
                //mail.Attachments.Add(attXml);
                //}
                MailAddress mailEnvia = new MailAddress(PI_UsuarioEnvia);
                mail.From = mailEnvia;

                string BanderaCorreoTest = System.Configuration.ConfigurationManager.AppSettings["CorreoDestinatarioTest"];

                if (!string.IsNullOrEmpty(BanderaCorreoTest))
                {
                    PI_UsuariosDestinos = BanderaCorreoTest;
                }

                string[] lv_UsuariosDestino = PI_UsuariosDestinos.Split(';');
                MailAddress mailDestino;
                RegexUtilities lv_Regex = new RegexUtilities();
                foreach (string lv_UsuarioDestino in lv_UsuariosDestino)
                {
                    if (lv_Regex.IsValidEmail(lv_UsuarioDestino.Trim()))
                    {
                        mailDestino = new MailAddress(lv_UsuarioDestino);
                        mail.To.Add(mailDestino);
                    }
                }

                mail.Subject = PI_MotivoCorreo;
                mail.IsBodyHtml = true;
                smtpClient.Timeout = 10000000;
                smtpClient.Send(mail);

                lv_Retorno = true;
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                lv_Retorno = false;
            }
            return lv_Retorno;
        }


        public class AdjuntoEnvio
        {
            public List<byte[]> lisadjunto = new List<byte[]>();
            public String NombreAdjunto = "";

        }

    }
}
