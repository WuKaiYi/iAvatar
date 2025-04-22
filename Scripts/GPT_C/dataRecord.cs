using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Student
{
    public string Name { get; set; }
    public string GroupName { get; set; }
    public Vector2 Position { get; set; }
    public List<Member> GroupMembers { get; set; }

    public Student()
    {
        GroupMembers = new List<Member>();
    }
}

[Serializable]
public class Member
{
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public float Distance { get; set; }
}
