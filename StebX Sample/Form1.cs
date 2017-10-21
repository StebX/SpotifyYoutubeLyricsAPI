using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StebXSpotifyYoutubeLyricsAPI;


namespace StebX_Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TIMER_Tick(object sender, EventArgs e)
        {
            //Spotify//Youtube

            //StebGetLyricsAPI.Youtube.GetInformation(label2);
            StebGetLyricsAPI.Spotify.GetInformation(label2);
            label1.Text = "Artist : " + StebGetLyricsAPI.oArtist;
            label2.Text = "Full : " + StebGetLyricsAPI.oFull;
            label3.Text = "Link : " + StebGetLyricsAPI.oLink;
            label4.Text = "Song : " + StebGetLyricsAPI.oSong;
            label5.Text = "Title : " + StebGetLyricsAPI.oTitle;
            textBox1.Text = StebGetLyricsAPI.Spotify.GetLyrics();
            //textBox1.Text = StebGetLyricsAPI.Youtube.GetLyrics();

        }
    }
}
