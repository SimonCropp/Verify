using System.IO;

namespace VerifyTests
{
    public readonly struct Target
    {
        readonly string? stringData;
        readonly Stream? streamData;
        public string Extension { get; }
        public OnDiff? OnDiff { get; }

        public Stream StreamData
        {
            get
            {
                if (streamData == null)
                {
                    throw new("Use StringData.");
                }

                return streamData;
            }
        }

        public bool IsStream { get => streamData != null; }

        public string StringData
        {
            get
            {
                if (stringData == null)
                {
                    throw new("Use StreamData.");
                }

                return stringData;
            }
        }

        public bool IsString { get => stringData != null; }

        public Target(string extension, Stream streamData, OnDiff? onDiff)
        {
            Guard.AgainstBadExtension(extension, nameof(extension));
            Guard.AgainstNull(streamData, nameof(streamData));

            if (EmptyFiles.Extensions.IsText(extension))
            {
                throw new("Dont pass a stream for text. Instead use the `Target(string extension, string stringData)`.");
            }

            Extension = extension;
            OnDiff = onDiff;
            this.streamData = streamData;
            stringData = null;
        }

        public Target(string extension, string stringData, OnDiff? onDiff)
        {
            Guard.AgainstBadExtension(extension, nameof(extension));
            Guard.AgainstNull(stringData, nameof(stringData));
            if (!EmptyFiles.Extensions.IsText(extension))
            {
                throw new("Dont pass a stream for text. Instead use the `Target(string extension, Stream streamData)`.");
            }

            Extension = extension;
            OnDiff = onDiff;
            this.stringData = stringData;
            streamData = null;
        }
    }
}