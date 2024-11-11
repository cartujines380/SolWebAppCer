using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SBXMLCore;
using SBXMLSec;
using SBXMLAdESIntf;
using SBXMLSig;
using SBHTTPSClient;
using SBHTTPTSPClient;
using SBPDF;
using SBDCXMLEnc;
using SBX509;
using SBCustomCertStorage;
using SBCertValidator;
using SBWinCertStorage;
using System.IO;

namespace clibFirmador
{
    public class clsEstablecimiento
    {
        public string Msg = "";
        public string Codigo = "";
        private SBCertValidator.TElX509CertificateValidator CertificateValidator;
        private TElMemoryCertStorage CertStorage;
        public TElX509Certificate Cert;
        public DateTime lv_FechaExpiracion_Desde;
        public DateTime lv_FechaExpiracion_Hasta;
        public string lv_ResponsableFirma;
        public string lv_CertificadoPor;
        public string RUC;

        public clsEstablecimiento(string PI_RUC, MemoryStream PI_Certificado, string PI_Clave)
        {
            // Calling SetLicenseKey            
            SBUtils.Unit.SetLicenseKey("046AD8B4A1AA3AA8821BB9A027D712C969576EBD15BB6E5A0659D8BCF55990C2C931464A78F5A87E59D72DE7D430E2E88E72F34ADF3BB33CDD23759B364F3162A6C29AA1335AEAFE5FEA79B0D73310240F8783FD7FBE94AE13E8D9DE4BB6D7EA851D1A7B95719FABBD6FF355F91A976DA5E57B6BA77C7E66CED99359701026C248B55FC0D9551267E282117B807E7F284AA0A39E3F70AD62348FC872CC18982FAEEB0F32A2090D12B1C67CFC1008F9B2A6FA95605407500EC619583EFDD39BE764796757FB993DCE1B75FD5B4553AC0FCE988B0821120F4136D26B109554A46CE2B3CF6359E7C26CA1DC20DA922C5E9A1CA249F641FC9AA05F7772264B82B6A9");
            RUC = PI_RUC;
            if (!CargarCertificado(PI_Certificado, PI_Clave))
            {
                throw new Exception(Msg);
            }
        }

        public bool CargarCertificado_Old(MemoryStream PI_Certificado, string PI_Clave)
        {
 
            Msg = "";
            Codigo = "0";
            bool Retorno = true;
            try
            {
                CertStorage = new TElMemoryCertStorage();
                TElXAdESVerifier XAdESVerifier; TElXMLVerifier Verifier;
                this.CertificateValidator = new SBCertValidator.TElX509CertificateValidator();
                this.CertificateValidator.CheckCRL = true;
                this.CertificateValidator.CheckOCSP = true;
                this.CertificateValidator.CheckValidityPeriodForTrusted = true;
                this.CertificateValidator.IgnoreCAKeyUsage = false;
                this.CertificateValidator.IgnoreSystemTrust = false;
                this.CertificateValidator.MandatoryCRLCheck = true;
                this.CertificateValidator.MandatoryOCSPCheck = true;
                this.CertificateValidator.Tag = null;


                DateTime SigningTime = DateTime.Now;

                Verifier = new TElXMLVerifier();
                XAdESVerifier = new TElXAdESVerifier();
                Verifier.XAdESProcessor = XAdESVerifier;


                MemoryStream F = PI_Certificado;
                try
                {
                    //CertStorage.LoadFromBufferPFX(System.IO.File.ReadAllBytes(@"C:\Users\dmunoz.DOMSIPECOM\Documents\cert\SIPECOM\carlos_yimmy_sanchez_pineda_Sipecom.p12"), PI_Clave);
                    //TElX509Certificate  cert1= CertStorage.get_Certificates(0);
                    //TElX509Certificate  cert2 = CertStorage.get_Certificates(1);

                    //if  CertStorage.Count  
                    //int CertFormat = TElX509Certificate.DetectKeyFileFormat(F, PI_Clave);                                        

                    int CertFormat = TElX509Certificate.DetectCertFileFormat(F);

                    F.Position = 0;
                    Cert = new TElX509Certificate();
                    try
                    {
                        if (CertFormat == SBX509.Unit.cfDER)
                        {
                            Cert.LoadFromStream(F, (int)F.Length);
                            CertStorage.Add(Cert, true);
                        }
                        else if (CertFormat == SBX509.Unit.cfPEM)
                        {
                            Cert.LoadFromStreamPEM(F, PI_Clave, (int)F.Length);
                            CertStorage.Add(Cert, true);
                        }
                        else if (CertFormat == SBX509.Unit.cfPFX)
                        {
                            Cert.LoadFromStreamPFX(F, PI_Clave, (int)F.Length);
                            //Cert.LoadFromStreamPFX(F, PI_Clave, (int)F.Length);
                            CertStorage.Add(Cert, true);
                        }
                        else
                            throw new Exception("Fallo al cargar el Certificado");

                        int i; TElX509Certificate cert_tmp;
                        for (i = 0; i <= CertStorage.Count - 1; i++)
                        {
                            cert_tmp = CertStorage.get_Certificates(i);
                            if (((cert_tmp.Extensions.Included & SBX509Ext.Unit.ceKeyUsage) > 0))
                            {
                                CertStorage.get_Certificates(i).Clone(Cert, true);
                            }
                        }


                        lv_FechaExpiracion_Desde = Cert.ValidFrom;
                        lv_FechaExpiracion_Hasta = Cert.ValidTo;
                    }
                    catch (Exception ex1)
                    {
                        Cert.Dispose();
                        Msg = ex1.Message;
                        Codigo = "ENC001";
                        Retorno = false;
                    }
                }
                finally
                {
                    F.Close();
                }

                //Retorno = true;
            }
            catch (Exception E)
            {
                Msg = "CargarCertificado: " + E.Message;
                Codigo = "ENC001";
                Retorno = false;
            }
            return Retorno;

        }

