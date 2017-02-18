﻿using System.Security.Cryptography;

namespace Arci.Networking.Tests.NetCore
{
    public static class RSAKey
    {
        public static readonly byte[] Modulus =
        {
            0xb4,0x2c,0xaa,0x6f,0xd4,0x50,0xac,0xbf,0x1d,0x26,0xa7,0x03,0xbb,0xf0,
            0xaa,0xf0,0x3b,0xfb,0x61,0x2c,0x2f,0x02,0x14,0x03,0xdd,0x5c,0x92,0xbd,0x37,
            0x4c,0x4f,0xb3,0xb8,0x1e,0x2d,0x35,0x69,0x55,0xc7,0x0a,0x17,0x69,0x5f,0x13,
            0xd4,0x46,0x58,0x36,0x90,0x82,0x34,0x65,0x04,0x52,0xbe,0x9a,0xdd,0xe8,0xab,
            0x89,0x25,0x5c,0xe8,0xc8,0xee,0xe8,0xf2,0x0b,0xd3,0xe5,0x4f,0x5d,0x57,0xaf,
            0xc7,0xdb,0x9b,0x27,0xea,0x10,0x1c,0x06,0x5d,0x73,0xba,0x4a,0xbd,0xb2,0xb7,
            0xbf,0x4b,0x2b,0x19,0xc3,0x61,0x58,0xeb,0x2f,0x4d,0xe8,0x06,0x8e,0x8f,0xce,
            0x0a,0x95,0xcb,0x7f,0xcd,0x00,0xad,0xfe,0x84,0x7a,0x7c,0xee,0x71,0xeb,0xf2,
            0x25,0xed,0x51,0xbe,0x9c,0x28,0x73,0xe2,0x9c,0x4c,0xea,0xd1,0xfc,0xf7,0xfd,
            0x02,0x2e,0xcb,0x6c,0x6e,0xc5,0xe5,0xd7,0x18,0x89,0xef,0xc0,0xf1,0xe1,0x78,
            0xb5,0xe2,0x4c,0x26,0x27,0x0b,0xcc,0x8b,0x11,0xbe,0x70,0xaa,0x27,0x87,0x57,
            0x9f,0x87,0x20,0xab,0x13,0x8f,0xc7,0x3e,0xa2,0x9b,0x9f,0xbc,0xdd,0x74,0xc6,
            0x97,0xef,0xc4,0x62,0x4a,0x5b,0x01,0x18,0xa6,0x51,0x0b,0x1e,0x6d,0x32,0xf4,
            0x5c,0x66,0x51,0xbb,0x2d,0x3b,0x80,0x45,0x7d,0xf4,0x96,0xfd,0x62,0x66,0x9c,
            0x45,0xfa,0x7a,0x53,0x5c,0x32,0xae,0x32,0x33,0x7e,0x0f,0xe9,0x30,0x8b,0x70,
            0x1c,0x71,0xe7,0xa0,0x88,0x26,0x64,0x00,0xae,0xf5,0x39,0x14,0x18,0xf0,0xf8,
            0xbf,0x41,0x98,0x94,0x84,0x40,0xb6,0xc7,0x33,0x22,0x3f,0x05,0x7d,0xb2,0xbb,
            0xf7,0x1d
        };

        public static readonly byte[] PublicExponent = { 1, 0, 1 };

        private static readonly byte[] privateExponent =
        {
            0x96,0x2c,0x24,0x62,0xd1,0x33,0xec,0xc7,0xde,0x24,0x39,0x50,0x83,0x75,
            0x35,0x04,0xc6,0xf9,0xdf,0x24,0x54,0x8a,0x06,0xe4,0xb1,0xbc,0x57,0x12,0x1e,
            0xe5,0x1c,0x09,0x4f,0x8c,0xd7,0x61,0x8d,0x4a,0x51,0x7b,0xb7,0xc7,0xbb,0xd7,
            0x6b,0x36,0xb6,0x8f,0xc4,0x22,0xc2,0x48,0xf0,0x31,0x87,0x6c,0xcd,0x49,0x00,
            0xb0,0x6e,0xd3,0xe1,0xb2,0x98,0x6e,0xd0,0x4f,0xcb,0x6d,0x75,0x98,0xf7,0x5d,
            0x2b,0xd7,0x7e,0x9a,0xe6,0x1c,0x47,0x3f,0x86,0xe4,0xce,0x81,0x3d,0x5b,0x98,
            0x8b,0x78,0xbb,0x93,0xdd,0xa5,0x65,0xd7,0xa0,0xb5,0xee,0x8e,0x88,0x84,0x93,
            0xd7,0x7e,0xc2,0xfe,0xe2,0x7f,0x8e,0x86,0x75,0x25,0x22,0xbc,0x1b,0x77,0xed,
            0x7c,0x6f,0x31,0x93,0x0d,0xd5,0x29,0xc1,0xd5,0xde,0x65,0xcc,0x76,0x19,0x5a,
            0x7c,0x2d,0xb4,0x2f,0xe6,0x94,0x4a,0xd0,0x30,0xf6,0xc2,0x9b,0xc1,0x1e,0x39,
            0x5b,0x74,0x9e,0xf9,0x23,0x4d,0xdc,0xf9,0x12,0xed,0x2d,0x4b,0x0a,0xd9,0xa7,
            0x45,0x2f,0x80,0x23,0x6b,0xe0,0xce,0x8e,0xdc,0xa1,0xe7,0xd4,0x39,0x5e,0x4f,
            0x67,0xe8,0x10,0x0e,0xef,0xc7,0x1e,0x08,0xa0,0x4d,0x61,0x7b,0x36,0x89,0x1d,
            0x67,0x6a,0x6e,0xff,0x03,0x22,0x8d,0xb6,0x1e,0x26,0xf6,0xb4,0xc2,0xd2,0xf3,
            0x6e,0x35,0xf3,0x12,0x74,0xce,0xa0,0xe5,0x3a,0x04,0xae,0x9a,0x00,0xa4,0xb2,
            0xdf,0xd6,0x99,0x7a,0xec,0x34,0xaa,0x4a,0xef,0xcb,0x5b,0x25,0x35,0xb8,0xfb,
            0x88,0xa7,0xc1,0x2a,0x5f,0x58,0xa5,0x64,0xa3,0xbc,0x25,0x81,0x9f,0x5e,0x86,
            0x1e,0x81
        };

