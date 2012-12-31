using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation.ASource;

namespace Animation
{
    class Factory
    {
        static public IASource createDMHY() {
            return new DMHY();
        }

        static public IASource createKTXP() {
            return new KTXP();
        }

        internal static IASource create(ResourcesKind currentRK) {
            if (currentRK == ResourcesKind.DMHY) {
                return new DMHY();
            }
            return new KTXP();
        }
    }
}
