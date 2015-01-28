using System;
using System.Collections.Generic;

public class SetData : SetInfo
{
	/// <summary>
	/// The code that Gatherer uses for the set. Only present if different than 'code'
	/// </summary>
	public string gathererCode;

	/// <summary>
	/// An old style code used by some Magic software. Only present if different than 'gathererCode' and 'code'
	/// </summary>
	public string oldCode;

	/// <summary>
	/// The type of border on the cards, either "white", "black" or "silver"
	/// </summary>
	public string border;

	/// <summary>
	/// Type of set. One of: "core", "expansion", "reprint", 
	/// "box", "un","from the vault", "premium deck", 
	/// "duel deck","starter", "commander", "planechase", 
	/// "archenemy","promo", "vanguard", "masters"
	/// </summary>
	public string type;

	/// <summary>
	/// The block this set is in,
	/// </summary>
	public string block;

	/// <summary>
	/// Present and set to true if the set was only released online
	/// </summary>
	public string onlineOnly;

	/// <summary>
	/// Booster contents for this set, see below for details
	/// </summary>
	public object[] booster;

	/// <summary>
	/// The cards in the set
	/// </summary>
	public CardData[] cards;
}

/// <summary>
/// A unique identifier (UID) key for each card can be made by combining: setCode + cardName + imageName
/// </summary>
public class CardData : CardInfo
{
	/// <summary>
	/// The card layout. Possible values: normal, split, flip, double-faced, token, plane, scheme, phenomenon, leveler, vanguard
	/// </summary>
	public string layout;

	/// <summary>
	/// Only used for split, flip and dual cards. Will contain all the names on this card, front or back.
	/// </summary>
	public string[] names;

	/// <summary>
	/// The supertypes of the card. These appear to the far left of the card type. Example values: Basic, Legendary, Snow, World, Ongoing
	/// </summary>
	public string[] supertypes;

	/// <summary>
	/// The types of the card. These appear to the left of the dash in a card type. Example values: Instant, Sorcery, Artifact, Creature, Enchantment, Land, Planeswalker
	/// </summary>
	public string[] types;

	/// <summary>
	/// The subtypes of the card. These appear to the right of the dash in a card type. Usually each word is its own subtype. Example values: Trap, Arcane, Equipment, Aura, Human, Rat, Squirrel, etc.
	/// </summary>
	public string[] subtypes;

	/// <summary>
	/// The multiverseid of the card on Wizard's Gatherer web page.
	/// Cards from sets that do not exist on Gatherer will NOT have a multiverseid.
	/// Sets not on Gatherer are: ATH, ITP, DKM, RQS, DPA and all sets with a 4 letter code that starts with a lowercase 'p'.
	/// </summary>
	public string multiverseid;

	/// <summary>
	/// If a card has alternate art (for example, 4 different Forests, or the 2 Brothers Yamazaki) then each other variation's multiverseid will be listed here, NOT including the current card's multiverseid.
	/// </summary>
	public string[] variations;

	/// <summary>
	/// The mtgimage.com file name for this card. For AllSets and set specific JSON, use the /set or /setname prefix. For AllCards JSON use the /card prefix. See mtgimage.com for more details.
	/// </summary>
	public string imageName;

	/// <summary>
	/// The watermark on the card. Note: Split cards don't currently have this field set, despite having a watermark on each side of the split card.
	/// </summary>
	public string watermark;

	/// <summary>
	/// If the border for this specific card is DIFFERENT than the border specified in the top level set JSON, then it will be specified here. (Example: Unglued has silver borders, except for the lands which are black bordered)
	/// </summary>
	public string border;

	/// <summary>
	/// If this card was a timeshifted card in the set.
	/// </summary>
	public string timeshifted;

	/// <summary>
	/// Maximum hand size modifier. Only exists for Vanguard cards.
	/// </summary>
	public string hand;

	/// <summary>
	/// Starting life total modifier. Only exists for Vanguard cards.
	/// </summary>
	public string life;

