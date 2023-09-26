using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public string DisplayName { get; set; }
    public string Handle { get; set; }
    public string UserId { get; set; }
    public string ProfilePicture {  get; set; }
    public List<string> Followers { get; set; }
    public List<string> Following { get; set; }
    public string Title { get; set; }
    public long LastSignIn { get; set; }
    public int StoriesRead { get; set; }
    public int GamesPlayed { get; set; }
    public float StoryXp { get; set; }
    public float GameXp { get; set; }
    public int StoryTitlesWon { get; set; }
    public int GameTitlesWon { get; set; }
    public string CountryOfOrigin { get; set; }
}
