using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;

namespace moofy.JsonServices {
    class Utils {
        //sets the HTTP status code to 400, with desc as description
        private void BadReq(string desc) {
            WebOperationContext cctx = WebOperationContext.Current;
            cctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            cctx.OutgoingResponse.StatusDescription = desc;
        }

        //sets the HTTP status code to 404, with desc as description
        private void NotFound(string desc) {
            WebOperationContext cctx = WebOperationContext.Current;
            cctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            cctx.OutgoingResponse.StatusDescription = desc;
        }
    }
}