	/// <summary>
	/// Set to true if this card is reserved by Wizards Official Reprint Policy
	/// </summary>
	public string reserved;

	/// <summary>
	/// The date this card was released. This is only set for promo cards. The date may not be accurate to an exact day and month, thus only a partial date may be set (YYYY-MM-DD or YYYY-MM or YYYY). Some promo cards do not have a known release date.
	/// </summary>
	public string releaseDate;

	/// <summary>
	/// The rulings for the card. An array of objects, each object having 'date' and 'text' keys.
	/// </summary>
	public Ruling[] rulings;

	/// <summary>
	/// Foreign language names for the card. An array of objects, each object having 'language' and 'name' keys. Only present if different than the english card name.
	/// </summary>
	public ForeignName[] foreignNames;

	/// <summary>
	/// The sets that this card was printed in.
	/// </summary>
	public string[] printings;

	/// <summary>
	/// The original text on the card at the time it was printed. This field is not available for promo cards.
	/// </summary>
	public string originalText;

	/// <summary>
	/// The original type on the card at the time it was printed. This field is not available for promo cards.
	/// </summary>
	public string originalType;

	///// <summary>
	///// Which formats this card is legal, restricted or banned in. The object's keys are the format and the values are the legality.
	///// </summary>
	public Dictionary<string, string> legalities;

	/// <summary>
	/// For promo cards, this is where this card was originally obtained. For box sets that are theme decks, this is which theme deck the card is from.
	/// </summary>
	public string source;

	public struct Ruling
	{
		public string date;
		public string text;
	}

	public struct ForeignName
	{
		public string language;
		public string name;
	}
}

[Serializable]
public class CardInfo
{
	/// <summary>
	/// The card name. For split, double-faced and flip cards, just the name of one side of the card. Basically each 'sub-card' has its own record.
	/// </summary>
	public string name;

	/// <summary>
	/// The mana cost of this card. Consists of one or more
	/// </summary>
	public string manaCost;

	/// <summary>
	/// Converted mana cost. Always a number. NOTE: cmc may have a decimal point as cards from unhinged may contain "half mana" (such as 'Little Girl' with a cmc of 0.5)
	/// </summary>
	public string cmc;

	/// <summary>
	/// The card colors. Usually this is derived from the casting cost, but some cards are special (like the back of dual sided cards and Ghostfire).
	/// </summary>
	public string[] colors;

	/// <summary>
	/// The card type. This is the type you would see on the card if printed today. Note: The dash is a UTF8 'long dash' as per the MTG rules
	/// </summary>
	public string type;

	/// <summary>
	/// The rarity of the card. Examples: Common, Uncommon, Rare, Mythic Rare, Special, Basic Land
	/// </summary>
	public string rarity;

	/// <summary>
	/// The text of the card. May contain mana symbols and other symbols.
	/// </summary>
	public string text;

	/// <summary>
	/// The card number. This is printed at the bottom-center of the card in small text. This is a string, not an integer, because some cards have letters in their numbers.
	/// </summary>
	public string number;

	/// <summary>
	/// The power of the card. This is only present for creatures. This is a string, not an integer, because some cards have powers like: "1+*"
	/// </summary>
	public string power;

	/// <summary>
	/// The toughness of the card. This is only present for creatures. This is a string, not an integer, because some cards have toughness like: "1+*"
	/// </summary>
	public string toughness;

	/// <summary>
	/// The loyalty of the card. This is only present for planeswalkers.
	/// </summary>
	public string loyalty;

	/// <summary>
	/// The flavor text of the card.
	/// </summary>
	public string flavor;

	/// <summary>
	/// The artist of the card. This may not match what is on the card as MTGJSON corrects many card misprints.
	/// </summary>
	public string artist;
}

[Serializable]
public class SetInfo
{
	/// <summary>
	/// The name of the set
	/// </summary>
	public string name;

	/// <summary>
	/// The code name of the set
	/// </summary>
	public string code;

	/// <summary>
	/// When the set was released (YYYY-MM-DD). For promo sets, the date the first card was released.
	/// </summary>
	public string releaseDate;
}