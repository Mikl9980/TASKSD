using System;
using System.IO;
using System.Runtime.Serialization;

[DataContract(IsReference = true)]
public class ListNode
{
    [DataMember]
    public ListNode Previous;
    [DataMember]
    public ListNode Next;
    [DataMember]
    public ListNode Random;
    [DataMember]
    public string Data;
}

[DataContract(IsReference = true)]
public class ListRandom
{
    [DataMember]
    public ListNode Head;
    [DataMember]
    public ListNode Tail;
    [DataMember]
    public int Count;

    public void Serialize(Stream s)
    {
        var serializer = new DataContractSerializer(typeof(ListRandom));
        serializer.WriteObject(s, this);
    }

    public void Deserialize(Stream s)
    {
        var serializer = new DataContractSerializer(typeof(ListRandom));
        var newList = (ListRandom)serializer.ReadObject(s);
        Head = newList.Head;
        Tail = newList.Tail;
        Count = newList.Count;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Тестовый список
        var list = new ListRandom
        {
            Head = new ListNode { Data = "Node1" },
            Tail = new ListNode { Data = "Node2" },
            Count = 2
        };
        list.Head.Next = list.Tail;
        list.Tail.Previous = list.Head;
        list.Head.Random = list.Tail;
        list.Tail.Random = list.Head;

        // Сериализуем список в файл
        using (var fileStream = new FileStream("list.xml", FileMode.Create))
        {
            list.Serialize(fileStream);
        }

        // Десериализуем список из файла
        var newList = new ListRandom();
        using (var fileStream = new FileStream("list.xml", FileMode.Open))
        {
            newList.Deserialize(fileStream);
        }

        // Проверка 
        Console.WriteLine($"Original list count: {list.Count}");
        Console.WriteLine($"Deserialized list count: {newList.Count}");
        Console.WriteLine($"Original list head data: {list.Head.Data}");
        Console.WriteLine($"Deserialized list head data: {newList.Head.Data}");
        Console.WriteLine($"Original list tail data: {list.Tail.Data}");
        Console.WriteLine($"Deserialized list tail data: {newList.Tail.Data}");
        Console.WriteLine($"Original list head random data: {list.Head.Random.Data}");
        Console.WriteLine($"Deserialized list head random data: {newList.Head.Random.Data}");
        Console.WriteLine($"Original list tail random data: {list.Tail.Random.Data}");
        Console.WriteLine($"Deserialized list tail random data: {newList.Tail.Random.Data}");
    }
}