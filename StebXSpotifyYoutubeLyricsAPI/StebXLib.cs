using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace StebXSpotifyYoutubeLyricsAPI
{
    public static class StebGetLyricsAPI
    {
        public static string oFull;
        public static string oArtist;
        public static string oSong;
        public static string oTitle;
        public static string oLink;
        public static string oCode;
        public static bool isChanged;
        public static bool isPaused;
        public static string LyricsDatabase = "http://www.metrolyrics.com/"; //https://www.azlyrics.com/ //http://www.metrolyrics.com/

        public static class Misc
        {
            public static string GetTrackInfo(string process)
            {
                var proc = Process.GetProcessesByName(process).FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

                if (proc == null)
                    return process + " is not running!";

                if (string.Equals(proc.MainWindowTitle, process, StringComparison.InvariantCultureIgnoreCase))
                {
                    isPaused = true;
                    return "Paused";
                }
                else
                    isPaused = false;

                return proc.MainWindowTitle;
            }
            public static string RemoveSpecialChar(string input, bool removespace, bool link)
            {
                if (link == true)
                {
                    string[] special = new string[] { " ", ",", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", "|", "[", "]", "'", ";" };

                    for (int i = 0; i < special.Length; i++)
                        if (input.Contains(special[i]))
                            input = input.Replace(special[i], string.Empty);
                }
                else
                {
                    if (removespace == false)
                    {
                        string[] special = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "'" };

                        for (int i = 0; i < special.Length; i++)
                            if (input.Contains(special[i]))
                                input = input.Replace(special[i], string.Empty);
                    }

                    if (removespace == true)
                    {
                        string[] special = new string[] { " ", ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "'" };

                        for (int i = 0; i < special.Length; i++)
                            if (input.Contains(special[i]))
                                input = input.Replace(special[i], string.Empty);
                    }
                }

                return input;
            }
            public static string SplitFeaturing(string iSong, bool spotify)
            {
                if (spotify)
                {
                    if (LyricsDatabase == "https://www.azlyrics.com")
                    {
                        if (iSong.Contains("("))
                        {
                            iSong = iSong.Replace(" ", string.Empty).Replace("'", string.Empty).ToLower().Replace("(", "-");
                            string[] t2 = Regex.Split(iSong, "-");
                            iSong = t2[0];
                        }
                        else
                            iSong = iSong.Replace(" ", string.Empty).Replace("'", string.Empty).ToLower();
                    }
                    if (LyricsDatabase == "http://www.metrolyrics.com/")
                    {
                        if (iSong.Contains("("))
                        {
                            iSong = iSong.Replace("'", string.Empty).ToLower().Replace("(", "-");
                            string[] t2 = Regex.Split(iSong, "-");
                            iSong = t2[0];
                        }
                        else
                            iSong = iSong.Replace("'", string.Empty).ToLower();
                    }
                }
                else
                {
                    if (iSong.Contains("ft."))
                    {
                        iSong = iSong.Replace(" ", string.Empty).Replace("'", string.Empty).ToLower().Replace("ft.", "/");
                        string[] t2 = Regex.Split(iSong, "/");
                        iSong = t2[0];
                    }
                    else if (iSong.Contains("feat."))
                    {
                        iSong = iSong.Replace(" ", string.Empty).Replace("'", string.Empty).ToLower().Replace("feat.", "/");
                        string[] t2 = Regex.Split(iSong, "/");
                        iSong = t2[0];
                    }
                    else
                        iSong = iSong.Replace(" ", string.Empty).Replace("'", string.Empty).ToLower();
                }

                return iSong.Replace("(officialmusicvideo)", string.Empty);
            }
            public static string GetLink()
            {
                string FullLink = "";

                if (LyricsDatabase == "https://www.azlyrics.com/lyrics/")
                    FullLink = LyricsDatabase + oArtist.ToLower() + "/" + oTitle + ".html";
                if (LyricsDatabase == "http://www.metrolyrics.com/")
                    FullLink = (LyricsDatabase + oTitle.TrimEnd() + "-lyrics-" + oArtist.ToLower().TrimEnd() + ".html").Replace(" ", "-");

                return FullLink;
            }
            public static string GetRange(int position)
            {
                //azlyrics

                if (position == 0)
                    return "<!-- Usage of azlyrics.com content by any third-party lyrics provider is prohibited by our licensing agreement. Sorry about that. -->";
                if (position == 1)
                    return "<!-- MxM banner -->";

                //metrolyrics

                if (position == 2)
                    return "<!-- First Section -->";
                if (position == 3)
                    return "<!--WIDGET - RELATED-->";
                if (position == 4)
                    return "<!-- Second Section -->";
                if (position == 5)
                    return "<!--WIDGET - PHOTOS-->";
                if (position == 6)
                    return "<!-- Third Section -->";
                if (position == 7)
                    return "<!--BOTTOM MPU-->";

                return "";
            }
            public static string FilterCode()
            {
                #region azlyrics
                try
                {
                    if (LyricsDatabase == "https://www.azlyrics.com/lyrics/")
                    {
                        string[] special = new string[] { "<div>", "<br>", "</i>", "[:]", "e:]", "<i>", "</div>", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", "|", "'", ";" };

                        for (int i = 0; i < special.Length; i++)
                            if (oCode.Contains(special[i]))
                                oCode = oCode.Replace(special[i], string.Empty);

                        string filter1 = oCode.Substring(oCode.IndexOf(GetRange(0)));
                        string filter2 = "";

                        int resultIndex = filter1.IndexOf(GetRange(1));
                        if (resultIndex != -1)
                            filter2 = filter1.Substring(0, resultIndex);

                        return filter2.Replace(GetRange(0), string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    LyricsDatabase = "http://www.metrolyrics.com/";
                    Reset();
                }
                #endregion
                #region metrolyrics
                try
                {
                    if (LyricsDatabase == "http://www.metrolyrics.com/")
                    {
                        string[] special = new string[] { "<p class=verse>", "<div>", "<br>", "</i>", "[:]", "e:]", "<i>", "</div>", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", "|", "'", ";", "</p>" };

                        for (int i = 0; i < special.Length; i++)
                            if (oCode.Contains(special[i]))
                                oCode = oCode.Replace(special[i], string.Empty);

                        //First Section
                        string filter1 = oCode.Substring(oCode.IndexOf(GetRange(2)));
                        string section1 = "";

                        int resultIndex = filter1.IndexOf(GetRange(3));
                        if (resultIndex != -1)
                            section1 = filter1.Substring(0, resultIndex);

                        //Second Section
                        string filter2 = oCode.Substring(oCode.IndexOf(GetRange(4)));
                        string section2 = "";

                        int resultIndex2 = filter2.IndexOf(GetRange(5));
                        if (resultIndex2 != -1)
                            section2 = filter2.Substring(0, resultIndex2);

                        //Third Section
                        string filter3 = oCode.Substring(oCode.IndexOf(GetRange(6)));
                        string section3 = "";

                        int resultIndex3 = filter3.IndexOf(GetRange(7));
                        if (resultIndex3 != -1)
                            section3 = filter3.Substring(0, resultIndex3);

                        string fulllyrics = section1 + section2 + section3;

                        string[] special2 = new string[] { "<!-- First Section -->", "<!--WIDGET - RELATED-->", "<!-- Second Section -->", "<!--WIDGET - PHOTOS-->", "<!-- Third Section -->", "<!--BOTTOM MPU-->" };

                        for (int i = 0; i < special2.Length; i++)
                            if (fulllyrics.Contains(special2[i]))
                                fulllyrics = fulllyrics.Replace(special2[i], string.Empty);

                        return fulllyrics;
                    }
                }
                catch (Exception ex)
                {
                    LyricsDatabase = "https://www.azlyrics.com/";
                    Reset();
                }
                #endregion

                return "[Database is not responding]";
            }
            public static void Reset()
            {
                isChanged = false;
            }
        }

        public static class Spotify
        {
            public static void GetInformation(Label SongTitle)
            {
                try
                {
                    string info = Misc.GetTrackInfo("Spotify");
                    string[] info2 = Regex.Split(info, " - ");
                    oArtist = Misc.RemoveSpecialChar(info2[0], false, false);
                    oTitle = Misc.SplitFeaturing(info2[1], true);
                    oLink = Misc.RemoveSpecialChar(Misc.GetLink(), false, true);
                    oFull = info;
                    oSong = info2[1];

                    SongTitle.TextChanged += new EventHandler(isTextChanged);
                }
                catch
                { }
            }
            public static string GetLyrics()
            {
                if (isChanged == false)
                {
                    try
                    {
                        using (WebClient client = new WebClient())
                            oCode = client.DownloadString(oLink);
                        isChanged = true;
                    }
                    catch
                    { }
                }

                if (isPaused)
                    return "Paused";

                return Misc.FilterCode();
            }
            public static void isTextChanged(object sender, EventArgs e)
            {
                Misc.Reset();
            }
        }

        public static class Youtube
        {
            public static void GetInformation(Label SongTitle)
            {
                try
                {
                    string info = Misc.GetTrackInfo("Chrome");
                    string[] info2 = Regex.Split(info, " - ");
                    oArtist = Misc.RemoveSpecialChar(info2[0], false, false);
                    oTitle = Misc.SplitFeaturing(info2[1], false);
                    oLink = Misc.RemoveSpecialChar(Misc.GetLink(), false, true);
                    oFull = info.Replace("YouTube", string.Empty).Replace("Google Chrome", string.Empty).Replace("-", string.Empty);
                    oSong = info2[1];

                    SongTitle.TextChanged += new EventHandler(isTextChanged);
                }
                catch
                { }
            }
            public static string GetLyrics()
            {
                if (isChanged == false)
                {
                    try
                    {
                        using (WebClient client = new WebClient())
                            oCode = client.DownloadString(oLink);
                        isChanged = true;
                    }
                    catch
                    { }
                }

                if (isPaused)
                    return "Paused";

                return Misc.FilterCode();
            }
            public static void isTextChanged(object sender, EventArgs e)
            {
                Misc.Reset();
            }
        }
    }
}
