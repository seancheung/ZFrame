using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (CardEntity))]
public class CardRenderer : MonoBehaviour
{
	public UISprite frameSprite;
	public UILabel nameLabel;
	public UILabel costLabel;
	public UILabel typeLabel;
	public UISprite raritySprite;
	public UILabel textLabel;

	private void Start()
	{
		Refresh();
	}

	private void Refresh()
	{
		CardData data = GetComponent<CardEntity>().data;
		nameLabel.text = data.name;
		costLabel.text = data.manaCost;
		typeLabel.text = data.type;
		textLabel.text = data.text;

		frameSprite.spriteName = CardFrameParser.Parse(data);
	}

	private static class CardFrameParser
	{
		public const string Back = "back";
		private const string Artifact = "a";
		private const string Colorless = "c";
		private const string Green = "g";
		private const string Red = "r";
		private const string Blue = "u";
		private const string White = "w";
		private const string Black = "b";
		private const string Multicolor = "m";
		private const string Land = "l";

		private static readonly Dictionary<string, string> _colorcodes = new Dictionary<string, string>
		{
			{"colorless", Colorless},
			{"green", Green},
			{"red", Red},
			{"blue", Blue},
			{"white", White},
			{"black", Black},
			{"blackgreen", Black + Green},
			{"blackred", Black + Red},
			{"greenblue", Green + Blue},
			{"greenwhite", Green + White},
			{"redgreen", Red + Green},
			{"redwhite", Red + White},
			{"blueblack", Blue + Black},
			{"bluered", Blue + Red},
			{"whiteblack", White + Black},
			{"whiteblue", White + Blue},
		};


		public static string Parse(CardData data)
		{
			if (data.colors == null)
				return GetSpriteName(null, data.types);

			switch (data.colors.Length)
			{
				case 0:
					return GetSpriteName(null, data.types);
				case 1:
					return GetSpriteName(data.colors[0], data.types);
				case 2:
					return GetSpriteName(data.colors);
				default:
					return GetSpriteName();
			}
		}

		private static string GetSpriteName(string color, string[] types)
		{
			if (types.Contains("Artifact"))
				return Artifact;

			return types.Contains("Land")
				? (string.IsNullOrEmpty(color) ? Colorless + Land : _colorcodes[color.ToLower()] + Land)
				: (string.IsNullOrEmpty(color) ? Colorless : _colorcodes[color.ToLower()]);
		}

		private static string GetSpriteName(string[] colors)
		{
			//hybrid?
			return Multicolor + Reorder(_colorcodes[colors[0].ToLower()] + _colorcodes[colors[1].ToLower()]);
		}

		private static string Reorder(string colors)
		{
			return
				colors.Replace(Green + Black, Black + Green).
					Replace(Red + Black, Black + Red)
					.Replace(Blue + Green, Green + Blue)
					.Replace(White + Green, Green + White)
					.Replace(Green + Red, Red + Green)
					.Replace(White + Red, Red + White)
					.Replace(Black + Blue, Blue + Black)
					.Replace(Red + Blue, Blue + Red)
					.Replace(Black + White, White + Black)
					.Replace(Blue + White, White + Blue);
		}

		private static string GetSpriteName()
		{
			return Multicolor;
		}
	}
}