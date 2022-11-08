﻿using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Guids;

/* This code is taken from jhtodd/SequentialGuid https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs */

/// <summary>
/// Implements <see cref="IGuidGenerator"/> by creating sequential Guids.
/// Use <see cref="AbpSequentialGuidGeneratorOptions"/> to configure.
/// </summary>
public class SequentialGuidGenerator : IGuidGenerator, ITransientDependency
{
    public AbpSequentialGuidGeneratorOptions Options { get; }

    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    public SequentialGuidGenerator(IOptions<AbpSequentialGuidGeneratorOptions> options)
    {
        Options = options.Value;
    }

    public virtual Guid Create()
    {
        return Create(Options.GetDefaultSequentialGuidType());
    }

    public virtual Guid Create(SequentialGuidType guidType)
    {
        // We start with 16 bytes of cryptographically strong random data.
        var randomBytes = new byte[8];
        RandomNumberGenerator.GetBytes(randomBytes);

        // An alternate method: use a normally-created GUID to get our initial
        // random data:
        // byte[] randomBytes = Guid.NewGuid().ToByteArray();
        // This is faster than using RNGCryptoServiceProvider, but I don't
        // recommend it because the .NET Framework makes no guarantee of the
        // randomness of GUID data, and future versions (or different
        // implementations like Mono) might use a different method.

        // Now we have the random basis for our GUID.  Next, we need to
        // create the six-byte block which will be our timestamp.

        // We start with the number of milliseconds that have elapsed since
        // DateTime.MinValue.  This will form the timestamp.  There's no use
        // being more specific than milliseconds, since DateTime.Now has
        // limited resolution.

        // Old:
        // Using millisecond resolution for our 48-bit timestamp gives us
        // about 5900 years before the timestamp overflows and cycles.
        // Hopefully this should be sufficient for most purposes. :)
        // long timestamp = DateTime.UtcNow.Ticks / 10000L;

        // New:
        // the timestamp generated in the case of high concurrency may be the same,
        // resulting in the Guid generated at the same time not being sequential.
        // See: https://github.com/abpframework/abp/issues/11453
        long timestamp = DateTime.UtcNow.Ticks;

        // Backward compatibility.
        long milliseconds = timestamp / 10000L;
        long remainderTicks = timestamp % 10000L;

        // Then get the bytes
        byte[] timestampBytes = new byte[8];
        byte[] millisecondsBytes = BitConverter.GetBytes(milliseconds);
        byte[] remainderTicksBytes = BitConverter.GetBytes(remainderTicks);

        // Merge two arrays
        Buffer.BlockCopy(millisecondsBytes, 0, timestampBytes, 2, 6);
        Buffer.BlockCopy(remainderTicksBytes, 0, timestampBytes, 0, 2);

        // Since we're converting from an Int64, we have to reverse on
        // little-endian systems.
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        byte[] guidBytes = new byte[16];

        switch (guidType)
        {
            case SequentialGuidType.SequentialAsString:
            case SequentialGuidType.SequentialAsBinary:

                // For string and byte-array version, we copy the timestamp first, followed
                // by the random data.
                Buffer.BlockCopy(timestampBytes, 0, guidBytes, 0, 8);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 8, 8);

                // If formatting as a string, we have to compensate for the fact
                // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
                // respectively.  That means that it switches the order on little-endian
                // systems.  So again, we have to reverse.
                if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                    Array.Reverse(guidBytes, 6, 2);
                }

                break;

            case SequentialGuidType.SequentialAtEnd:

                // For sequential-at-the-end versions, we copy the random data first,
                // followed by the timestamp.
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 8);

                // MSSQL and System.Data.SqlTypes.SqlGuid sort first by the last 6 Data4 bytes, left to right,
                // then the first two bytes of Data4 (again, left to right),
                // then Data3, Data2, and Data1 right to left.
                Buffer.BlockCopy(timestampBytes, 6, guidBytes, 8, 2);
                Buffer.BlockCopy(timestampBytes, 0, guidBytes, 10, 6);
                break;
        }

        return new Guid(guidBytes);
    }
}
