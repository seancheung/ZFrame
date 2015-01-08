using System.IO;
using System.Text;
using System.Xml.Serialization;
using ZFrame.Debugger;

namespace ZFrame.GUI
{
	[XmlRoot]
	public sealed class ZUI
	{
		[XmlAttribute]
		public UIType type;
		[XmlElement]
		public UISize maxSize;
		[XmlElement]
		public UISize minSize;
		[XmlAttribute]
		public ZoomStyle zoomStyle;

		[XmlArray]
		public ZPanel[] panels;

		public string Serialize()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				XmlSerializer ser = new XmlSerializer(GetType());
				ser.Serialize(stream, this);
				return Encoding.UTF8.GetString(stream.ToArray());
			}
		}
	}

	[XmlInclude(typeof(ZButton)), XmlInclude(typeof(ZLabel)), XmlInclude(typeof(ZImage)), XmlInclude(typeof(ZPanel))]
	public abstract class UIItem
	{
		[XmlAttribute]
		public int depth;
		[XmlElement]
		public UIPosition position;
		[XmlElement]
		public UISize size;

		[XmlArray]
		public UIItem[] items;
	}

	public sealed class ZPanel : UIItem
	{

	}

	public class ZButton : UIItem
	{
		[XmlAttribute]
		public string name;
	}

	public class ZLabel : UIItem
	{
		[XmlAttribute]
		public string content;
		[XmlAttribute]
		public int fontSize;
		[XmlAttribute]
		public string font;
	}

	public class ZImage : UIItem
	{
		[XmlAttribute]
		public string src;
	}

	public struct UIPosition
	{
		[XmlAttribute]
		public float x;
		[XmlAttribute]
		public float y;
		[XmlAttribute]
		public float z;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UIPosition"/> class.
		/// </summary>
		public UIPosition(float x, float y, float z) : this()
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UIPosition"/> class.
		/// </summary>
		public UIPosition(float x, float y) : this()
		{
			this.x = x;
			this.y = y;
		}
	}

	public enum UIType
	{
		TwoD,
		ThreeD
	}

	public struct UISize
	{
		[XmlAttribute]
		public int height;
		[XmlAttribute]
		public int width;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UISize"/> class.
		/// </summary>
		public UISize(int width, int height) : this()
		{
			this.width = width;
			this.height = height;
		}
	}

	public enum ZoomStyle
	{
		AspRatio,
		Fixed,
		Free
	}
}