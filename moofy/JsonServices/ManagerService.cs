using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.JsonServices {
    public partial class MoofyServices : IManagerService {

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
