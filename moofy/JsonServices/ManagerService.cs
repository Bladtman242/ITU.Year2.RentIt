using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.JsonServices {
    public partial class MoofyServices : IManagerService {

        public SuccessFlag PromoteUserToManager(int managerid, int userid) {
            if (managerid > 0 && userid > 0) {
                db.Open();
                bool suc = db.PromotetoAdmin(managerid, userid);
                db.Close();
                return new SuccessFlag() {
                    success = suc,
                    message = suc ? "" : "Couldn't promote user, is " + managerid +" a manager?"
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
            int userid;
            try {
                userid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlag() { success = false, message = id + " is a weird number" };
            }
            if (managerid > 0 && userid > 0) {
                db.Open();
                bool suc = db.DemoteAdmin(managerid, userid);
                db.Close();
                return new SuccessFlag() {
                    success = suc,
                    message = suc ? "" : "Couldn't promote user, are both users managers?"
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