        private static readonly byte[] prime1 =
        {
            0xe0,0xa8,0x8c,0x51,0x8f,0x5a,0xde,0x58,0x81,0x4c,0x1f,0x95,0x60,0x36,
            0xf1,0x5a,0x38,0x97,0x63,0xb3,0x09,0x04,0xf8,0x6c,0x2e,0x7f,0x65,0x7d,0x39,
            0x06,0x0c,0x6c,0x5b,0x1a,0xe3,0x18,0xf3,0x48,0xfb,0x54,0x6a,0x4a,0x3a,0x98,
            0xe3,0x95,0x9d,0x4a,0x27,0xb8,0xce,0x88,0x46,0x9f,0xfd,0x1a,0xe2,0x49,0x0e,
            0xfa,0x3d,0xd9,0x47,0x45,0x9d,0xde,0xbb,0x46,0x49,0xbc,0x88,0x36,0x23,0xc5,
            0x9e,0x81,0x43,0x92,0x80,0x81,0x38,0x26,0x45,0xb4,0x3d,0x15,0x58,0xd7,0x77,
            0x23,0x8b,0x46,0x5a,0x87,0x39,0x19,0x71,0x13,0x53,0xab,0xb7,0xba,0xe3,0x14,
            0x04,0xf3,0x17,0xa2,0x00,0xf2,0x84,0x40,0x97,0x97,0x23,0xb3,0x98,0x97,0x22,
            0x10,0x7b,0x34,0x32,0xee,0x5f,0x89,0xc9,0x5b
        };

        private static readonly byte[] prime2 =
        {
            0xcd,0x4f,0x6b,0x2e,0x74,0xb4,0x09,0x1a,0x99,0x78,0xd1,0x0b,0x65,0xcf,
            0x90,0x48,0x84,0xd5,0xda,0xb1,0xd6,0x26,0x93,0x37,0xc9,0x27,0x3d,0xed,0xce,
            0x98,0xc9,0xdc,0x73,0xfa,0x9f,0x82,0xc3,0xd4,0xd7,0x6a,0x40,0x45,0xb9,0xb7,
            0x17,0x20,0xae,0xf5,0xed,0xbc,0x87,0x1a,0xa6,0x07,0xcb,0x04,0x2d,0x38,0x09,
            0xfa,0xdb,0x3f,0x4c,0x2b,0x4f,0xb4,0x5e,0xf3,0x88,0xec,0xdc,0xf3,0xe2,0xc1,
            0xf4,0x4f,0x62,0x95,0xa7,0x85,0x74,0x9d,0x7c,0xd4,0x25,0x16,0x41,0x5d,0xe9,
            0x46,0xf0,0xf2,0x81,0x64,0x7d,0xbd,0x14,0x01,0x2d,0x71,0xb6,0x8d,0x4a,0xa3,
            0x0e,0x62,0x5c,0x4e,0x78,0xbb,0xc9,0x85,0x3f,0x54,0x96,0xe7,0xbc,0xf2,0x84,
            0x3f,0xbe,0x3e,0x92,0xa6,0x45,0x12,0xb2,0xe7
        };

