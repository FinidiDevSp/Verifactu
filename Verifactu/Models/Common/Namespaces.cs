using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verifactu.Models.Common
{
    /// <summary>
    /// Constantes de namespaces utilizados por los esquemas de Veri*Factu.
    /// </summary>
    public static class VerifactuXmlNamespaces
    {
        public const string SoapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
        public const string XmlDsig = "http://www.w3.org/2000/09/xmldsig#";

        // XSD públicos de AEAT (ver ejemplos de la documentación oficial)
        public const string SuministroInformacion =
            "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/SuministroInformacion.xsd";

        public const string SuministroLR =
            "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/SuministroLR.xsd";

        public const string ConsultaLR =
            "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/ConsultaLR.xsd";

        public const string RespuestaSuministro =
            "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/RespuestaSuministro.xsd";

        public const string RespuestaConsultaLR =
            "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/RespuestaConsultaLR.xsd";
        public const string RespuestaValRegistNoVeriFactu =
            "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/RespuestaValRegistNoVeriFactu.xsd";

    }
}
