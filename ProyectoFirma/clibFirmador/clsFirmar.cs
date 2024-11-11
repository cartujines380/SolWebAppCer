using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SBXMLCore;
using SBXMLSec;
using SBXMLAdESIntf;
using SBXMLSig;
using SBHTTPSClient;
using SBHTTPTSPClient;
using SBDCXMLEnc;
using SBX509;
using SBCustomCertStorage;
using SBCertValidator;
using SBWinCertStorage;

namespace clibFirmador
{
    public class clsFirmar
    {
        private TElXMLDOMDocument FXMLDocument = null;
        public string Msg = "";
        public string Codigo = "";
        private SBCertValidator.TElX509CertificateValidator CertificateValidator;
        //private TElMemoryCertStorage CertStorage;
        public TElX509Certificate Cert;
        // RMC
        public string XMLFirmado { get; set; }
        // RMC

        public string InfoCert()
        {
            if (Cert == null)
                return "";
            return Cert.SubjectName.CommonName + " " + Cert.SubjectName.Organization + " " + Cert.GetFriendlyName();
        }

        private bool _CargarCertificado(string PI_CommonName, string PI_SerialNumber, string PI_SystemStores)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = true;
            TElWinCertStorage lv_WinCertStorage;
            TElX509Certificate lv_Cert_Temp;
            try
            {
                lv_WinCertStorage = new TElWinCertStorage();
                lv_WinCertStorage.SystemStores.Text = PI_SystemStores; // "MY";
                TElCertificateLookup lookUp = new TElCertificateLookup();
                int lv_IndiceCert = lv_WinCertStorage.FindFirst(lookUp);
                string lv_SerialNumber, lv_CommonName;
                for (int i = 0; i < lv_WinCertStorage.Count; i++)
                {
                    lv_Cert_Temp = lv_WinCertStorage.get_Certificates(i);
                    lv_SerialNumber = SBUtils.Unit.BeautifyBinaryString(SBUtils.Unit.BinaryToString(lv_Cert_Temp.SerialNumber), ' ');
                    lv_CommonName = lv_Cert_Temp.SubjectName.CommonName;
                    if (PI_SerialNumber != "")
                    {
                        if (lv_SerialNumber == PI_SerialNumber)
                        {
                            Cert = lv_Cert_Temp;
                            break;
                        }
                    }
                    if (PI_CommonName != "")
                    {
                        if (lv_CommonName == PI_CommonName)
                        {
                            Cert = lv_Cert_Temp;
                            break;
                        }
                    }
                }

            }
            catch (Exception E)
            {
                Msg = "CargarCertificado: " + E.Message;
                Codigo = "ENC001";
                Retorno = false;
            }
            return Retorno;
        }
        public bool CargarCertificado(string PI_CommonName, string PI_SerialNumber, string PI_SystemStores)
        {
            return _CargarCertificado(PI_CommonName, PI_SerialNumber, PI_SystemStores);
        }
        public bool CargarCertificado(string PI_Certificado, string PI_Clave)
        {
            return _CargarCertificado(PI_Certificado, PI_Clave);
        }

        private bool _CargarCertificado(string PI_Certificado, string PI_Clave)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = true;
            try
            {
                TElMemoryCertStorage CertStorage;
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


                FileStream F = new FileStream(PI_Certificado, FileMode.Open, FileAccess.Read);
                try
                {
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
                            CertStorage.Add(Cert, true);
                        }
                        else
                            throw new Exception("Fallo al cargar el Certificado");
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
            return _CargarCertificado(PI_Certificado, PI_Clave);
        }

        private bool _CargarCertificado(MemoryStream PI_Certificado, string PI_Clave)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = true;
            try
            {
                TElMemoryCertStorage CertStorage;
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
                            CertStorage.Add(Cert, true);
                        }
                        else
                            throw new Exception("Fallo al cargar el Certificado");
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
        private int lv_Anio = 2014;
        private int lv_Mes = 08;
        private int lv_Dia = 15;
        private int lv_Dias = 90;

        private void validar2()
        {
            DateTime endTime = DateTime.Now;
            DateTime startTime = new DateTime(lv_Anio, lv_Mes, lv_Dia);

            int lv_Validacion = 360;
            int lv_Calculo = 1;
            if (lv_Calculo > 0)
            {
                lv_Validacion = 0;
            }
            else
                lv_Validacion = 180;
            if (lv_Validacion == 0)
                lv_Validacion = 1;


            TimeSpan span = endTime.Subtract(startTime);
            if (span.Days > 1)
            {
                clsEncriptacion objE = new clsEncriptacion();
                throw new Exception(objE.Decrypt(this._Ms_1));
            }

            if (lv_Calculo > 1)
            {
                lv_Validacion = 1;
            }
            else
                lv_Validacion = 360;
            if (lv_Validacion == 1)
                lv_Validacion = 360;

        }

