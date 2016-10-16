using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1/GPL 2.0/LGPL 2.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is HashTableHashing.SuperFastHash.
 *
 * The Initial Developer of the Original Code is
 * Davy Landman.
 * Portions created by the Initial Developer are Copyright (C) 2009
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *
 *
 * Alternatively, the contents of this file may be used under the terms of
 * either the GNU General Public License Version 2 or later (the "GPL"), or
 * the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
 * in which case the provisions of the GPL or the LGPL are applicable instead
 * of those above. If you wish to allow use of your version of this file only
 * under the terms of either the GPL or the LGPL, and not to allow others to
 * use your version of this file under the terms of the MPL, indicate your
 * decision by deleting the provisions above and replace them with the notice
 * and other provisions required by the GPL or the LGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file under
 * the terms of any one of the MPL, the GPL or the LGPL.
 *
 * ***** END LICENSE BLOCK ***** */
//source code from http://landman-code.blogspot.ru/

//using System;
//using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
//using System.Text;
//using System.Numerics;

namespace KVStorage
{
    public class HashFNV
    {
        internal ulong CreateHash64bit(byte[] bytes)
        {
            return CreateHash64bit(ref bytes);
        }
        internal ulong CreateHash64bit(ref byte[] bytes)
        {
            const ulong fnv64Offset = 14695981039346656037;
            const ulong fnv64Prime = 0x100000001b3;
            ulong hash = fnv64Offset;

            int i = 0, ilen = bytes.Length;
            for (i = 0; i < ilen; i += 8)//partial unroll
            {
                if (ilen > i + 7)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 2];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 3];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 4];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 5];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 6];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 7];
                    hash *= fnv64Prime;
                }
                else if (ilen > i + 6)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 2];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 3];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 4];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 5];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 6];
                    hash *= fnv64Prime;
                }
                else if (ilen > i + 5)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 2];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 3];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 4];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 5];
                    hash *= fnv64Prime;
                }
                else if (ilen > i + 4)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 2];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 3];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 4];
                    hash *= fnv64Prime;
                }
                else if (ilen > i + 3)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 2];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 3];
                    hash *= fnv64Prime;
                }
                else if (ilen > i + 2)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 2];
                    hash *= fnv64Prime;
                }
                else if (ilen > i + 1)
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                    hash ^= bytes[i + 1];
                    hash *= fnv64Prime;
                }
                else
                {
                    hash ^= bytes[i];
                    hash *= fnv64Prime;
                }
            }

            return hash;
        }
    }
}