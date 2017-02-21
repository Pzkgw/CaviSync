using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace MainLib
{
	[MessageContract]
	public class FileUploadMessage
	{
		[MessageHeader(MustUnderstand=true)]
		public string VirtualPath { get; set; }

		[MessageBodyMember(Order=1)]
		public Stream DataStream { get; set; }
	}
}