        private void validar()
        {
            DateTime endTime = DateTime.Now;

            int lv_Validacion = 360;
            int lv_Calculo = 1;
            if (lv_Calculo > 0)
            {
                lv_Validacion = 0;
            }
            else
                lv_Validacion = 180;
            if (lv_Validacion == 0)
                lv_Validacion = 1;

            DateTime startTime = new DateTime(lv_Anio, lv_Mes, lv_Dia);

            TimeSpan span = endTime.Subtract(startTime);
            if (span.Days > lv_Dias)
            {
                clsEncriptacion objE = new clsEncriptacion();
                throw new Exception(objE.Decrypt(this._Ms_1));
            }

            if (lv_Calculo > 1)
            {
                lv_Validacion = 1;
            }
            else
                lv_Validacion = 360;
            if (lv_Validacion == 1)
                lv_Validacion = 360;

        }

        public clsFirmar()
        {

            // Calling SetLicenseKey            
            SBUtils.Unit.SetLicenseKey("046AD8B4A1AA3AA8821BB9A027D712C969576EBD15BB6E5A0659D8BCF55990C2C931464A78F5A87E59D72DE7D430E2E88E72F34ADF3BB33CDD23759B364F3162A6C29AA1335AEAFE5FEA79B0D73310240F8783FD7FBE94AE13E8D9DE4BB6D7EA851D1A7B95719FABBD6FF355F91A976DA5E57B6BA77C7E66CED99359701026C248B55FC0D9551267E282117B807E7F284AA0A39E3F70AD62348FC872CC18982FAEEB0F32A2090D12B1C67CFC1008F9B2A6FA95605407500EC619583EFDD39BE764796757FB993DCE1B75FD5B4553AC0FCE988B0821120F4136D26B109554A46CE2B3CF6359E7C26CA1DC20DA922C5E9A1CA249F641FC9AA05F7772264B82B6A9");
            //validar();
            FXMLDocument = new TElXMLDOMDocument();

        }

        private bool _FirmarDocumento(string PI_ArchivoXmlClaro, string PI_ArchivoXmlFirmado)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = false;
            try
            {
                TElMemoryCertStorage CertStorage;
                CertStorage = new TElMemoryCertStorage();
                using (TElXMLDOMDocument Doc = new TElXMLDOMDocument())
                {
                    using (FileStream F = new FileStream(PI_ArchivoXmlClaro, FileMode.Open, FileAccess.Read))
                    {
                        Doc.LoadFromStream(F);
                    }
                    TElXMLSigner Signer = new TElXMLSigner();
                    try
                    {
                        //<!-- INICIO DE LA FIRMA DIGITAL ESPECIFICACION XADES_BES -->
                        Signer.SignatureType = SBXMLSec.Unit.xstEnveloped;//4
                        Signer.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon;//1
                        Signer.SignatureMethodType = SBXMLSec.Unit.xmtSig;//0
                        Signer.SignatureMethod = SBXMLSec.Unit.xsmRSA_SHA1;//2
                        Signer.MACMethod = 1;
                        TElXMLDOMNode SignatureNode;
                        SignatureNode = Doc.DocumentElement; //<factura

                        //<!-- HASH O DIGEST DE TODO EL ARCHIVO XML IDENTIFICADO POR EL id="comprobante"-->
                        TElXMLReference Ref_Elemento_Comprobante = new TElXMLReference();
                        Ref_Elemento_Comprobante.DigestMethod = SBXMLSec.Unit.xdmSHA1; // SBXMLSec.Unit.xsmRSA_SHA1;
                        Ref_Elemento_Comprobante.URINode = Doc.DocumentElement;
                        Ref_Elemento_Comprobante.URI = "#comprobante";
                        Ref_Elemento_Comprobante.TransformChain.Add(new SBXMLTransform.TElXMLEnvelopedSignatureTransform());
                        Signer.References.Add(Ref_Elemento_Comprobante);

                        Signer.IncludeKey = true;
                        Signer.KeyName = "";
                        TElXMLKeyInfoX509Data X509KeyData = new TElXMLKeyInfoX509Data(false);
                        X509KeyData.IncludeKeyValue = true;
                        X509KeyData.Certificate = Cert;
                        Signer.KeyData = X509KeyData;

                        TElXAdESSigner XAdESSigner;
                        XAdESSigner = new TElXAdESSigner();
                        Signer.XAdESProcessor = XAdESSigner;
                        XAdESSigner.PolicyId.SigPolicyId.Description = "";
                        XAdESSigner.PolicyId.SigPolicyId.Identifier = "";
                        XAdESSigner.PolicyId.SigPolicyId.IdentifierQualifier = SBXMLAdES.Unit.xqtNone;
                        XAdESSigner.XAdESVersion = SBXMLAdES.Unit.XAdES_v1_3_2;//3                         
                        CertStorage.Add(Cert, false);
                        XAdESSigner.SigningCertificates = CertStorage;
                        XAdESSigner.SigningTime = DateTime.UtcNow.ToLocalTime();
                        XAdESSigner.Generate(2);
                        XAdESSigner.QualifyingProperties.XAdESPrefix = "etsi";

                        Signer.OnFormatElement += FormatElement;
                        Signer.OnFormatText += FormatText;

                        Signer.UpdateReferencesDigest();
                        Signer.GenerateSignature();
                        Signer.Save(ref SignatureNode);

                    }
                    finally
                    {
                        Signer.Dispose();

                    }
                    // RMC
                    //using (FileStream F = new FileStream(PI_ArchivoXmlFirmado, FileMode.Create, FileAccess.ReadWrite))

                    //{
                    //    Doc.SaveToStream(F, SBXMLDefs.Unit.xcmNone, "");

                    //}

                    XMLFirmado = Doc.OuterXML;
                    // RMC
                }
                Retorno = true;
            }
            catch (Exception E)
            {
                Msg = "FirmarDocumento: " + E.Message;
                Codigo = "ENC002";
                Retorno = false;
            }