        public bool CargarCertificado(MemoryStream PI_Certificado, string PI_Clave)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = true;
            try
            {

                CertStorage = new TElMemoryCertStorage();
                TElXAdESVerifier XAdESVerifier; TElXMLVerifier Verifier;
                this.CertificateValidator = new SBCertValidator.TElX509CertificateValidator();
                this.CertificateValidator.CheckCRL = true;
                this.CertificateValidator.CheckOCSP = true;
                this.CertificateValidator.CheckValidityPeriodForTrusted = true;
                this.CertificateValidator.IgnoreCAKeyUsage = false;
                this.CertificateValidator.IgnoreSystemTrust = false;
                this.CertificateValidator.MandatoryCRLCheck = true;
                this.CertificateValidator.MandatoryOCSPCheck = true;
                this.CertificateValidator.Tag = null;


                DateTime SigningTime = DateTime.Now;

                Verifier = new TElXMLVerifier();
                XAdESVerifier = new TElXAdESVerifier();
                Verifier.XAdESProcessor = XAdESVerifier;


                MemoryStream F = PI_Certificado;
                try
                {
                    int lv_CertCount;
                    bool lv_EncontroCert = false;
                    Cert = new TElX509Certificate();
                    PI_Certificado.Position = 1;
                    lv_CertCount = CertStorage.LoadFromBufferPFX(PI_Certificado.ToArray(), PI_Clave);
                    DateTime desde = DateTime.Now.AddMonths(-300);

                    for (int i = 0; i < CertStorage.Count; i++)
                    {
                        TElX509Certificate cert1 = CertStorage.get_Certificates(i);
                        if (((cert1.Extensions.Included & SBX509Ext.Unit.ceKeyUsage) > 0))
                        {
                            //if (cert1.SubjectName.CommonName.Contains("OSWALDO EDISSON CHAVEZ PONCE"))
                            //{
                            //    if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                            //    {
                            //        CertStorage.get_Certificates(1).Clone(Cert, true);
                            //        lv_EncontroCert = true;
                            //        break;
                            //    }
                            //}
                            //if (cert1.SubjectName.CommonName.Contains("QUEPER"))
                            //{
                            //    if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                            //    {
                            //        CertStorage.get_Certificates(1).Clone(Cert, true);
                            //        lv_EncontroCert = true;
                            //        break;
                            //    }
                            //}
                            ////BARREIRO ALCIVAR
                            //if (cert1.SubjectName.CommonName.Contains("BARREIRO ALCIVAR"))
                            //{
                            //    if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                            //    {
                            //        CertStorage.get_Certificates(2).Clone(Cert, true);
                            //        lv_EncontroCert = true;
                            //        break;
                            //    }
                            //}
                            //if (cert1.SubjectName.CommonName.Contains("ESTRADA MONAR"))
                            //{
                            //    if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                            //    {
                            //        CertStorage.get_Certificates(1).Clone(Cert, true);
                            //        lv_EncontroCert = true;
                            //        break;
                            //    }
                            //}
                            ////ARAGUNDI BARBERAN
                            //if (cert1.SubjectName.CommonName.Contains("ARAGUNDI BARBERAN"))
                            //{
                            //    if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                            //    {
                            //        CertStorage.get_Certificates(1).Clone(Cert, true);
                            //        lv_EncontroCert = true;
                            //        break;
                            //    }
                            //}

                            //if (cert1.SubjectName.CommonName.Contains("NELLY DOLORES PINARGOTE CARDENAS"))
                            //{
                            //    if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                            //    {
                            //        CertStorage.get_Certificates(0).Clone(Cert, true);
                            //        lv_EncontroCert = true;
                            //        break;
                            //    }
                            //}
                           
                            if (!cert1.SubjectName.CommonName.Contains("BANCO CENTRAL"))
                            {
                               

                                if (cert1.ValidFrom >= desde) {
                                    desde = cert1.ValidFrom;
                               
                                if (cert1.ValidTo.AddDays(-2) > DateTime.Now)
                                {
                                    CertStorage.get_Certificates(i).Clone(Cert, true);
                                    lv_EncontroCert = true;
                                 //   break;
                                }
                                }
                            }
                        }
                    }
                   /* if (!lv_EncontroCert)
                        throw new Exception("Certificado Expirado");
                    */
                    lv_FechaExpiracion_Desde = Cert.ValidFrom;
                    lv_FechaExpiracion_Hasta = Cert.ValidTo;
                    lv_ResponsableFirma = Cert.SubjectName.CommonName;
                    lv_CertificadoPor = Cert.IssuerName.CommonName;

                }
                catch (Exception ex1)
                {
                    Cert.Dispose();
                    Msg = ex1.Message;
                    Codigo = "ENC001";
                    Retorno = false;
                }
                finally
                {
                    F.Close();
                }

                //Retorno = true;
            }
            catch (Exception E)
            {
                Msg = "CargarCertificado: " + E.Message;
                Codigo = "ENC001";
                Retorno = false;
            }
            return Retorno;

}
    }
}
