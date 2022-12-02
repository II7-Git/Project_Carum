using System;

[Serializable]
public class MusicItem
{
    public string id;
    public string artist;
    public string emotion_tag;
    public string resource;
    public string title;

    public MusicItem(string id, string artist, string emotion_tag, string resource, string title)
    {
        this.id = id;
        this.artist = artist;
        this.emotion_tag = emotion_tag;
        this.resource = resource;
        this.title = title;
    }
    public MusicItem(string id, string artist, string title, string resource)
    {
        this.id = id;
        this.artist = artist;
        this.emotion_tag = "";
        this.resource = resource;
        this.title = title;
    }
}