            return Retorno;

        }
        public bool FirmarDocumento(string PI_ArchivoXmlClaro, string PI_ArchivoXmlFirmado)
        {
            return _FirmarDocumento(PI_ArchivoXmlClaro, PI_ArchivoXmlFirmado);
        }

        private bool _FirmarDocumento(string PI_ContenidoXmlClaro, string PO_ArchivoXmlFirmado, string PO_Data)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = false;
            TElMemoryCertStorage CertStorage;
            try
            {

                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                ////System.Text.UTF8Encoding encoding = new UTF8Encoding();
                ////stream.Write(encoding.GetBytes(PI_ContenidoXmlClaro), 0, PI_ContenidoXmlClaro.Length);
                stream = new MemoryStream(Encoding.UTF8.GetBytes(PI_ContenidoXmlClaro));

                CertStorage = new TElMemoryCertStorage();
                using (TElXMLDOMDocument Doc = new TElXMLDOMDocument())
                {

                    //TElXMLDOMDocument Document = new TElXMLDOMDocument();
                    //TElXMLDOMElement Element = SBXMLUtils.Unit.ParseElementFromXMLString(PI_ContenidoXmlClaro, Doc);
                    //Doc.AppendChild(Element);
                    stream.Position = 0;
                    //Doc.LoadFromStream(stream, "utf-8", false);  

                    ////Doc.LoadFromStream(stream);
                    Doc.LoadFromStream(stream, "UTF-8", true);
                    TElXMLSigner Signer = new TElXMLSigner();
                    try
                    {
                        //<!-- INICIO DE LA FIRMA DIGITAL ESPECIFICACION XADES_BES -->
                        Signer.SignatureType = SBXMLSec.Unit.xstEnveloped;//4
                        Signer.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon;//1
                        Signer.SignatureMethodType = SBXMLSec.Unit.xmtSig;//0
                        Signer.SignatureMethod = SBXMLSec.Unit.xsmRSA_SHA1;//2
                        Signer.MACMethod = 1;
                        TElXMLDOMNode SignatureNode;
                        SignatureNode = Doc.DocumentElement; //<factura

                        //<!-- HASH O DIGEST DE TODO EL ARCHIVO XML IDENTIFICADO POR EL id="comprobante"-->
                        TElXMLReference Ref_Elemento_Comprobante = new TElXMLReference();
                        Ref_Elemento_Comprobante.DigestMethod = SBXMLSec.Unit.xdmSHA1; // SBXMLSec.Unit.xsmRSA_SHA1;
                        Ref_Elemento_Comprobante.URINode = Doc.DocumentElement;
                        Ref_Elemento_Comprobante.URI = "#comprobante";
                        Ref_Elemento_Comprobante.TransformChain.Add(new SBXMLTransform.TElXMLEnvelopedSignatureTransform());
                        Signer.References.Add(Ref_Elemento_Comprobante);

                        Signer.IncludeKey = true;
                        Signer.KeyName = "";
                        TElXMLKeyInfoX509Data X509KeyData = new TElXMLKeyInfoX509Data(false);
                        X509KeyData.IncludeKeyValue = true;
                        X509KeyData.Certificate = Cert;
                        Signer.KeyData = X509KeyData;

                        TElXAdESSigner XAdESSigner;
                        XAdESSigner = new TElXAdESSigner();
                        Signer.XAdESProcessor = XAdESSigner;
                        XAdESSigner.PolicyId.SigPolicyId.Description = "";
                        XAdESSigner.PolicyId.SigPolicyId.Identifier = "";
                        XAdESSigner.PolicyId.SigPolicyId.IdentifierQualifier = SBXMLAdES.Unit.xqtNone;
                        XAdESSigner.XAdESVersion = SBXMLAdES.Unit.XAdES_v1_3_2;//3                         
                        CertStorage.Add(Cert, false);
                        XAdESSigner.SigningCertificates = CertStorage;
                        XAdESSigner.SigningTime = DateTime.UtcNow.ToLocalTime();
                        XAdESSigner.Generate(2);
                        XAdESSigner.QualifyingProperties.XAdESPrefix = "etsi";

                        Signer.OnFormatElement += FormatElement;
                        Signer.OnFormatText += FormatText;

                        Signer.UpdateReferencesDigest();
                        Signer.GenerateSignature();
                        Signer.Save(ref SignatureNode);

                        XAdESSigner.Dispose();
                        X509KeyData.Dispose();

                    }
                    finally
                    {
                        Signer.Dispose();
                    }
                    XMLFirmado = Doc.OuterXML;
                }

                try
                {
                    stream.Close();
                    stream.Dispose();
                    CertStorage.Dispose();
                }
                catch { }

                Retorno = true;
            }
            catch (Exception E)
            {
                Msg = "FirmarDocumento: " + E.Message;
                Codigo = "ENC002";
                Retorno = false;
            }
            finally
            {
                try
                {

                }
                catch { }
            }

            return Retorno;
        }
        public bool FirmarDocumento(string PI_ContenidoXmlClaro, string PO_ArchivoXmlFirmado, string PO_Data)
        {
            return _FirmarDocumento(PI_ContenidoXmlClaro, PO_ArchivoXmlFirmado, PO_Data);
        }

        private bool _FirmarDocumento_Stream(string PI_ContenidoXmlClaro, string PO_ArchivoXmlFirmado, ref byte[] PO_DataSign)
        {
            Msg = "";
            Codigo = "0";
            bool Retorno = false;
            try
            {
                TElMemoryCertStorage CertStorage;
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                System.Text.UTF8Encoding encoding = new UTF8Encoding();
                stream.Write(encoding.GetBytes(PI_ContenidoXmlClaro), 0, PI_ContenidoXmlClaro.Length);
                CertStorage = new TElMemoryCertStorage();
                using (TElXMLDOMDocument Doc = new TElXMLDOMDocument())
                {

                    //TElXMLDOMDocument Document = new TElXMLDOMDocument();
                    //TElXMLDOMElement Element = SBXMLUtils.Unit.ParseElementFromXMLString(PI_ContenidoXmlClaro, Doc);
                    //Doc.AppendChild(Element);
                    stream.Position = 0;
                    //Doc.LoadFromStream(stream, "utf-8", false);                   
                    Doc.LoadFromStream(stream);
                    TElXMLSigner Signer = new TElXMLSigner();
                    try
                    {
                        //<!-- INICIO DE LA FIRMA DIGITAL ESPECIFICACION XADES_BES -->
                        Signer.SignatureType = SBXMLSec.Unit.xstEnveloped;//4
                        Signer.CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon;//1
                        Signer.SignatureMethodType = SBXMLSec.Unit.xmtSig;//0
                        Signer.SignatureMethod = SBXMLSec.Unit.xsmRSA_SHA1;//2
                        Signer.MACMethod = 1;
                        TElXMLDOMNode SignatureNode;
                        SignatureNode = Doc.DocumentElement; //<factura

                        //<!-- HASH O DIGEST DE TODO EL ARCHIVO XML IDENTIFICADO POR EL id="comprobante"-->
                        TElXMLReference Ref_Elemento_Comprobante = new TElXMLReference();
                        Ref_Elemento_Comprobante.DigestMethod = SBXMLSec.Unit.xdmSHA1; // SBXMLSec.Unit.xsmRSA_SHA1;
                        Ref_Elemento_Comprobante.URINode = Doc.DocumentElement;
                        Ref_Elemento_Comprobante.URI = "#comprobante";
                        Ref_Elemento_Comprobante.TransformChain.Add(new SBXMLTransform.TElXMLEnvelopedSignatureTransform());
                        Signer.References.Add(Ref_Elemento_Comprobante);

                        Signer.IncludeKey = true;
                        Signer.KeyName = "";
                        TElXMLKeyInfoX509Data X509KeyData = new TElXMLKeyInfoX509Data(false);
                        X509KeyData.IncludeKeyValue = true;
                        X509KeyData.Certificate = Cert;
                        Signer.KeyData = X509KeyData;

                        TElXAdESSigner XAdESSigner;
                        XAdESSigner = new TElXAdESSigner();
                        Signer.XAdESProcessor = XAdESSigner;
                        XAdESSigner.PolicyId.SigPolicyId.Description = "";
                        XAdESSigner.PolicyId.SigPolicyId.Identifier = "";
                        XAdESSigner.PolicyId.SigPolicyId.IdentifierQualifier = SBXMLAdES.Unit.xqtNone;
                        XAdESSigner.XAdESVersion = SBXMLAdES.Unit.XAdES_v1_3_2;//3                         
                        CertStorage.Add(Cert, false);
                        XAdESSigner.SigningCertificates = CertStorage;
                        XAdESSigner.SigningTime = DateTime.UtcNow.ToLocalTime();
                        XAdESSigner.Generate(2);
                        XAdESSigner.QualifyingProperties.XAdESPrefix = "etsi";

                        Signer.OnFormatElement += FormatElement;
                        Signer.OnFormatText += FormatText;

                        Signer.UpdateReferencesDigest();
                        Signer.GenerateSignature();
                        Signer.Save(ref SignatureNode);

                    }
                    finally
                    {
                        Signer.Dispose();

                    }

                    using (MemoryStream F = new MemoryStream())
                    {
                        //string F1 = Doc.OuterXML;
                        Doc.SaveToStream(F, SBXMLDefs.Unit.xcmNone, "utf-8");
                        F.Position = 0;
                        //using (BinaryReader br = new BinaryReader(F))
                        //{
                        //    PO_DataSign = new byte[Convert.ToInt32(F.Length)];                            
                        //    PO_DataSign =  br.ReadBytes(Convert.ToInt32(F.Length));
                        //}
                        PO_DataSign = F.ToArray();
                    }

                }
                Retorno = true;
            }
            catch (Exception E)
            {
                Msg = "FirmarDocumento: " + E.Message;
                Codigo = "ENC002";
                Retorno = false;
            }

            return Retorno;
        }
        public bool FirmarDocumento_Stream(string PI_ContenidoXmlClaro, string PO_ArchivoXmlFirmado, ref byte[] PO_DataSign)
        {
            return _FirmarDocumento_Stream(PI_ContenidoXmlClaro, PO_ArchivoXmlFirmado, ref PO_DataSign);
        }
        private void FormatElement(object Sender, TElXMLDOMElement Element, int Level, string Path, ref string StartTagWhitespace, ref string EndTagWhitespace)
        {
            StartTagWhitespace = "\n";
            string s = new string('\t', Level - 1);

            StartTagWhitespace = StartTagWhitespace + s;
            if (Element.FirstChild != null)
            {
                bool HasElements = false;
                TElXMLDOMNode Node = Element.FirstChild;
                while (Node != null)
                {
                    if (Node.NodeType == SBXMLCore.Unit.ntElement)
                    {
                        HasElements = true;
                        break;
                    }

                    Node = Node.NextSibling;
                }

                if (HasElements)
                    EndTagWhitespace = "\n" + s;
            }
        }

        private string _Ms_1 = "lwrrI5PbdKj7CjXAtnQB7iya1fAa/p2bHau7v1IlLdlb7B59vHi/2mt8n4j91w09aoydobMlb39jQev9Yuzo2v7aZezmKxJql+n7klmqjVMRBlc+17ftLOl/6gZuPBDKKEl6W/NAiX4PruGw9qTjkg==";

        private void FormatText(object Sender, ref string Text, short TextType, int Level, string Path)
        {
            if ((TextType == SBXMLDefs.Unit.ttBase64) && (Text.Length > 64))
            {
                string s = "\n";
                while (Text.Length > 0)
                {
                    if (Text.Length > 64)
                    {
                        s = s + Text.Substring(0, 64) + "\n";
                        Text = Text.Remove(0, 64);
                    }
                    else
                    {
                        s = s + Text + "\n";
                        Text = "";
                    }
                }

                Text = s + new string('\t', Level - 2);
            }
        }

    }
}
