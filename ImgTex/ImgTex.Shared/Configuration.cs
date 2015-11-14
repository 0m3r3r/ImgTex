using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace ImgTex
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "ImgTex";

        List<Senaryo> scenarios = new List<Senaryo>
        {
            new Senaryo() { Title="ImgTex Görüntüyü Metin'e Çevir", ClassType=typeof(ExtractText)},
            new Senaryo() { Title="Metin Dil Çevirici", ClassType=typeof(Translate)},
        };
    }

    public class Senaryo
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}
