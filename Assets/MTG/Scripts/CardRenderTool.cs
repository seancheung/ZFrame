using System.Collections.Generic;
using System.Linq;
using ZFrame;

public class CardRenderTool : MonoSingleton<CardRenderTool>
{
	private static readonly Dictionary<string, string> Types = new Dictionary<string, string>
	{
		{"creature", CardType.Creature},
		{"artifact", CardType.Artifact},
		{"enchantment", CardType.Enchantment},
		{"instant", CardType.Instant},
		{"sorcery", CardType.Sorcery},
		{"planeswalker", CardType.PlanesWalker},
		{"land", CardType.Land},
	};

	private static readonly Dictionary<string, string> Colorcodes = new Dictionary<string, string>
	{
		{"colorless", CardFrame.Colorless},
		{"green", CardFrame.Green},
		{"red", CardFrame.Red},
		{"blue", CardFrame.Blue},
		{"white", CardFrame.White},
		{"black", CardFrame.Black},
		{"blackgreen", CardFrame.Black + CardFrame.Green},
		{"blackred", CardFrame.Black + CardFrame.Red},
		{"greenblue", CardFrame.Green + CardFrame.Blue},
		{"greenwhite", CardFrame.Green + CardFrame.White},
		{"redgreen", CardFrame.Red + CardFrame.Green},
		{"redwhite", CardFrame.Red + CardFrame.White},
		{"blueblack", CardFrame.Blue + CardFrame.Black},
		{"bluered", CardFrame.Blue + CardFrame.Red},
		{"whiteblack", CardFrame.White + CardFrame.Black},
		{"whiteblue", CardFrame.White + CardFrame.Blue},
	};


	public static string ParseType(string[] types)
	{
		IEnumerable<string> res = types.Select(t => Types[t.ToLower().Trim()]);
		return res.Aggregate((a, b) => a + b);
	}

	public static string ParseFrame(CardData data)
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
			return CardFrame.Artifact;

		return types.Contains("Land")
			? (string.IsNullOrEmpty(color) ? CardFrame.Colorless + CardFrame.Land : Colorcodes[color.ToLower()] + CardFrame.Land)
			: (string.IsNullOrEmpty(color) ? CardFrame.Colorless : Colorcodes[color.ToLower()]);
	}

	private static string GetSpriteName(string[] colors)
	{
		//hybrid?
		return CardFrame.Multicolor + Reorder(Colorcodes[colors[0].ToLower()] + Colorcodes[colors[1].ToLower()]);
	}

	private static string Reorder(string colors)
	{
		return
			colors.Replace(CardFrame.Green + CardFrame.Black, CardFrame.Black + CardFrame.Green).
				Replace(CardFrame.Red + CardFrame.Black, CardFrame.Black + CardFrame.Red)
				.Replace(CardFrame.Blue + CardFrame.Green, CardFrame.Green + CardFrame.Blue)
				.Replace(CardFrame.White + CardFrame.Green, CardFrame.Green + CardFrame.White)
				.Replace(CardFrame.Green + CardFrame.Red, CardFrame.Red + CardFrame.Green)
				.Replace(CardFrame.White + CardFrame.Red, CardFrame.Red + CardFrame.White)
				.Replace(CardFrame.Black + CardFrame.Blue, CardFrame.Blue + CardFrame.Black)
				.Replace(CardFrame.Red + CardFrame.Blue, CardFrame.Blue + CardFrame.Red)
				.Replace(CardFrame.Black + CardFrame.White, CardFrame.White + CardFrame.Black)
				.Replace(CardFrame.Blue + CardFrame.White, CardFrame.White + CardFrame.Blue);
	}

	private static string GetSpriteName()
	{
		return CardFrame.Multicolor;
	}

	public static class CardType
	{
		public const string MultiType = "[M]";
		public const string Creature = "[C]";
		public const string Artifact = "[A]";
		public const string Enchantment = "[E]";
		public const string Instant = "[I]";
		public const string Sorcery = "[S]";
		public const string PlanesWalker = "[P]";
		public const string Land = "[L]";
	}

	public static class CardFrame
	{
		public const string Back = "back";
		public const string Artifact = "a";
		public const string Colorless = "c";
		public const string Green = "g";
		public const string Red = "r";
		public const string Blue = "u";
		public const string White = "w";
		public const string Black = "b";
		public const string Multicolor = "m";
		public const string Land = "l";
	}
}