        private static readonly byte[] exponent1 =
        {
            0x1c,0xb8,0x2f,0x47,0xf5,0xe5,0x8d,0xeb,0x0e,0x8c,0x66,0xb5,0x37,0xd9,0x8e,
            0x3d,0x14,0x62,0xf6,0x11,0xdb,0x23,0x51,0x86,0xb2,0xe3,0x02,0x5c,0x61,0xbf,
            0xce,0x32,0xc3,0xea,0xca,0x01,0x54,0x88,0x8d,0xe8,0x9a,0xb5,0xe6,0x8b,0xc8,
            0xfc,0x45,0x61,0x47,0x76,0xae,0xa0,0x69,0x36,0xe6,0xaa,0x5b,0x27,0x2f,0xcc,
            0xf9,0xbf,0x1f,0x07,0x5d,0x49,0x2d,0xf3,0xac,0x55,0x77,0xac,0x44,0x22,0x6d,
            0x42,0xe5,0x1a,0x83,0x67,0x01,0x80,0x93,0x04,0x99,0x92,0x73,0x0b,0x08,0x65,
            0xf8,0xd6,0x03,0x98,0xa9,0xca,0x00,0xd4,0x91,0xab,0xb0,0x0f,0x2a,0x1c,0x53,
            0x0d,0xa8,0x85,0xc3,0x4d,0x3d,0x6e,0xec,0x72,0x1b,0xd4,0x47,0xbb,0x7a,0x55,
            0x55,0x0a,0xb5,0x40,0x17,0xe2,0x8b,0x65
        };

        private static readonly byte[] exponent2 =
        {
            0x15,0x72,0x5c,0x61,0xe7,0xf2,0xfe,0x98,0x76,0xbb,0xb6,0x2a,0x98,0xa2,0x0c,
            0x12,0x67,0x3d,0xe7,0xb0,0x78,0x0c,0x63,0x88,0x8a,0x4c,0xbd,0x1d,0x60,0x5d,
            0x79,0x88,0xbb,0xdc,0xcc,0x58,0xde,0x98,0x17,0x40,0x94,0x22,0x34,0x7a,0x39,
            0xc2,0x42,0x44,0x92,0x67,0x05,0x3d,0xf5,0x66,0x0f,0x01,0x0a,0xb0,0x35,0xea,
            0xac,0x88,0x7a,0x2e,0x74,0x0f,0x05,0x74,0x2f,0x33,0x7f,0x09,0x43,0x00,0xbb,
            0xc2,0xa7,0x2b,0xb6,0xea,0x2b,0xfa,0x5f,0x95,0xd1,0xa1,0xf7,0x86,0x15,0xde,
            0x41,0x41,0x33,0xdf,0x09,0x7b,0x7c,0xe1,0x7a,0x9b,0xcc,0x54,0x0a,0x1d,0xea,
            0x5c,0xc0,0x03,0xba,0x7c,0xf3,0x5a,0x25,0xf9,0x72,0x22,0x17,0x0c,0xc6,0x5f,
            0xda,0x41,0xb3,0x14,0x07,0x57,0x22,0xb5
        };

        private static readonly byte[] coefficient =
        {
            0x52,0xa0,0xb8,0x2c,0x49,0x01,0x84,0xa5,0xcb,0x4a,0x54,0xe7,0x0f,0x6b,0x64,
            0xee,0x66,0xfc,0xda,0x53,0x88,0x09,0xa5,0xb2,0xbc,0x04,0x0c,0x4c,0x54,0xe3,
            0x46,0x4f,0x3b,0x8f,0x07,0x65,0x48,0xfd,0xd6,0x44,0x59,0x66,0x5f,0xb8,0x12,
            0x0f,0x5a,0x3d,0x74,0xaa,0x38,0x44,0x61,0x54,0x6a,0xcf,0x1c,0x94,0x19,0xa3,
            0x81,0x05,0x10,0xb1,0x81,0xc6,0xb3,0x9b,0x09,0x34,0xc1,0x6e,0x55,0x64,0x9c,
            0x85,0xf0,0x59,0x87,0xf3,0x32,0xab,0x04,0x53,0x77,0x99,0x71,0x01,0x5d,0x3e,
            0xdb,0x3c,0x58,0xbf,0x71,0xc5,0x8b,0x21,0x35,0x33,0x65,0xb3,0x2b,0xa7,0x2b,
            0x12,0xe5,0x7e,0xa3,0xcd,0x96,0x45,0x5f,0xbc,0x32,0xcf,0xa0,0x7c,0x0f,0x18,
            0x11,0x67,0x9b,0xad,0x5b,0xed,0xee,0x7f
        };

        public static RSAParameters RsaParams
        {
            get
            {
                RSAParameters rsaParams = new RSAParameters();
                rsaParams.Modulus = Modulus;
                rsaParams.Exponent = PublicExponent;
                rsaParams.D = privateExponent;
                rsaParams.P = prime1;
                rsaParams.Q = prime2;
                rsaParams.DP = exponent1;
                rsaParams.DQ = exponent2;
                rsaParams.InverseQ = coefficient;
                return rsaParams;
            }
        }
    }
}
