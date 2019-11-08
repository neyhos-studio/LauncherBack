using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherBack.Controllers.Inscription
{
    public enum RegisterEnum
    {
        nok = 0,
        ok = 1,
        passwordNoMinLength = 2,
        passwordMaxLength = 3,
        nicknameMaxLength = 4,
        nicknameWordForbidden = 5,
        mailInvalid = 6,
        nicknameTwoSpaces = 7,
        nicknameTwoDashes = 8,
        mailAlreadyExist = 9
    }
}
