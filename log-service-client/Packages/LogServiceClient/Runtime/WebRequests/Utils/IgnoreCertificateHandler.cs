using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace LogServiceClient.Runtime.WebRequests.Utils {
    public sealed class IgnoreCertificateHandler : CertificateHandler {
        protected override bool ValidateCertificate(byte[] certificateData) {
            return true;
        }
    }
}
