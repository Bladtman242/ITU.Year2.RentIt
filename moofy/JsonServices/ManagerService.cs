using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;

namespace moofy.JsonServices {
    public class ManagerService : IManagerService {

        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "promote")]
        public SuccessFlag PromoteUserToManager(int managerid, int userid) {
            if (managerid > 0 && userid > 0) {
                return new SuccessFlag() {
                    success = true,
                    message = "Both ids are valid. This has not yet been implemented."
                };
            }
            else {
                return new SuccessFlag() {
                    success = false,
                    message = "An id is not valid."
                };
            }
        }

        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "{id}/demote")]
        public SuccessFlag DemoteManagerToUser(string id, int managerid) {
            int userid = Convert.ToInt32(id);
            if (managerid > 0 && userid > 0) {
                return new SuccessFlag() {
                    success = true,
                    message = "Both ids are valid. This has not yet been implemented."
                };
            }
            else {
                return new SuccessFlag() {
                    success = false,
                    message = "An id is not valid."
                };
            }
        }

    }
}
