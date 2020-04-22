using System;
using System.Collections.Generic;
using System.Text;

namespace SnapUi {

    public class InvalidParentException : Exception {

        public InvalidParentException()
            : base(CreateMessage()) { }

        public InvalidParentException(string message)
            : base(message) { }

        public InvalidParentException(string message, Exception innerException)
            : base(message, innerException) { }

        public InvalidParentException(Type typeOfRequired)
            : this(CreateMessage(typeOfRequired.FullName)) { }

        public InvalidParentException(Type typeOfRequired, Exception innerException)
            : this(CreateMessage(typeOfRequired.FullName), innerException) { }

        private static string CreateMessage(string? ancestor = null) =>
            "Required parent"
            + ((ancestor == null) ? " " : ":'" + ancestor + "' ")
            + "not found.";

    }
}
