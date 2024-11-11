using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eComp.PDF
{
    class ValidaFirmaDigital
    {
        public ValidaFirmaDigital() { }

        public String[] CertificarDocumento(string rutaDocumentoFirmado)
        {
            String[] DocFirmado =  new String[10];
            using (var reader = new PdfReader(rutaDocumentoFirmado))
            {
                var campos = reader.AcroFields;
                var nombresDefirmas = campos.GetSignatureNames();

                int c = 0;
                foreach (var nombre in nombresDefirmas)
                {
                    var firmaTmp = campos.VerifySignature(nombre);
                    string[] NombreFirma = firmaTmp.SigningCertificate.SubjectDN.ToString().Split(',');

                    for (int g = 0; g < NombreFirma.Length; g++)
                    {
                        if (NombreFirma[g].Contains("CN"))
                        {
                            //Console.WriteLine("Nombre " + c + ": " + NombreFirma[g]);
                            DocFirmado[c] = NombreFirma[g];

                        }
                    }

                    c++;
                }
            }

            return DocFirmado;
        }
    }
}
