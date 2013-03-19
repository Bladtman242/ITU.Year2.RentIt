using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public struct Purchase {
        private File file;
        private DateTime endTime;

        public Purchase(File file, DateTime endTime) {
            this.file = file;
            this.endTime = endTime;
        }

        /// <summary>
        /// The File purchased
        /// </summary>
        public File File {
            get { return file; }
        }

        /// <summary>
        /// When the purchase expires
        /// </summary>
        public DateTime EndTime {
            get { return endTime; }
        }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter is null return false.
            if (obj == null || !(obj is Purchase)) {
                return false;
            }

            // If parameter cannot be cast to File return false.
            Purchase p = (Purchase) obj;
            // Return true if the fields match:
            return (File.Equals(p.File))
                    && (EndTime.Equals(p.EndTime));
        }

        /// <summary>
        /// A supplement to the normal equals method, which takes a File parameter for performance.
        /// It is recommended by Microsoft, that in addition to implementing Equals(object), Equals(type) should also be implemented for the class's own type in order to enhance performance.
        /// </summary>
        /// <param name="f">The File to compare</param>
        /// <returns>True if the two objects are equal</returns>
        public bool Equals(Purchase p) {
            // If parameter is null return false:
            if ((object)p == null) {
                return false;
            }

            // Return true if the fields match:
            return (File.Equals(p.File))
                   && (EndTime.Equals(p.EndTime));
        }

        public override int GetHashCode() {
            return File.GetHashCode() ^ EndTime.GetHashCode();
        }
    }
}
