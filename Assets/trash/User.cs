using System;

public abstract class User {
	private static int _counter;
	public int Id;

	protected User() {
		Id = _counter;
		_counter++;
	}

	protected abstract User UpdateFromJson(string json);

	public static User GetUserFromJson(string json) => GetUserFromJson(json, GetUserType(json));

	public static User GetUserFromJson(string json, Type userType) {
		switch (userType.Name) {
			case nameof(Partner):
				// return Partner.FromJson(json);
				return new Partner().UpdateFromJson(json);
			case nameof(Merchant):
				// return Merchant.FromJson(json);
				return new Merchant().UpdateFromJson(json);
			default: return default;
		}
	}

	private static Type GetUserType(string json) { throw new NotImplementedException(); }
}

public interface IPartner {
	string Name { get; set; }
	int Old { get; set; }
}

public class Partner : User, IPartner {
	public string Name { get; set; }
	public int Old { get; set; }

	public Partner() { }

	public Partner(string name, int old) : base() {
		Name = name;
		Old = old;
	}

	// public static User FromJson(string json) {
	// 	var name = JsonUtils.GetValue<string>(json, nameof(Name));
	// 	var old = JsonUtils.GetValue<int>(json, nameof(Old));
	// 	return new Partner(name, old);
	// }

	protected override User UpdateFromJson(string json) {
		Name = JsonUtils.GetValue<string>(json, nameof(Name));
		Old = JsonUtils.GetValue<int>(json, nameof(Old));
		return this;
	}
}

public interface IMerchant {
	string Name { get; set; }
	float Height { get; set; }
}

public class Merchant : User, IMerchant {
	public string Name { get; set; }
	public float Height { get; set; }

	public Merchant() { }

	public Merchant(string name, float height) : base() {
		Name = name;
		Height = height;
	}

	// public static User FromJson(string json) {
	// 	var name = JsonUtils.GetValue<string>(json, "name");
	// 	var old = JsonUtils.GetValue<float>(json, "old");
	// 	return new Merchant(name, old);
	// }

	protected override User UpdateFromJson(string json) {
		Name = JsonUtils.GetValue<string>(json, nameof(Name));
		Height = JsonUtils.GetValue<float>(json, nameof(Height));
		return this;
	}
}

public static class JsonUtils {
	public static T GetValue<T>(string json, string key) { throw new NotImplementedException(); }
}

public static class Class {
	// private static Type userType;
	public static void Main(string[] args) { Click(); }

	private static void Click() {
		// userType = typeof(Partner);
		Login();
	}

	private static void Login() { throw new NotImplementedException(); }

	private static void Receive(string json) {
		var user = User.GetUserFromJson(json);
		// var user = User.GetUserFromJson(json, userType);
	}
}