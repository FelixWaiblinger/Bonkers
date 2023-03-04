using System;

[Serializable]
public class SerializableDictStringBool : SerializableDictionary<string, bool> {}

[Serializable]
public class SerializableDictUlongTuple : SerializableDictionary<ulong, Tuple<string, int>> {}

[Serializable]
public class SerializableDictStringInt : SerializableDictionary<string, int> {}
