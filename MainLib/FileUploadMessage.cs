using System.ServiceModel;
using System.IO;

namespace MainLib
{
	[MessageContract]
	public class FileUploadMessage
	{
		[MessageHeader(MustUnderstand=true)]
		public string VirtualPath { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public long LastWriteTimeUtcTicks { get; set; }

        [MessageBodyMember(Order=1)]
		public Stream DataStream { get; set; }
	}
}
